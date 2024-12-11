using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    #region params

    private ColorTarget _colour;

    private float _speed;
    private int _score;
    private int _lifePoints;

    private bool _hasShield;
    private int _shieldLifePoints;
    private ColorTarget _shieldColour;

    private bool _canFire;
    private FireMode _fireMode;
    private ProjectileTypeSO _projectileTypeSo;
    private TargetingMode _targetingMode;
    private float _fireAngleRange;
    private float _fireRate;

    private int _nbBurstProjectiles;
    private float _burstProjectileAngleSpacing;

    #endregion

    [CanBeNull] private Player _targetPlayer;

    private GameObject _shield;
    private MeshRenderer _shieldRenderer;

    private SplineContainer _spline;
    private float3 _splineRelativePosition;

    private bool _ready = false;
    private bool _completedSpline = false;

    private float _fireTimer;
    private float _splineTraveledDistance;

    public void Initialize(EnemyTypeSO enemyTypeSo, SplineContainer spline, Vector3 splineRelativePosition)
    {
        //Params
        _colour = enemyTypeSo.colour;

        _speed = enemyTypeSo.speed;
        _score = enemyTypeSo.score;
        _lifePoints = enemyTypeSo.lifePoints;

        //Shield params
        _shield = transform.GetChild(0).gameObject;
        _hasShield = enemyTypeSo.hasShield;
        _shieldColour = enemyTypeSo.shieldColour;
        _shield.SetActive(_hasShield);
        if (_hasShield)
        {
            _shieldRenderer = _shield.GetComponent<MeshRenderer>();
            _shieldRenderer.material = enemyTypeSo.shieldMaterial;

            _shieldLifePoints = enemyTypeSo.shieldLifePoints;
        }

        //Projectile params
        _canFire = enemyTypeSo.canFire;
        if (_canFire)
        {
            _projectileTypeSo = enemyTypeSo.projectileTypeSo;
            _fireMode = enemyTypeSo.fireMode;
            _targetingMode = enemyTypeSo.targetingMode;
            _fireAngleRange = enemyTypeSo.fireAngleRange;
            _fireRate = enemyTypeSo.fireRate;
            _fireTimer = _fireRate;

            _nbBurstProjectiles = enemyTypeSo.nbBurstProjectiles;
            _burstProjectileAngleSpacing = enemyTypeSo.burstProjectileAngleSpacing;

            switch (_colour)
            {
                case ColorTarget.YELLOW:
                    _targetPlayer = GameManager.instance.GetPlayerFromColour(ColorTarget.PURPLE);
                    break;
                case ColorTarget.PURPLE:
                    _targetPlayer = GameManager.instance.GetPlayerFromColour(ColorTarget.YELLOW);
                    break;
            }
        }

        _spline = spline;
        _splineRelativePosition = splineRelativePosition;


        _ready = true;
    }

    private void Update()
    {
        if (_ready)
        {
            Move();

            if (_targetingMode == TargetingMode.Locked || _targetingMode == TargetingMode.PredictedLocked || !_canFire)
                Rotate();

            if (_canFire)
                Firing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bound"))
            Destroy(gameObject);

        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            PlayerProjectile playerProjectile = other.gameObject.GetComponent<PlayerProjectile>();
            int damage = playerProjectile.GetDamage(_hasShield, _shieldColour);
            if (damage != 0)
                TakeHit(damage, playerProjectile.GetParent());

            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.attachedRigidbody.SendMessage("OnTakeDamage", 1, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Rotate()
    {
        if (_targetPlayer)
            transform.LookAt(_targetPlayer.transform.position);
    }

    private void Move()
    {
        if (_fireMode == FireMode.Homing)
        {
            //Should always be looking at player
            transform.position += transform.forward * (Time.deltaTime * _speed);
        }
        else
        {
            if (!_completedSpline)
            {
                _splineTraveledDistance += Time.deltaTime * _speed;
                float splineProgression = _splineTraveledDistance / _spline.Spline.GetLength();
                transform.position = _spline.Spline.EvaluatePosition(splineProgression) + _splineRelativePosition;
                if (splineProgression >= 1f)
                {
                    _completedSpline = true;
                    OnSplineCompleted();
                }
            }
        }
    }

    private void Firing()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
        {
            switch (_fireMode)
            {
                case FireMode.Single:
                    FireSingle();
                    break;
                case FireMode.Burst:
                    FireBurst();
                    break;
            }
        }
    }

    private void FireSingle()
    {
        Quaternion spawnRotation = GetRotatationFromTargetMode(_targetingMode);

        GameObject spawnedProjectile = Instantiate(_projectileTypeSo.prefab, transform.position, spawnRotation);
        spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileTypeSo, _colour);

        _fireTimer = _fireRate;
    }

    private void FireBurst()
    {
        Quaternion baseSpawnRotation = GetRotatationFromTargetMode(_targetingMode);
        Quaternion spawnRotation;

        for (int i = -_nbBurstProjectiles; i <= _nbBurstProjectiles; i++)
        {
            spawnRotation = Quaternion.Euler(baseSpawnRotation.eulerAngles.x + (_burstProjectileAngleSpacing * i), baseSpawnRotation.eulerAngles.y, baseSpawnRotation.eulerAngles.z);
            GameObject spawnedProjectile = Instantiate(_projectileTypeSo.prefab, transform.position, spawnRotation);
            spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileTypeSo, _colour);
        }

        _fireTimer = _fireRate;
    }

    private Quaternion GetRotatationFromTargetMode(TargetingMode targetingMode)
    {
        Vector3 direction;
        Quaternion targetRotation;
        switch (targetingMode)
        {
            case TargetingMode.None:
                return Quaternion.Euler(transform.rotation.eulerAngles.x + RandomAngle(),
                    transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            case TargetingMode.Locked:
                direction = Vector3.Normalize(_targetPlayer.transform.position - transform.position);
                targetRotation = Quaternion.LookRotation(direction);
                return Quaternion.Euler(targetRotation.eulerAngles.x + RandomAngle(),
                    targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

            case TargetingMode.PredictedLocked:
                direction = Vector3.Normalize(_targetPlayer.transform.position + _targetPlayer.GetVelocity() - transform.position);
                targetRotation = Quaternion.LookRotation(direction);
                return Quaternion.Euler(targetRotation.eulerAngles.x + RandomAngle(),
                    targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        }

        Debug.LogError("No rotation found for targeting mode");
        return Quaternion.identity;
    }

    private float RandomAngle()
    {
        return Random.Range(-(_fireAngleRange / 2), _fireAngleRange / 2);
    }

    private void TakeHit(int damage, Player from)
    {
        if (_hasShield)
        {
            _shieldLifePoints -= damage;
            if (_shieldLifePoints <= 0)
                LoseShield();
        }
        else
        {
            _lifePoints -= damage;
            if (_lifePoints <= 0)
                Die(from);
        }
    }

    private void LoseShield()
    {
        _shield.SetActive(false);
        _hasShield = false;
    }

    private void Die(Player killer)
    {
        killer.AddScore(_score);
        Destroy(gameObject);
    }

    private void OnSplineCompleted()
    {
        Destroy(gameObject);
    }
}

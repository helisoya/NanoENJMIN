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
    private Transform _projectileSpawnPoint;
    private ParticleSystem _projectileParticleSystem;
    private TargetingMode _targetingMode;
    private float _fireAngleRange;
    private float _fireRate;


    private int _nbBurstProjectiles;
    private float _burstProjectileAngleSpacing;

    #endregion

    [CanBeNull] private Player _targetPlayer;

    private GameObject _shield;
    private HitFlash _shieldHitFlash;
    private MeshRenderer _shieldRenderer;

    private SplineContainer _spline;
    private float3 _splineRelativePosition;

    private bool _ready = false;
    private bool _completedSpline = false;

    private float _fireTimer;
    private float _splineTraveledDistance;

    private HitFlash _hitFlash;

    public void Initialize(EnemyTypeSO enemyTypeSo, SplineContainer spline, Vector3 splineRelativePosition)
    {
        //Params
        _colour = enemyTypeSo.colour;

        _speed = enemyTypeSo.speed;
        _score = enemyTypeSo.score;
        _lifePoints = enemyTypeSo.lifePoints;

        //Shield params
        _shield = transform.GetChild(0).gameObject;
        _shieldHitFlash = _shield.GetComponent<HitFlash>();
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
            _projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
            _projectileParticleSystem = _projectileSpawnPoint.GetComponentInChildren<ParticleSystem>();
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

        _hitFlash = GetComponent<HitFlash>();

        _ready = true;
    }

    private void Update()
    {
        if (!GameManager.instance.InGame)
        {
            return;
        }
        if (_ready)
        {
            Move();

            if (_canFire && (_targetingMode == TargetingMode.Locked || _targetingMode == TargetingMode.PredictedLocked || _fireMode == FireMode.Homing))
            {
                Rotate();
            }

            if (_canFire)
            {
                Firing();
            }
        }
    }

    public void OnPlayerProjectileHit(PlayerProjectile playerProjectile)
    {
        int damage = playerProjectile.GetDamage(_hasShield, _shieldColour);
        if (damage != 0)
        {
            TakeHit(damage, playerProjectile.GetParent());
        }
        else
        {
            EnemyManager.instance.PlayShieldNoDmgClip();
        }

        playerProjectile.DestroyProjectile();
    }

    private void Rotate()
    {
        if (_targetPlayer)
        {
            transform.LookAt(_targetPlayer.transform.position);
        }
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
                    EnemyManager.instance.PlayShotClip();
                    FireSingle();
                    break;
                case FireMode.Burst:
                    EnemyManager.instance.PlayShotClip();
                    FireBurst();
                    break;
            }
        }
    }

    private void FireSingle()
    {
        Quaternion spawnRotation = GetRotatationFromTargetMode(_targetingMode);

        GameObject spawnedProjectile = Instantiate(_projectileTypeSo.prefab, _projectileSpawnPoint.position, spawnRotation);
        spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileTypeSo, _colour);
        _projectileParticleSystem.Play();

        _fireTimer = _fireRate;
    }

    private void FireBurst()
    {
        Quaternion baseSpawnRotation = GetRotatationFromTargetMode(_targetingMode);
        Quaternion spawnRotation;

        for (int i = -_nbBurstProjectiles; i <= _nbBurstProjectiles; i++)
        {
            spawnRotation = Quaternion.Euler(baseSpawnRotation.eulerAngles.x + (_burstProjectileAngleSpacing * i), baseSpawnRotation.eulerAngles.y, baseSpawnRotation.eulerAngles.z);
            GameObject spawnedProjectile = Instantiate(_projectileTypeSo.prefab, _projectileSpawnPoint.position, spawnRotation);
            spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileTypeSo, _colour);
        }
        _projectileParticleSystem.Play();

        _fireTimer = _fireRate;
    }

    private Quaternion GetRotatationFromTargetMode(TargetingMode targetingMode)
    {
        Vector3 direction;
        Quaternion targetRotation;
        if (!_targetPlayer)
        {
            return Quaternion.Euler(transform.rotation.eulerAngles.x + RandomAngle(),
                                    transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
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

    public void TakeHit(int damage, Player from)
    {
        if (_hasShield)
        {
            _shieldLifePoints -= damage;
            if (_shieldLifePoints <= 0)
            {
                LoseShield();
            }
            else
            {
                _shieldHitFlash.HitFlashAnimation();
                EnemyManager.instance.PlayShieldHitClip();
            }
        }
        else
        {
            _lifePoints -= damage;
            _hitFlash.HitFlashAnimation();

            if (_lifePoints <= 0)
            {
                Die(from);
            }
            else
            {
                EnemyManager.instance.PlayHitClip();
            }
        }
    }

    private void LoseShield()
    {
        _shield.SetActive(false);
        _hasShield = false;
        EnemyManager.instance.PlayShieldBreakClip();
    }

    private void Die(Player killer)
    {
        killer.AddScore(_score);
        EnemyManager.instance.PlayDeathClip();
        Destroy(gameObject);
    }

    private void OnSplineCompleted()
    {
        Destroy(gameObject);
    }
}

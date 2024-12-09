using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region params

    private ColorTarget _colour;
    
    private float _speed;
    private int _score;
    private int _lifePoints;

    private bool _hasShield;
    private int _shieldLifePoints;

    private ProjectileType _projectileType;
    private float _fireRate;

    #endregion

    private GameObject _shield;
    private MeshRenderer _shieldRenderer;

    private bool _ready = false;

    private float _fireTimer;

    public void Initialize(EnemyType enemyType)
    {
        //Params
        _colour = enemyType.colour;
        
        _speed = enemyType.speed;
        _score = enemyType.score;
        _lifePoints = enemyType.lifePoints;
        
        //Shield params
        _shield = transform.GetChild(0).gameObject;
        _hasShield = enemyType.hasShield;
        _shield.SetActive(_hasShield);
        if (_hasShield)
        {
            _shieldRenderer = _shield.GetComponent<MeshRenderer>();
            _shieldRenderer.material = enemyType.shieldMaterial;

            _shieldLifePoints = enemyType.shieldLifePoints;
        }

        //Projectile params
        _projectileType = enemyType.projectileType;
        _fireRate = enemyType.fireRate;
        _fireTimer = _fireRate;

        _ready = true;
    }

    private void Update()
    {
        if (_ready)
        {
            Move();
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
            int damage = playerProjectile.GetDamage(_hasShield, _colour);
            if (damage != 0)
                TakeHit(damage);

            Destroy(other.gameObject);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    }

    private void Firing()
    {
        _fireTimer -= Time.deltaTime;
        if (_fireTimer <= 0)
            Fire();
    }

    private void Fire()
    {
        //Projetile Spawning
        GameObject spawnedProjectile = Instantiate(_projectileType.prefab, transform.position, transform.rotation);
        spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileType, _colour, transform.forward);

        _fireTimer = _fireRate;
    }

    private void TakeHit(int damage)
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
                Die();
        }
    }

    private void LoseShield()
    {
        _shield.SetActive(false);
        _hasShield = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

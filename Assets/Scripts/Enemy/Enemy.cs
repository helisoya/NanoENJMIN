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

    private bool _canFire;
    private ProjectileTypeSO _projectileTypeSo;
    private float _fireRate;

    #endregion

    private GameObject _shield;
    private MeshRenderer _shieldRenderer;

    private bool _ready = false;

    private float _fireTimer;

    public void Initialize(EnemyTypeSO enemyTypeSo)
    {
        //Params
        _colour = enemyTypeSo.colour;
        
        _speed = enemyTypeSo.speed;
        _score = enemyTypeSo.score;
        _lifePoints = enemyTypeSo.lifePoints;
        
        //Shield params
        _shield = transform.GetChild(0).gameObject;
        _hasShield = enemyTypeSo.hasShield;
        _shield.SetActive(_hasShield);
        if (_hasShield)
        {
            _shieldRenderer = _shield.GetComponent<MeshRenderer>();
            _shieldRenderer.material = enemyTypeSo.shieldMaterial;

            _shieldLifePoints = enemyTypeSo.shieldLifePoints;
        }

        //Projectile params
        _canFire = enemyTypeSo.canFire;
        _projectileTypeSo = enemyTypeSo.projectileTypeSo;
        _fireRate = enemyTypeSo.fireRate;
        _fireTimer = _fireRate;

        _ready = true;
    }

    private void Update()
    {
        if (_ready)
        {
            Move();
            if(_canFire)
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
        GameObject spawnedProjectile = Instantiate(_projectileTypeSo.prefab, transform.position, transform.rotation);
        spawnedProjectile.AddComponent<EnemyProjectile>().Initialize(_projectileTypeSo, _colour, transform.forward);

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

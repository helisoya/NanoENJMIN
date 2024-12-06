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


    private bool _ready = false;

    private float _fireTimer;

    public void Initialize(EnemyType enemyType)
    {
        _colour = enemyType.colour;

        _speed = enemyType.speed;
        _score = enemyType.score;
        _lifePoints = enemyType.lifePoints;

        _hasShield = enemyType.hasShield;
        _shieldLifePoints = enemyType.shieldLifePoints;

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

    private void Move()
    {
        Vector3 movement = transform.forward * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    }

    private void Firing()
    {
        _fireTimer -= Time.deltaTime;
        if(_fireTimer <= 0)
            Fire();
            
    }

    private void Fire()
    {
        //Projetile Spawning
        GameObject spawnedProjectile = Instantiate(_projectileType.prefab, transform.position, transform.rotation);
        spawnedProjectile.AddComponent<Projectile>().Initialize(_projectileType, _colour, transform.forward);

        _fireTimer = _fireRate;
    }
}

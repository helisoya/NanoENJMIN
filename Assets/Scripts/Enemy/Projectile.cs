using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region params

        private ColorTarget _colour;

        private float _speed;
        private int _inkRecharge;

        private Vector3 _direction;

    #endregion


    private bool _ready = false;
    
    public void Initialize(ProjectileType projectileType, ColorTarget colour, Vector3 direction)
    {
        _colour = colour;

        _speed = projectileType.speed;
        _inkRecharge = projectileType.inkRecharge;

        _direction = direction;

        _ready = true;
    }

    private void Update()
    {
        if (_ready)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 movement = _direction * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    }
}

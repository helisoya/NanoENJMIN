using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region params

        private ColorTarget _colour;

        private float _speed;
        private int _inkRecharge;

        private Vector3 _direction;

    #endregion

    private MeshRenderer _renderer;

    private bool _ready = false;
    
    public void Initialize(ProjectileType projectileType, ColorTarget colour, Vector3 direction)
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = projectileType.colourMaterials[(int)colour];
        
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bound"))
            Destroy(gameObject);
    }

    private void Move()
    {
        Vector3 movement = _direction * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);
    }

    public int GetInkToRecharge(ColorTarget playerColour)
    {
        if (playerColour != _colour)
            return 0;
        
        return _inkRecharge;
    }
}

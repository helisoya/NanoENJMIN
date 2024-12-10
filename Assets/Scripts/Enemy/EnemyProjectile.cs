using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region params

    private ColorTarget _colour;

    private float _speed;
    private float _inkRecharge;

    private Vector3 _direction;

    #endregion

    private MeshRenderer _renderer;

    private bool _ready = false;

    public void Initialize(ProjectileTypeSO projectileTypeSo, ColorTarget colour, Vector3 direction)
    {
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = projectileTypeSo.colourMaterials[(int)colour];

        _colour = colour;

        _speed = projectileTypeSo.speed;
        _inkRecharge = projectileTypeSo.inkRecharge;

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
        if (other.gameObject.CompareTag("Bound"))
            Destroy(gameObject);
    }

    private void Move()
    {
        Vector3 movement = _direction * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);

        if (transform.position.x <= -30) Destroy(gameObject);
    }

    public float GetInkToRecharge(ColorTarget playerColour)
    {
        if (playerColour != _colour)
            return 0f;

        return _inkRecharge;
    }
}

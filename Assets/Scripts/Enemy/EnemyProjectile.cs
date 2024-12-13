using System;
using System.Collections;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    #region params

    private ColorTarget _colour;

    private float _speed;
    private float _inkRecharge;

    #endregion

    private Transform _transform;
    private Collider _collider;
    private MeshRenderer _renderer;
    private bool _ready = false;

    private void Awake()
    {
        _transform = transform;
        _collider = GetComponent<Collider>();
    }

    public void Initialize(ProjectileTypeSO projectileTypeSo, ColorTarget colour)
    {
        _renderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
        _renderer.material = projectileTypeSo.colourMaterials[(int)colour];

        _colour = colour;

        _speed = projectileTypeSo.speed;
        _inkRecharge = projectileTypeSo.inkRecharge;

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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bound"))
        {
            Destroy(gameObject);
        } 
        else if (other.CompareTag("PlayerCollisionBox"))
        {
            other.GetComponent<PlayerCollisionBox>().TriggerEnter(gameObject);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * (_speed * Time.deltaTime);
        transform.Translate(movement, Space.World);

        if (transform.position.x <= -30) Destroy(gameObject);
    }

    public float GetInkToRecharge(ColorTarget playerColour)
    {
        if (playerColour != _colour)
            return 0f;

        return _inkRecharge;
    }

    public void DoAbsorption(Transform by)
    {
        _collider.enabled = false;
        _ready = false;
        _transform.parent = by;
        StartCoroutine(AbsorptionCoroutine());

        IEnumerator AbsorptionCoroutine()
        {
            float duration = 1f;
            float time = 0f;
            float startScale = _transform.localScale.x;
            while (time <= duration)
            {
                _transform.localScale = Vector3.one * Mathf.Lerp(startScale, 0, EaseInQuart(time / duration));
                time += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
        float EaseInQuart(float x) {
            return x * x * x * x;
        }
    }
}

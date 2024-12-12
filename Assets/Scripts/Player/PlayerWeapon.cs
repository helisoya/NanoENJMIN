using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Renderer[]            _renderers;
    private MaterialPropertyBlock _propertyBlock;

    private static readonly int EmissiveIntensity = Shader.PropertyToID("_EmissiveIntensity");

    private void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void SetEmissionIntensity(float emissionIntensity)
    {
        _propertyBlock.SetFloat(EmissiveIntensity, emissionIntensity);
        foreach (var r in _renderers)
        {
            r.SetPropertyBlock(_propertyBlock);
        }
    }
}
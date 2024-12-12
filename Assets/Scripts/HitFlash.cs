using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    [SerializeField] private float _hitFlashInDuration = .064f;
    [SerializeField] private float _hitFlashOutDuration = .064f;
    [SerializeField] private List<Renderer> _renderers;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    private static readonly int Opacity = Shader.PropertyToID("_FlashOpacity");

    private void Awake()
    {
        _materialPropertyBlock = new MaterialPropertyBlock();
    }


    public void HitFlashAnimation()
    {
        StartCoroutine(HitFlashAnimationCoroutine());
    }

    private IEnumerator HitFlashAnimationCoroutine()
    {
        yield return FlashCoroutine(0, 1, _hitFlashInDuration, EaseOutQuad);
        yield return FlashCoroutine(1, 0, _hitFlashOutDuration, EaseOutQuad);
        yield break;

        IEnumerator FlashCoroutine(float startValue, float endValue, float duration, Func<float, float> ease)
        {
            float time = 0;

            while (time < duration)
            {
                _materialPropertyBlock.SetFloat(Opacity, Mathf.Lerp(startValue, endValue, ease(time / duration)));
                foreach (var r in _renderers)
                {
                    r.SetPropertyBlock(_materialPropertyBlock);
                }
                time += Time.deltaTime;
                yield return null;
            }

            _materialPropertyBlock.SetFloat(Opacity, endValue);
            foreach (var r in _renderers)
            {
                r.SetPropertyBlock(_materialPropertyBlock);
            }
        }

        float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }
    }
}

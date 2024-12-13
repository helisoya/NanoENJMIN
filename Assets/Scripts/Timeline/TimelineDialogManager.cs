using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDialogManager : MonoBehaviour
{
    public static TimelineDialogManager instance;
 
    [SerializeField]
    private CanvasGroup _dialogTextCanvasGroup;
    
    [SerializeField]
    private TMP_Text _dialogText;

    [SerializeField]
    private Image _leftBackground;

    [SerializeField]
    private Image _rightBackground;

    [SerializeField]
    [Range(0, 2)]
    private float _slomoTimeScale = 0.3f;
    
    public bool InDialog { get; private set; }

    private RectTransform _leftBackgroundRect;
    private RectTransform _rightBackgroundRect;
    private float         _previousTimeScale;
    
    private void Awake()
    {
        instance = this;
        _leftBackgroundRect = _leftBackground.rectTransform;
        _rightBackgroundRect = _rightBackground.rectTransform;
        _leftBackgroundRect.anchoredPosition = new Vector2(-Screen.width, 0);
        _rightBackgroundRect.anchoredPosition = new Vector2(Screen.width, 0);
    }

    public void ShowDialog(string text)
    {
        InDialog = true;
        _dialogText.text = text;
        StartCoroutine(ShowDialogCoroutine(.5f));
        _previousTimeScale = Time.timeScale;
        StartCoroutine(TimeScaleCoroutine(_previousTimeScale, _slomoTimeScale, 1f));
    }

    public void HideDialog()
    {
        if (!InDialog)
        {
            return;
        }
        StartCoroutine(HideDialogCoroutine(.5f));
        StartCoroutine(TimeScaleCoroutine(_slomoTimeScale, _previousTimeScale, 1f, () => InDialog = false));
        
    }

    private IEnumerator ShowDialogCoroutine(float duration)
    {
        StartCoroutine(BackgroundImageSlideCoroutine(_leftBackgroundRect, new Vector2(-Screen.width * 0.5f, 0), new Vector2(0, 0), duration - .1f));
        StartCoroutine(BackgroundImageSlideCoroutine(_rightBackgroundRect, new Vector2(Screen.width * 0.5f, 0), new Vector2(0, 0), duration - .1f));
        yield return new WaitForSecondsRealtime(duration - .1f);
        yield return CanvasAlphaCoroutine(_dialogTextCanvasGroup, 0, 1, .1f);
    }

    private IEnumerator HideDialogCoroutine(float duration)
    {
        yield return CanvasAlphaCoroutine(_dialogTextCanvasGroup, 1, 0, .1f);
        StartCoroutine(BackgroundImageSlideCoroutine(_leftBackgroundRect, new Vector2(0, 0), new Vector2(-Screen.width * 0.5f, 0), duration - .1f));
        StartCoroutine(BackgroundImageSlideCoroutine(_rightBackgroundRect, new Vector2(0, 0), new Vector2(Screen.width * 0.5f, 0), duration - .1f));
        yield return new WaitForSecondsRealtime(duration - .1f);
    }

    private IEnumerator BackgroundImageSlideCoroutine(RectTransform rectTransform, Vector2 startPosition, Vector2 endPosition, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, EaseOutQuad(time / duration));
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = endPosition;
        float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }
    }

    private IEnumerator CanvasAlphaCoroutine(CanvasGroup canvasGroup, float alphaStart, float alphaEnd, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(alphaStart, alphaEnd, EaseOutQuad(time / duration));
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = alphaEnd;
        float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }
    }

    private IEnumerator TimeScaleCoroutine(float startValue, float endValue, float duration, Action callback = null)
    {
        float time = 0;
        while (time < duration)
        {
            Time.timeScale = Mathf.Lerp(startValue, endValue, EaseOutQuad(time / duration));
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        callback?.Invoke();
        
        float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }
    }
}

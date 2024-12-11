using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour, INotificationReceiver
{
    public static TimelineManager instance;

    [SerializeField]
    private CanvasGroup _dialogCanvasGroup;
    
    [SerializeField]
    private TMP_Text _dialogText;

    private PlayableDirector _playableDirector;
    
    private void Awake()
    {
        instance = this;
        _playableDirector = GetComponent<PlayableDirector>();
    }

    public void Play()
    {
        _playableDirector.Play();
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is TimelineLoopEvent loopEvent)
        {
            if (loopEvent.IsActive)
            {
                GoToTime(loopEvent.TimeToGoBackTo);
            }
        }
    }

    public void GoToTime(float timeToGoTo)
    {
        _playableDirector.time = timeToGoTo;
    }

    public void ShowDialog(string text)
    {
        _dialogText.text = text;
        StartCoroutine(AlphaCoroutine(0, 1, .3f));
    }

    public void HideDialog()
    {
        Debug.Log("HideDialog");
        StartCoroutine(AlphaCoroutine(1, 0, .6f));
    }

    private IEnumerator AlphaCoroutine(float alphaStart, float alphaEnd, float duration)
    {
        float time = 0;
        _dialogCanvasGroup.alpha = alphaStart;
        while (time < duration)
        {
            _dialogCanvasGroup.alpha = Mathf.Lerp(alphaStart, alphaEnd, EaseOutQuad(time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        _dialogCanvasGroup.alpha = alphaEnd;
        
        float EaseInQuad(float x) {
            return x * x;
        }
        float EaseInCubic(float x) {
            return x * x * x;
        }
        float EaseOutQuad(float x) {
            return 1 - (1 - x) * (1 - x);
        }
        float EaseOutCubic(float x) {
            return 1 - Mathf.Pow(1 - x, 3);
        }
    }
}

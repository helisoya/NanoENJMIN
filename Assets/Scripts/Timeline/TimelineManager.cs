using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour, INotificationReceiver
{
    public static TimelineManager instance;

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
}

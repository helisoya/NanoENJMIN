using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineLoopEvent : Marker, INotification, INotificationOptionProvider
{
    [SerializeField]
    private float _timeToGoBackTo;

    public PropertyName id => new PropertyName();
        
    public NotificationFlags flags => NotificationFlags.TriggerInEditMode | NotificationFlags.Retroactive;

    public float TimeToGoBackTo => _timeToGoBackTo;
}
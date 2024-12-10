using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineLoopEvent : Marker, INotification, INotificationOptionProvider
{
    [SerializeField]
    private float _timeToGoBackTo;

    [SerializeField]
    private bool _isActive;

    public PropertyName id => new PropertyName();
        
    public NotificationFlags flags => NotificationFlags.TriggerInEditMode | NotificationFlags.Retroactive;

    public float TimeToGoBackTo => _timeToGoBackTo;
    public bool  IsActive       => _isActive;
}
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineCallbackEvent : Marker, INotification, INotificationOptionProvider
{
    [SerializeField]
    private UnityEvent _callback;

    public PropertyName id => new PropertyName();
        
    public NotificationFlags flags => NotificationFlags.TriggerInEditMode | NotificationFlags.Retroactive;

    public UnityEvent Callback => _callback;
}
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FloatSignalReceiver : MonoBehaviour, INotificationReceiver
{
    public SignalAssetEventPair[] signalAssetEventPairs;

    [Serializable]
    public class SignalAssetEventPair
    {
        public SignalAsset       signalAsset;
        public UnityEvent<float> callback;
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is FloatSignalEmitter emitter)
        {
            if (!emitter.isActive)
            {
                return;
            }
            var matches = signalAssetEventPairs.Where(x => ReferenceEquals(x.signalAsset, emitter.asset));
            foreach (var m in matches)
            {
                m.callback.Invoke(emitter.parameter);
            }
        }
    }
}

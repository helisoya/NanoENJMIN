using UnityEngine.Timeline;

public class ParametrizedSignalEmitter<TParam> : SignalEmitter
{
    public bool isActive = true;
    public TParam parameter;
}

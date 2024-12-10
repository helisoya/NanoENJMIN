using UnityEngine.Timeline;

public class ParametrizedSignalEmitter<T> : SignalEmitter
{
    public bool isActive;
    public T parameter;
}

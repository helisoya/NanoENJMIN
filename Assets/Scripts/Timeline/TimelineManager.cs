using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
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
}

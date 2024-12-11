using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WavePlayableBehaviour : PlayableBehaviour
{
    public List<SpawnSetup> spawnSetups;

    private bool _isProcessed;
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (_isProcessed)
        {
            return;
        }
        EnemyManager.instance.SpawnWave(spawnSetups);
        _isProcessed = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (!_isProcessed)
        {
            return;
        }
        _isProcessed = false;
    }
}
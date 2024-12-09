using UnityEngine;
using UnityEngine.Playables;

public class WavePlayableBehaviour : PlayableBehaviour
{
    public WaveSO wave;

    private bool _isProcessed;
    
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (_isProcessed)
        {
            return;
        }
        // EnemyManager.instance.SpawnWave(wave);
        Debug.Log($"Spawning wave {wave.name}");
        _isProcessed = true;
    }
}

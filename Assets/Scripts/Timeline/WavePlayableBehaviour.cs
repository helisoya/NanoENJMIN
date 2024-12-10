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
        EnemyManager.instance.SpawnWave(wave);
        _isProcessed = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        _isProcessed = false;
    }
}

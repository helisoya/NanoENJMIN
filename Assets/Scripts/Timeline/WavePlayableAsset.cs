using UnityEngine;
using UnityEngine.Playables;

public class WavePlayableAsset : PlayableAsset
{
    public WaveSO wave;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<WavePlayableBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.wave = wave;

        return playable;
    }
}

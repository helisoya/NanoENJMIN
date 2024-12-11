using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class WavePlayableAsset : PlayableAsset
{
    public List<SpawnSetup> spawnSetups;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<WavePlayableBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.spawnSetups = spawnSetups;

        return playable;
    }
}

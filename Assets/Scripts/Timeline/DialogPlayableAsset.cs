using UnityEngine;
using UnityEngine.Playables;

public class DialogPlayableAsset : PlayableAsset
{
    [TextArea(2, 10)]
    public string dialogText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogPlayableBehaviour>.Create(graph);

        var behaviour = playable.GetBehaviour();
        behaviour.dialogText = dialogText;

        return playable;
    }
}

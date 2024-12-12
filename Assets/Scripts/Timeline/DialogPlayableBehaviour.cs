using UnityEngine;
using UnityEngine.Playables;

public class DialogPlayableBehaviour : PlayableBehaviour
{
    public string dialogText;

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

        _isProcessed = true;
        TimelineManager.instance.DialogManager.ShowDialog(dialogText);
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
        TimelineManager.instance.DialogManager.HideDialog();
    }
}

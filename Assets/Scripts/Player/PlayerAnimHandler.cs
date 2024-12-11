using UnityEngine;

public class PlayerAnimHandler : MonoBehaviour
{
    [HideInInspector] public Player _player;

    public void OnEndDeathAnim()
    {
        _player.OnEndDeathAnim();
    }
}

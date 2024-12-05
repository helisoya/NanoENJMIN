using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Represents a player
/// </summary>
public class Player : MonoBehaviour
{
    public int ID { get; private set; }
    public int GUIID { get; private set; }

    [Header("Infos")]


    [Header("Components")]
    // [SerializeField] private PlayerMovements movements;
    // [SerializeField] private PlayerAttack attack;
    // [SerializeField] private PlayerInterraction interraction;
    [SerializeField] private Animator animator;

    [Header("Collisions")]
    [SerializeField] private PlayerInput playerInput;
    private Gamepad pad;


    void Start()
    {
        pad = playerInput.GetDevice<Gamepad>();
        if (pad != null) pad.SetMotorSpeeds(0f, 0f);

        ID = GameManager.instance.RegisterPlayer(this);
        //GUIID = GameGUI.instance.AddNewPlayerGUI(ID);

        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Players/" + ID + "/Player");
    }

    /// <summary>
    /// Sets the animator's trigger
    /// </summary>
    /// <param name="triggerName">The trigger's name</param>
    public void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    void OnMove(InputValue input)
    {
        if (GameManager.instance.InGame)
        {
            //movements.SetVelocity(input.Get<Vector2>());
        }
    }

    void OnShoot(InputValue input)
    {
        if (GameManager.instance.InGame)
        {
            //attack.TryAttack();
        }
    }

    void OnDash(InputValue input)
    {
        if (GameManager.instance.InGame)
        {

        }
    }

    void OnPause(InputValue input)
    {
        if (input.isPressed)
        {
            if (!GameManager.instance.InGame)
            {
                GameManager.instance.ReadyUp(ID);
            }
            else
            {
                // UI Pause
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameManager.instance.InGame) return;

        // Collided with thing
    }
}

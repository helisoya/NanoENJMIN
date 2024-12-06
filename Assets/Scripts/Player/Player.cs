using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Represents a player
/// </summary>
public class Player : MonoBehaviour
{
    public int ID { get; private set; }
    public int GUIID { get; private set; }
    public bool Alive { get; private set; }
    public ColorTarget Color { get; private set; }

    [Header("Infos")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxMana;
    private int currentMana;
    private int currentHealth;

    [Header("Components")]
    [SerializeField] private PlayerMovements movements;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private Animator animator;

    [Header("Collisions")]
    [SerializeField] private PlayerInput playerInput;
    private Gamepad pad;


    void Start()
    {
        pad = playerInput.GetDevice<Gamepad>();
        if (pad != null) pad.SetMotorSpeeds(0f, 0f);

        ID = GameManager.instance.RegisterPlayer(this);
        gameObject.name = "Player-" + ID;
        //GUIID = GameGUI.instance.AddNewPlayerGUI(ID);

        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Players/" + ID + "/Player");

        currentMana = maxMana;
        currentHealth = maxHealth;
        Alive = true;
        Color = (ColorTarget)(ID + 1);
    }

    /// <summary>
    /// Sets the animator's trigger
    /// </summary>
    /// <param name="triggerName">The trigger's name</param>
    public void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }


    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="amount"></param>
    void OnTakeDamage(int amount)
    {
        if (!Alive) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            Alive = false;

            // Dead
        }
    }



    void OnMove(InputValue input)
    {
        if (Alive && GameManager.instance.InGame)
        {
            movements.SetVelocity(input.Get<Vector2>());
        }
    }

    void OnShoot(InputValue input)
    {
        if (Alive && GameManager.instance.InGame)
        {
            attack.SetCanAttack(input.isPressed);
        }
    }

    void OnDash(InputValue input)
    {
        if (Alive && input.isPressed && GameManager.instance.InGame)
        {
            movements.Dash();
        }
    }

    void OnPause(InputValue input)
    {
        print("Pause");
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
        if (Alive && !GameManager.instance.InGame) return;

        // Collided with thing
    }
}

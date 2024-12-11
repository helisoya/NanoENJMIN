using System;
using System.Linq;
using Unity.VisualScripting;
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
    public float Mana { get { return currentMana; } }
    public int Score { get; private set; }
    public ColorTarget Color { get; private set; }

    [Header("Infos")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float maxMana = 15;
    [SerializeField] private float manaFillSpeed = 0.2f;
    [SerializeField] private float invincibilityLength = 4f;
    private float currentMana;
    private int currentHealth;
    private float invincibilityStart;
    private bool isInvincible;


    [Header("Components")]
    [SerializeField] private PlayerMovements movements;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer playerRenderer;

    [Header("Collisions")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Audios")]
    [SerializeField] private AudioClip[] inkAbsortionClips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] deathClips;

    private Gamepad pad;


    void Start()
    {

        pad = playerInput.GetDevice<Gamepad>();
        if (pad != null) pad.SetMotorSpeeds(0f, 0f);

        ID = GameManager.instance.RegisterPlayer(this);
        gameObject.name = "Player-" + ID;
        GUIID = GameGUI.instance.AddNewPlayerGUI(ID);

        //animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Players/" + ID + "/Player");

        currentMana = maxMana;
        currentHealth = maxHealth;
        Alive = true;
        isInvincible = false;
        Color = (ColorTarget)(ID + 1);
        playerRenderer.material = GameManager.instance.GetPlayerMaterial(Color);
    }

    void Update()
    {
        if (currentMana < maxMana)
        {
            AddMana(manaFillSpeed * Time.deltaTime);
        }

        if (isInvincible && Time.time - invincibilityStart >= invincibilityLength)
        {
            isInvincible = false;
        }
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
    public void OnTakeDamage(int amount)
    {
        if (!Alive || isInvincible)
        {
            AudioManager.instance.PlaySFX2D(hitClips[UnityEngine.Random.Range(0, hitClips.Length)]);
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        GameGUI.instance.SetPlayerHealth(GUIID, currentHealth);

        if (currentHealth == 0)
        {
            AudioManager.instance.PlaySFX2D(deathClips[UnityEngine.Random.Range(0, deathClips.Length)]);
            Die();
        }
        else
        {
            AudioManager.instance.PlaySFX2D(hitClips[UnityEngine.Random.Range(0, hitClips.Length)]);
            invincibilityStart = Time.time;
            isInvincible = true;
        }
    }

    /// <summary>
    /// Adds score to the player
    /// </summary>
    /// <param name="amount">The score amount to add</param>
    public void AddScore(int amount)
    {
        Score += amount;
        GameManager.instance.ShowTotalScore();
    }


    /// <summary>
    /// Adds mana to the player
    /// </summary>
    /// <param name="amount">The amount of mana to add</param>
    public void AddMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        GameGUI.instance.SetPlayerManaFill(GUIID, currentMana / maxMana);
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
            //movements.Dash();
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
                GameGUI.instance.TogglePauseMenu();
            }
        }
    }

    /// <summary>
    /// Recharges the player
    /// </summary>
    /// <param name="inkRecharge">How many ink should be recharged</param>
    public void Recharge(float inkRecharge)
    {
        AudioManager.instance.PlaySFX2D(inkAbsortionClips[UnityEngine.Random.Range(0, inkAbsortionClips.Length)]);
        AddMana(inkRecharge);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyProjectile"))
        {
            EnemyProjectile enemyProjectile = other.gameObject.GetComponent<EnemyProjectile>();

            float inkRecharge = enemyProjectile.GetInkToRecharge(Color);
            if (inkRecharge == 0f)
            {
                OnTakeDamage(1);
            }
            else
            {
                AudioManager.instance.PlaySFX2D(inkAbsortionClips[UnityEngine.Random.Range(0, inkAbsortionClips.Length)]);
                AddMana(inkRecharge);
            }

            Destroy(other.gameObject);
        }
    }
    */

    private void Die()
    {
        Alive = false;
        transform.position = new Vector3(0, -9999, 0);
        GameManager.instance.KillPlayer(ID);
    }

    public Vector3 GetVelocity()
    {
        return movements.GetVelocity();
    }
}

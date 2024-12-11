using System;
using System.Collections;
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
    private bool takingDamage;

    private Quaternion _bodyModelStartRot;


    [Header("Components")]
    [SerializeField] private PlayerMovements movements;
    [SerializeField] private PlayerAttack attack;
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private GameObject _bodyModel;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerAnimHandler _playerAnimHandler;

    [Header("Collisions")]
    [SerializeField] private PlayerInput playerInput;

    [Header("Audios")]
    [SerializeField] private AudioClip[] inkAbsortionClips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] deathClips;

    [Header("Absortion")]
    [SerializeField] private GameObject absortionObj;
    [SerializeField] private float absortionVFXLength = 1f;
    private float absortionStart;
    private bool absortionVSFX = false;

    private Gamepad pad;


    private void Awake()
    {
        GameManager.instance.onGameStarted += OnGameStarted;
    }

    void Start()
    {
        _playerAnimHandler._player = this;

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
        absortionObj.GetComponent<Renderer>().material.SetColor("_Color", ID == 0 ? UnityEngine.Color.yellow : new Color(0.8f, 0, 0.8f));

        _bodyModelStartRot = _bodyModel.transform.rotation;
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

        if (absortionVSFX && Time.time - absortionStart >= absortionVFXLength)
        {
            absortionVSFX = false;
            absortionObj.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the animator's trigger
    /// </summary>
    /// <param name="triggerName">The trigger's name</param>
    public void SetAnimationTrigger(string triggerName)
    {
        _animator.SetTrigger(triggerName);
    }


    /// <summary>
    /// Takes damage
    /// </summary>
    /// <param name="amount"></param>
    public void OnTakeDamage(int amount)
    {
        if (takingDamage)
            return;

        if (!Alive || isInvincible)
        {
            AudioManager.instance.PlaySFX2D(hitClips[UnityEngine.Random.Range(0, hitClips.Length)]);
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        GameGUI.instance.SetPlayerHealth(GUIID, currentHealth);

        _animator.SetTrigger("Die");
        takingDamage = true;
        movements.SetVelocity(Vector2.zero);
        attack.SetCanAttack(false);

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
        if (!takingDamage)
        {
            currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
            GameGUI.instance.SetPlayerManaFill(GUIID, currentMana / maxMana);
        }
    }


    void OnMove(InputValue input)
    {
        if (Alive && GameManager.instance.InGame && !takingDamage)
        {
            movements.SetVelocity(input.Get<Vector2>());
            float angle = 90f - (45f * input.Get<Vector2>().y);
            _bodyModel.transform.rotation = Quaternion.Euler(_bodyModel.transform.rotation.eulerAngles.x, _bodyModel.transform.rotation.eulerAngles.y, angle);
        }
    }

    void OnShoot(InputValue input)
    {
        if (Alive && GameManager.instance.InGame && !takingDamage)
        {
            attack.SetCanAttack(input.isPressed);
        }
    }

    void OnDash(InputValue input)
    {
        if (Alive && input.isPressed && GameManager.instance.InGame && !takingDamage)
        {
            //movements.Dash();
        }
    }

    private void OnGameStarted()
    {
        _animator.SetBool("isMoving", true);
    }

    void OnPause(InputValue input)
    {
        if (input.isPressed)
        {
            if (!GameManager.instance.InGame)
            {
                GameManager.instance.ReadyUp(ID);
            }
            else if (!GameGUI.instance.InMenu)
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
        absortionVSFX = true;
        absortionStart = Time.time;
        absortionObj.SetActive(true);

        AudioManager.instance.PlaySFX2D(inkAbsortionClips[UnityEngine.Random.Range(0, inkAbsortionClips.Length)]);
        AddMana(inkRecharge);
    }

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

    public void OnEndDeathAnim()
    {
        if (Alive)
            GameManager.instance.RespawnPlayer(this);
    }

    public void ResetBodyRotation()
    {
        _bodyModel.transform.rotation = _bodyModelStartRot;
    }
    public void Respawned(Vector3 destinationPosition)
    {
        StartCoroutine(RespawnCoroutine(destinationPosition));
    }

    private IEnumerator RespawnCoroutine(Vector3 destination)
    {
        float distance = Vector3.Distance(transform.position, destination);
        while (distance > 0.5f)
        {
            Vector3 movement = transform.right * (Time.deltaTime * 10f);
            transform.Translate(movement);
            distance = Vector3.Distance(transform.position, destination);
            yield return new WaitForEndOfFrame();
        }

        takingDamage = false;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the player's movements
/// </summary>
public class PlayerMovements : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float baseSpeed = 15;
    [SerializeField] private float dashSpeed = 40;
    [SerializeField] private float dashLength = 0.25f;
    private float dashStart;
    private bool dashing = false;


    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    private Vector2 velocity;

    /// <summary>
    /// Sets the player's velocity
    /// </summary>
    /// <param name="velocity">The new velocity</param>
    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public Vector3 GetVelocity()
    {
        return rb.linearVelocity;
    }

    /// <summary>
    /// Starts a dash
    /// </summary>
    public void Dash()
    {
        dashStart = Time.time;
        dashing = true;
    }

    void Update()
    {
        // Stops the dash if needed
        if (dashing && Time.time - dashStart >= dashLength)
        {
            dashing = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 speedVelocity = velocity * (dashing ? dashSpeed : baseSpeed);
        rb.linearVelocity = new Vector3(speedVelocity.x, speedVelocity.y, 0);
    }
}

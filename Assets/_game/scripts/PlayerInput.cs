using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Forward/backward speed
    public float turnSpeed = 180f; // Degrees per second

    [Header("Dash Settings")]
    //public float maxChargeTime = 2f; // How long can you hold to fully charge
    //public float dashForceMax = 20f; // Max force applied when dash is released
    //public KeyCode chargeKey = KeyCode.LeftShift;
    //private bool knowsTheTech = false;

    [HideInInspector] public bool isMoving = false; // Animator will read this

    private Rigidbody2D rb;
    //private float chargeTimer = 0f;
    //private bool isCharging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerInput requires a Rigidbody!");
        }
    } 

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        //HandleCharge();
    }

    private void HandleMovement()
    {
        // Tank-style forward/back movement
        float moveInput = Input.GetAxis("Vertical"); // W/s or Up/Down arrows
        float turnInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows

        // Translate long forward
        Vector2 move = (Vector2)transform.up * moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate
        float rotation = -turnInput * turnSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rotation);
    }

    /*
    private void HandleCharge()
    {
        if (Input.GetKeyDown(chargeKey))
        {
            isCharging = true;
            chargeTimer = 0f;
        }
    }
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; 
    public float turnSpeed = 180f;

    [Header("Boost Settings")]
    public float maxChargeTime = 0.75f; 
    public float boostForce = 12f;      
    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool isBoosting = false;

    [Tooltip("Unlocks boost ability after the hint is shown")]
    public bool knowsTheTech = false;

    [Header("UI")]
    public Slider boostBar; // world-space UI above the player

    private Animator animator;
    private Rigidbody2D rb;

    [HideInInspector] public bool isMoving = false;

    // NEW: gate for whether player can provide input
    [HideInInspector] public bool canMove = true;

    private Vector2 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (boostBar != null)
        {
            boostBar.gameObject.SetActive(false);
            boostBar.maxValue = maxChargeTime;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleBoost();

        if (animator != null)
            animator.SetBool("isMoving", isMoving);
    }

    private void HandleMovement()
    {
        // Prevent physics spin from collisions
        if (rb != null)
            rb.angularVelocity = 0f;

        // If player controls are disabled and we're not currently in the tail of a boost, do nothing.
        if (!canMove && !isBoosting)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            isMoving = false;
            return;
        }

        // Stop normal movement while charging (charging only allowed when canMove)
        if (isCharging)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            isMoving = false;
            return;
        }

        // If currently boosting, do not apply MovePosition (so boost velocity isn't overwritten)
        if (isBoosting)
        {
            isMoving = true;
            return;
        }

        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Forward/backward
        Vector2 move = (Vector2)transform.up * moveInput * moveSpeed * Time.deltaTime;
        if (rb != null) rb.MovePosition(rb.position + move);

        // Tank turn
        if (Mathf.Abs(turnInput) > 0.01f && rb != null)
        {
            float rotation = -turnInput * turnSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation + rotation);
        }

        isMoving = Mathf.Abs(moveInput) > 0.01f;
    }

    private void HandleBoost()
    {
        // Don't do boost logic unless player has learned it AND controls are enabled
        if (!knowsTheTech || !canMove) return;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isCharging = true;
            chargeTime = 0f;
            if (boostBar != null) boostBar.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.LeftShift) && isCharging)
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Min(chargeTime, maxChargeTime);
            if (boostBar != null) boostBar.value = chargeTime;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && isCharging)
        {
            isCharging = false;
            isBoosting = true;

            float chargePercent = chargeTime / maxChargeTime;
            float force = boostForce * chargePercent;

            if (rb != null)
            {
                rb.velocity = transform.up * force; // instant launch
            }

            if (boostBar != null) boostBar.gameObject.SetActive(false);
            chargeTime = 0f;
            StartCoroutine(EndBoostAfterDelay(0.2f));
        }
    }

    private IEnumerator EndBoostAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isBoosting = false;
    }

    // --------------------
    // Public helpers
    // --------------------

    // Completely disable player control and cancel any charging/boost UI.
    public void DisableControl()
    {
        canMove = false;

        // cancel charge/boost state
        isCharging = false;
        isBoosting = false;
        chargeTime = 0f;

        // hide UI
        if (boostBar != null) boostBar.gameObject.SetActive(false);

        // zero physics
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // force animator idle
        if (animator != null) animator.SetBool("isMoving", false);
    }

    // Re-enable control (used on restart)
    public void EnableControl()
    {
        canMove = true;
        // keep other flags false; player starts fresh
        isCharging = false;
        isBoosting = false;
        chargeTime = 0f;
        if (boostBar != null) boostBar.gameObject.SetActive(false);
    }
}

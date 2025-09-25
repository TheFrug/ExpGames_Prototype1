using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Forward/backward speed
    public float turnSpeed = 180f; // Degrees per second

    [Header("Animation Settings")]
    private Animator animator;

    [HideInInspector] public bool isMoving = false; // Animator will read this

    private Rigidbody2D rb;

    private Vector2 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerInput requires a Rigidbody2D!");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerInput requires an Animator!");
        }
    } 

    void Update()
    {
        HandleMovement();

        // Pass movement state to animator
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // Move forward/backward
        Vector2 move = (Vector2)transform.up * moveInput * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate (tank-style turning)
        float rotation = -turnInput * turnSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation + rotation);

        // Tell animator if we’re actually moving
        isMoving = Mathf.Abs(moveInput) > 0.01f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RatAnimator : MonoBehaviour
{
    public Sprite[] sprites;            // mouse_0 ... mouse_11
    public float animationSpeed = 0.15f;

    private PlayerInput input;        // Reference to movement script
    private SpriteRenderer sr;

    private float animTimer = 0f;
    private int animFrame = 0;

    void Awake()
    {
        input = GetComponent<PlayerInput>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        // Work out current facing direction
        float angle = transform.eulerAngles.z;

        // Choose sprite set based on facing
        int baseIndex;
        if (angle >= 45f && angle < 135f)
            baseIndex = 0; // Up: mouse_0-2
        else if (angle >= 135f && angle < 225f)
            baseIndex = 9; // Left: mouse_9-11
        else if (angle >= 225f && angle < 315f)
            baseIndex = 6; // Down: mouse_6-8
        else
            baseIndex = 3; // Right: mouse_3-5

        // Walk cycle if moving, idle frame otherwise
        if (input.isMoving)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= animationSpeed)
            {
                animTimer = 0f;
                animFrame = (animFrame + 1) % 3;
            }
        }
        else
        {
            animFrame = 1; // idle frame (middle of the set)
        }

        int spriteIndex = baseIndex + animFrame;
        sr.sprite = sprites[spriteIndex];
    }
}

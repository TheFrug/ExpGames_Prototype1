using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ManualRat : MonoBehaviour
{
    public float moveSpeed = 6f; // match DummyRat for timing
    private Rigidbody2D rb;
    private Vector2 input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // WASD / Arrow key input (4-dir, no diagonals if you want strict)
        float x = Input.GetAxisRaw("Horizontal"); // -1, 0, 1
        float y = Input.GetAxisRaw("Vertical");   // -1, 0, 1

        // Optional: block diagonal input by zeroing one axis if both pressed
        if (Mathf.Abs(x) > 0 && Mathf.Abs(y) > 0)
        {
            y = 0; // or x = 0, depending on preference
        }

        input = new Vector2(x, y).normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
    }
}

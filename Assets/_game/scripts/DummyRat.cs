using System.Collections;
using UnityEngine;

public class DummyRat : MonoBehaviour
{
    public float moveSpeed = 6f;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    private Coroutine runRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    // Starts from the startPosition and runs the course
    public void StartRunning()
    {
        StopRunning(); // ensure clean state

        transform.position = startPosition;
        rb.velocity = Vector2.zero;
        runRoutine = StartCoroutine(RunCourse());
        Debug.Log("[DummyRat] StartRunning()");
    }

    // Stops (if needed) then restarts the routine
    public void ResetAndRestart()
    {
        StopRunning();

        transform.position = startPosition;
        rb.velocity = Vector2.zero;
        runRoutine = StartCoroutine(RunCourse());
        Debug.Log("[DummyRat] ResetAndRestart()");
    }

    // Safely stop any running coroutine and zero velocity
    public void StopRunning()
    {
        if (runRoutine != null)
        {
            StopCoroutine(runRoutine);
            runRoutine = null;
        }
        if (rb != null) rb.velocity = Vector2.zero;
    }

    private IEnumerator RunCourse()
    {
        // Example sequence (your values)
        yield return Move(Vector2.right, 4.2f);
        yield return Move(Vector2.up, 1.7f);
        yield return Move(Vector2.left, 4f);
        yield return Move(Vector2.up, 1.5f);
        yield return Move(Vector2.right, 2.5f);
        yield return Move(Vector2.up, 2f);
        yield return Move(Vector2.left, 3f);
        yield return Move(Vector2.up, 1f);

        rb.velocity = Vector2.zero; // stop at finish
        runRoutine = null;
        Debug.Log("[DummyRat] RunCourse finished");
    }

    private IEnumerator Move(Vector2 direction, float duration)
    {
        // Rotate the rat to face the movement direction
        if (direction != Vector2.zero)
        {
            transform.up = direction.normalized;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            rb.velocity = direction.normalized * moveSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }

}

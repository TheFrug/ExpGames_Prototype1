using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public Vector2 startPosition;
    public Quaternion startRotation;

    public Timer timer;
    public GameObject finishUI;
    public bool finished = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startPosition = player.transform.position;
        startRotation = player.transform.rotation;
    }

    public void RestartMaze()
    {
        StartCoroutine(RestartAfterSlide());
    }

    private IEnumerator RestartAfterSlide()
    {
        // Wait for slide-out to finish
        yield return new WaitForSeconds(0.5f);

        // Reset player
        player.transform.position = startPosition;
        player.transform.rotation = startRotation;

        // Reset timer
        timer.ResetTimer();
    }

}

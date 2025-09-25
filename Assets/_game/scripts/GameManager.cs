using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject player;
    public Vector2 startPosition;
    public Quaternion startRotation;

    public Timer timer;
    public GameObject finishUI;
    public bool finished = false;

    [Header("Narrative Elements")]
    public GameObject whiteboard;         // assign in Inspector
    public GameObject tutorialText;       // child of whiteboard
    public GameObject preStage2Text;      // child of whiteboard
    public GameObject stickyNote;         // child of FinishUI

    [Header("Race Elements")]
    public DummyRat dummyRat;   // assign in Inspector
    public FinishLine finishLine;  // assign in Inspector
    private int stage = 1; // start at stage 1

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        startPosition = player.transform.position;
        startRotation = player.transform.rotation;

        // Ensure initial states
        tutorialText.SetActive(true);
        preStage2Text.SetActive(false);
        stickyNote.SetActive(false);
    }

        void Update()
    {
        // Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit requested");
            Application.Quit();

            // If in editor, stop play mode
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        // Restart maze
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void RestartMaze()
    {
        if (stage == 1)
            StartCoroutine(Stage2Routine());
        else if (stage == 2)
            StartCoroutine(Stage3Routine());
        else
            StartCoroutine(TrueRestartRoutine());
    }

    private IEnumerator Stage2Routine()
    {
        yield return new WaitForSeconds(0.5f);

        // Swap tutorial → stage 2 text
        tutorialText.SetActive(false);
        preStage2Text.SetActive(true);

        // Keep it visible for a moment
        yield return new WaitForSeconds(3f);

        // Slide the whiteboard off screen
        yield return StartCoroutine(SlideWhiteboardOff());

        // Start dummy rat
        if (dummyRat != null)
        {
            dummyRat.StartRunning();
        }

        // Add sticky note hint (player learns tech)
        stickyNote.SetActive(true);
        player.GetComponent<PlayerInput>().knowsTheTech = true;

        // Reset player/timer
        ResetPlayerAndTimer();

        stage = 2;
    }

    private IEnumerator Stage3Routine()
    {
        yield return new WaitForSeconds(0.5f);

        // Optionally restart dummy rat so it runs again during stage 3
        if (dummyRat != null)
        {
            dummyRat.ResetAndRestart();
        }

        ResetPlayerAndTimer();

        stage = 3;
    }

    private IEnumerator TrueRestartRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        ResetPlayerAndTimer();

        // Make sure dummy rat restarts too (clean)
        if (dummyRat != null)
        {
            dummyRat.ResetAndRestart();
        }
    }

    private void ResetPlayerAndTimer()
    {
        // Reset player transform & rotation
        player.transform.position = startPosition;
        player.transform.rotation = startRotation;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        var pi = player.GetComponent<PlayerInput>();
        if (pi != null)
        {
            pi.EnableControl();
        }

        if (timer != null) timer.ResetTimer();

        finished = false;

        // ✅ Reset finish line so it can trigger again
        if (finishLine != null)
        {
            finishLine.ResetFinishLine();
        }
    }



    private IEnumerator SlideWhiteboardOff()
    {
        Vector3 startPos = whiteboard.transform.position;
        Vector3 endPos = startPos + new Vector3(900f, 0f, 0f); // adjust distance
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            whiteboard.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        whiteboard.transform.position = endPos;
    }
}

using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public GameObject finishUI;       // assign the panel with UISlideIn + ResultsUI
    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (finished) return;

        if (collision.CompareTag("Player"))
        {
            finished = true;                      // <- prevent re-trigger
            GameManager.Instance.finished = true;

            // Stop the timer and dummy rat
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.StopTimer();
            }

            if (GameManager.Instance != null && GameManager.Instance.dummyRat != null)
            {
                GameManager.Instance.dummyRat.StopRunning();
            }

            // Disable player input immediately so nothing else can happen
            PlayerInput pInput = collision.GetComponent<PlayerInput>();
            if (pInput != null)
            {
                pInput.DisableControl();
            }

            // Show results (populate text)
            float elapsed = (timer != null) ? timer.ElapsedTime : 0f;
            if (finishUI != null)
            {
                finishUI.SetActive(true);

                ResultsUI results = finishUI.GetComponent<ResultsUI>();
                if (results != null)
                {
                    results.ShowResults(elapsed);
                }

                UISlideIn slider = finishUI.GetComponent<UISlideIn>();
                if (slider != null)
                {
                    slider.SlideIn();
                }
            }
        }
    }

    public void ResetFinishLine()
    {
        finished = false;
    }

}

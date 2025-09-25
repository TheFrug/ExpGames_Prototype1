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
            GameManager.Instance.finished = true;

            // 1. Stop the timer
            Timer timer = FindObjectOfType<Timer>();
            if (timer != null)
            {
                timer.StopTimer();
            }

            // 2. Show results (populate text)
            if (finishUI != null)
            {
                finishUI.SetActive(true);

                // Results text building
                ResultsUI results = finishUI.GetComponent<ResultsUI>();
                if (results != null)
                {
                    results.ShowResults(timer.ElapsedTime);
                }

                // Slide panel onto screen
                UISlideIn slider = finishUI.GetComponent<UISlideIn>();
                if (slider != null)
                {
                    slider.SlideIn();
                }
            }
        }
    }
}

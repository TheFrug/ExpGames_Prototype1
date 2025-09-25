using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TMP_Text timerText;   // drag in a TextMeshProUGUI from the canvas
    public bool isRunning = true;

    public float ElapsedTime { get; private set; } = 0f;

    void Update()
    {
        if (!isRunning) return;

        ElapsedTime += Time.deltaTime;

        if (timerText != null)
        {
            // format: mm:ss.ff (e.g. 01:23.45)
            int minutes = Mathf.FloorToInt(ElapsedTime / 60f);
            float seconds = ElapsedTime % 60f;
            timerText.text = $"{minutes:00}:{seconds:00.00}";
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        ElapsedTime = 0f;
        isRunning = true;
    }
}

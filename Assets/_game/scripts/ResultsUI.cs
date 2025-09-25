using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class ResultsUI : MonoBehaviour
{
    public TextMeshProUGUI[] resultLines; // drag 4 Text objects in order
    public Timer timer;

    public void ShowResults(float playerTime)
    {
        List<(string name, float time, bool isPlayer)> scores = new List<(string, float, bool)> {
            ("Subject 1", 16.23f, false),
            ("Subject 2", 28.45f, false),
            ("Subject 4", 41.10f, false),
            ("Subject 3 (You)", playerTime, true)
        };

        // Sort ascending by time
        var sorted = scores.OrderBy(s => s.time).ToList();

        // Print to UI
        for (int i = 0; i < sorted.Count; i++)
        {
            resultLines[i].text = $"{sorted[i].name} - {FormatTime(sorted[i].time)}s";

            // reset to neutral
            resultLines[i].color = Color.black;

            // highlight only the player’s entry
            if (sorted[i].isPlayer)
            {
                resultLines[i].color = Color.red;
            }
        }
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int millis = Mathf.FloorToInt((t * 100f) % 100f);
        return $"{minutes:00}:{seconds:00}:{millis:00}";
    }
}

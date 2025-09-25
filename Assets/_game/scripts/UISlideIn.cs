using UnityEngine;
using System.Collections;

public class UISlideIn : MonoBehaviour
{
    public RectTransform panel;
    public float slideTime = 0.5f;
    public Vector2 offscreenPosition = new Vector2(0, -800); // below screen
    private Vector2 onScreenPosition;

    void Awake()
    {
        onScreenPosition = panel.anchoredPosition;
        panel.anchoredPosition = offscreenPosition; // start hidden
    }

    public void SlideIn()
    {
        StartCoroutine(Slide(panel, offscreenPosition, onScreenPosition));
    }

    public void SlideOut()
    {
        StartCoroutine(Slide(panel, onScreenPosition, offscreenPosition));
        GameManager.Instance.finished = false;
    }

    private IEnumerator Slide(RectTransform target, Vector2 from, Vector2 to)
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / slideTime;
            target.anchoredPosition = Vector2.Lerp(from, to, t);
            yield return null;
        }
    }

    public void HideResults()
    {
        gameObject.SetActive(false);
    }

}

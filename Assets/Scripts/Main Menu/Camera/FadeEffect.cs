using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    private Image fadeImg;
    private void Awake()
    {
        fadeImg = GetComponent<Image>();
    }

    public IEnumerator FadeInCoroutine(float duration)
    {
        Color startColor = new(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1);
        Color targetColor = new(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0);
        yield return FadeCoroutine(startColor, targetColor, duration);
        gameObject.SetActive(false);
    }

    public IEnumerator FadeOutCoroutine(float duration)
    {
        Color startColor = new(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 0);
        Color targetColor = new(fadeImg.color.r, fadeImg.color.g, fadeImg.color.b, 1);
        gameObject.SetActive(true);
        yield return FadeCoroutine(startColor, targetColor, duration);
    }

    private IEnumerator FadeCoroutine(Color startColor, Color fadeColor, float duration)
    {
        float elapsedTime = 0;
        float elapsedPercentage = 0;

        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            fadeImg.color = Color.Lerp(startColor, fadeColor, elapsedPercentage);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

    }
}

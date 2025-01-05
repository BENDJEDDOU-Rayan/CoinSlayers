using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HoverEffectButton : MonoBehaviour
{

    private TMPro.TMP_Text textMeshPro;
    private Coroutine hoverEffectCoroutine;
    void Start()
    {
        textMeshPro = GetComponent<TMPro.TMP_Text>();
    }

    public void StartHoverEffect()
    {
        if(hoverEffectCoroutine != null)
        {
            StopCoroutine(hoverEffectCoroutine);
        }
        hoverEffectCoroutine = StartCoroutine(TransitionEffect(1.2f, Color.yellow));
    }

    public void ResetHoverEffect()
    {
        if (hoverEffectCoroutine != null)
        {
            StopCoroutine(hoverEffectCoroutine);
        }

        // Lancer la transition vers l'�tat "normal"
        hoverEffectCoroutine = StartCoroutine(TransitionEffect(1.0f, new Color(255f, 255f, 130f)));
    }

    private IEnumerator TransitionEffect(float targetFontSizeFactor, Color newColor)
    {
        float elapsedTime = 0f;
        float startFontSize = textMeshPro.fontSize;
        Color startColor = textMeshPro.color;

        // Transition progressive sur la dur�e sp�cifi�e
        while (elapsedTime < 0.1f)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / 0.1f);

            // Interpolation lin�aire pour la taille et la couleur
            textMeshPro.fontSize = Mathf.Lerp(startFontSize, 60 * targetFontSizeFactor, progress);
            textMeshPro.color = Color.Lerp(startColor, newColor, progress);

            yield return null; // Attend la frame suivante
        }

        // Assure que la transition finit exactement � la cible
        textMeshPro.fontSize = 60 * targetFontSizeFactor;
        textMeshPro.color = newColor;
    }
}

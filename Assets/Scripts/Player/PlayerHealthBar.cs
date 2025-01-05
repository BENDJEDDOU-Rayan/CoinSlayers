using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private Image healthBar;
    private PlayerHealth playerHealth;

    private float currentFillAmount;
    private Coroutine healthChangeCoroutine;
    [SerializeField] private float transitionDuration = 0.3f;

    void Start()
    {
        healthBar = GetComponent<Image>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            currentFillAmount = playerHealth.GetCurrentHealth() / 100f;
            healthBar.fillAmount = currentFillAmount;
        }
    }

    void Update()
    {
        if (playerHealth != null)
        {
            float targetFillAmount = playerHealth.GetCurrentHealth() / 100f;

            if (healthChangeCoroutine == null || healthBar.fillAmount != targetFillAmount)
            {
                StartSmoothHealthChange(targetFillAmount);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                playerHealth.TakeDamage(20);
            }
        }
    }

    private void StartSmoothHealthChange(float targetFillAmount)
    {
        // Si une transition est déjà en cours, l'arrêter
        if (healthChangeCoroutine != null)
        {
            StopCoroutine(healthChangeCoroutine);
        }

        // Lancer une nouvelle transition
        healthChangeCoroutine = StartCoroutine(SmoothHealthChange(targetFillAmount));
    }

    private IEnumerator SmoothHealthChange(float targetFillAmount)
    {
        float startFillAmount = healthBar.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / transitionDuration);

            healthBar.fillAmount = Mathf.Lerp(startFillAmount, targetFillAmount, progress);

            yield return null;
        }

        healthBar.fillAmount = targetFillAmount;
        healthChangeCoroutine = null;
    }
}

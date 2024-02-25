using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Image healthBarFill; // Reference to the fill image of the health bar
    public GameObject gameOverUI; // Reference to the game over UI object
    public float healthDepletionRate = 0.1f; // Rate at which health depletes
    public float gameOverThreshold = 0f; // Threshold at which the game is considered over
    public float fadeDuration = 0.5f; // Duration of fade in and fade out

    private float currentHealth; // Current health value
    private float timeSinceLastDepletion; // Time since health last depleted
    private CanvasGroup healthBarCanvasGroup; // Canvas group for fading health bar

    private void Start()
    {
        currentHealth = 1f;
        UpdateHealthBar();
        healthBarCanvasGroup = healthBarFill.GetComponentInParent<CanvasGroup>();
        healthBarCanvasGroup.alpha = 0f; // Start with health bar faded out
    }

    private void Update()
    {
        Debug.Log("Current Health: " + currentHealth);
        // Fade in/out health bar based on time since last depletion
        FadeHealthBar();
    }

    public void ReduceHealth(float amount)
    {
        // Reduce health when an arrow is missed or target position is pressed
        currentHealth -= amount;
        UpdateHealthBar();
        timeSinceLastDepletion = 0f;

        // Check if health is depleted
        if (currentHealth <= gameOverThreshold)
        {
            // Game over
            Time.timeScale = 0; // Pause the game
            gameOverUI.SetActive(true); // Show the game over UI
        }
    }

    private void UpdateHealthBar()
    {
        // Update the fill amount of the health bar based on the current health value
        healthBarFill.fillAmount = currentHealth;
    }

    private void FadeHealthBar()
    {
        if (currentHealth > gameOverThreshold && timeSinceLastDepletion >= 0.5f && healthBarCanvasGroup.alpha > 0f)
        {
            // Fade out health bar
            StartCoroutine(FadeCanvasGroup(healthBarCanvasGroup, healthBarCanvasGroup.alpha, 0f, fadeDuration));
        }
        else if (healthBarCanvasGroup.alpha < 1f)
        {
            // Fade in health bar
            StartCoroutine(FadeCanvasGroup(healthBarCanvasGroup, healthBarCanvasGroup.alpha, 1f, fadeDuration));
        }
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}

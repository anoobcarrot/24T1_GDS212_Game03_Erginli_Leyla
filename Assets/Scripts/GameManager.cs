using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image healthBarFill; // Reference to the fill image of the health bar
    public GameObject gameOverUI; // Reference to the game over UI object
    public GameObject submitScoreQuestionPanel;
    public float healthDepletionRate = 0.1f; // Rate at which health depletes
    public float gameOverThreshold = 0f; // Threshold at which the game is considered over
    public float fadeDuration = 0.5f; // Duration of fade in and fade out
    public float fadeOutDelay = 1.0f; // Added delay before fading out
    public string sceneName; // Name of the scene to reload

    private float timeSinceDepletion; // Time since health started depleting
    private float currentHealth; // Current health value
    private CanvasGroup healthBarCanvasGroup; // Canvas group for fading health bar
    private bool isHealthDepleting; // Flag to track if health is currently depleting

    private void Start()
    {
        Debug.Log("Game Manager is starting");
        Time.timeScale = 1;
        currentHealth = 1f;
        UpdateHealthBar();
        healthBarCanvasGroup = healthBarFill.GetComponentInParent<CanvasGroup>();
        healthBarCanvasGroup.alpha = 0f; // Start with health bar faded out
    }

    private void Update()
    {
        // Fade in/out health bar based on health depletion status
        FadeHealthBar();
    }

    public void ReduceHealth(float amount)
    {
        // Set isHealthDepleting to true before reducing health
        isHealthDepleting = true;
        FadeHealthBar();

        // Reduce health when an arrow is missed or target position is pressed
        currentHealth -= amount;
        UpdateHealthBar();

        // Check if health is depleted
        if (currentHealth <= gameOverThreshold)
        {
            // Game over
            Time.timeScale = 0; // Pause the game
            submitScoreQuestionPanel.SetActive(true); // Show the game over UI
        }

        // Reset timer
        timeSinceDepletion = 0f;

        // Delay setting isHealthDepleting to false
        StartCoroutine(DelayResetIsHealthDepleting());
    }

    private IEnumerator DelayResetIsHealthDepleting()
    {
        yield return new WaitForSeconds(1f); // Change delayTime to adjust the duration
        isHealthDepleting = false;
    }

    public void EnableGameOverPanel()
    {
        submitScoreQuestionPanel.SetActive(false);
        // Enable the submit score panel GameObject
        gameOverUI.SetActive(true);
    }

    private void UpdateHealthBar()
    {
        // Update the fill amount of the health bar based on the current health value
        healthBarFill.fillAmount = currentHealth;
    }

    public void RestartScene()
    {
        // Reload the specified scene
        SceneManager.LoadScene(sceneName);
    }

    private void FadeHealthBar()
    {
        if (isHealthDepleting)
        {
            // Health is depleting and health bar is faded in, fade out health bar
            StartCoroutine(FadeCanvasGroup(healthBarCanvasGroup, healthBarCanvasGroup.alpha, 1f, fadeDuration));
        }
        else if (timeSinceDepletion >= fadeOutDelay)
        {
            // Health is not depleting and fade out delay has passed, fade out health bar
            StartCoroutine(FadeCanvasGroup(healthBarCanvasGroup, healthBarCanvasGroup.alpha, 0f, fadeDuration));
        }
        else
        {
            // Health is not depleting yet, increase timer
            timeSinceDepletion += Time.deltaTime;
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


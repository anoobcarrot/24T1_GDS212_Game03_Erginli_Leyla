using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private PlayerInput playerInput;

    private AudioAnalyser audioAnalyser;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        audioAnalyser = FindObjectOfType<AudioAnalyser>();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Pause time
        isPaused = true;
        playerInput.SetCanInteract(false);
        audioAnalyser.PauseAudio();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Unpause time
        isPaused = false;
        playerInput.SetCanInteract(true);
        audioAnalyser.ResumeAudio();
    }
}

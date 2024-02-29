using UnityEngine;
using System.Collections;

public class AudioAnalyser : MonoBehaviour
{
    public AudioClip musicClip; // audio clip containing the music
    public AudioClip completeAudioClip;
    public int sampleSize = 1024; // Number of samples to analyse per frame
    public float beatThreshold = 0.5f; // Threshold for detecting a beat
    public float beatCooldown = 0.2f; // Cooldown between beat detections
    public GameObject[] fallingArrowPrefabs; // Array of falling arrow prefabs
    public Transform[] spawnPoints; // Array of spawn points for falling arrows
    public GameObject submitScoreQuestionPanel;

    private AudioSource audioSource;
    private float[] spectrumData; // Array to store spectrum data
    private float beatTimer = 0f;
    private float lastBeatTime = 0f;
    private bool isAudioPlaying = false;

    private PlayerInput playerInput;
    private GameManager gameManager;

    public float fallSpeed = 5f;

    // Define frequency range for analysis
    public float minFrequency = 100f; // Start frequency of the range
    public float maxFrequency = 5000f; // End frequency of the range

    void Start()
    {
        // Find the PlayerInput component in the scene
        playerInput = FindObjectOfType<PlayerInput>();
        gameManager = FindObjectOfType<GameManager>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found in the scene.");
        }
        Debug.Log("Audio Analyser is starting");
        // Initialise the AudioSource component and spectrumData array
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        spectrumData = new float[sampleSize];

        // Start playing the music after a delay of 5 seconds
        StartCoroutine(DelayedAudioStart(5f));
    }

    IEnumerator DelayedAudioStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Start playing the music
        audioSource.Play();
        isAudioPlaying = true;
    }

    void Update()
    {
        // Get spectrum data from the audio source
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        float currentHealth = gameManager.GetCurrentHealth();

        // Check if a beat is detected
        if (IsBeatDetected())
        {
            // Log the beat
            Debug.Log("Beat detected at time: " + Time.time);

            // Reset beat timer
            beatTimer = beatCooldown;
            lastBeatTime = Time.time;

            // Instantiate falling arrows at spawn points
            InstantiateFallingArrows();
        }

        // Update beat timer
        if (beatTimer > 0)
        {
            beatTimer -= Time.deltaTime;
        }

        if (isAudioPlaying && !audioSource.isPlaying && currentHealth > 0)
        {
            // The audio has stopped playing, show the submit score panel
            playerInput.SetCanInteract(false);
            ShowSubmitScorePanel();
        }
    }

    public void ShowSubmitScorePanel()
    {
        // Play the complete audio clip when the submit score panel appears
        if (completeAudioClip != null)
        {
            audioSource.clip = completeAudioClip;
            audioSource.PlayOneShot(completeAudioClip);
            isAudioPlaying = false;
        }
        // Show the submit score panel
        submitScoreQuestionPanel.SetActive(true);
    }

    public bool IsBeatDetected()
    {
        // Analyze spectrum data for the specified frequency range
        float averageAmplitude = CalculateAverageAmplitude(minFrequency, maxFrequency);

        // Check if beat is detected based on threshold
        return averageAmplitude > beatThreshold && beatTimer <= 0;
    }

    float CalculateAverageAmplitude(float startFrequency, float endFrequency)
    {
        // Calculate bin indices for the specified frequency range
        int startIndex = Mathf.FloorToInt(startFrequency / (AudioSettings.outputSampleRate / 2) * sampleSize);
        int endIndex = Mathf.CeilToInt(endFrequency / (AudioSettings.outputSampleRate / 2) * sampleSize);

        // Clamp indices to valid range
        startIndex = Mathf.Clamp(startIndex, 0, sampleSize - 1);
        endIndex = Mathf.Clamp(endIndex, 0, sampleSize - 1);

        float averageAmplitude = 0f;
        for (int i = startIndex; i < endIndex; i++)
        {
            averageAmplitude += spectrumData[i];
        }
        averageAmplitude /= (endIndex - startIndex + 1); // Add 1 to include the endIndex
        return averageAmplitude;
    }

    void InstantiateFallingArrows()
    {
        // Randomly select a spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Use the same index to select the corresponding arrow prefab
        GameObject selectedPrefab = fallingArrowPrefabs[spawnIndex % fallingArrowPrefabs.Length];

        // Spawn the arrow at the selected spawn point
        Instantiate(selectedPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    public void StopAudioSource()
    {
        // Check if the AudioSource component is found
        if (audioSource != null)
        {
            // Stop the audio source from playing
            audioSource.Stop();
        }
    }
}

















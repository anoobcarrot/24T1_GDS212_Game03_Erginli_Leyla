using UnityEngine;
using System.Collections;

public class AudioAnalyser : MonoBehaviour
{
    public AudioClip musicClip; // The audio clip containing the music
    public int sampleSize = 128; // Number of samples to analyze per frame
    public float beatThreshold = 0.5f; // Threshold for detecting a beat
    public float beatCooldown = 0.2f; // Cooldown between beat detections
    public GameObject[] fallingArrowPrefabs; // Array of falling arrow prefabs
    public Transform[] spawnPoints; // Array of spawn points for falling arrows

    // Define frequency range for high band
    public float startFrequency = 1000f; // Start frequency of the range
    public float endFrequency = 5000f; // End frequency of the range

    private AudioSource audioSource;
    private float[] spectrumData; // Array to store spectrum data
    private float beatTimer = 0f;
    private float lastBeatTime = 0f;
    private int beatCount = 0;

    public float fallSpeed = 5f;

    void Start()
    {
        // Initialise the AudioSource component and spectrumData array
        audioSource = gameObject.AddComponent<AudioSource>();
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
    }

    void Update()
    {
        // Get spectrum data from the audio source
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Check if a beat is detected
        if (IsBeatDetected())
        {
            // Log the beat
            Debug.Log("Beat detected at time: " + Time.time);

            // Reset beat timer
            beatTimer = beatCooldown;
            lastBeatTime = Time.time;
            beatCount++;

            // Instantiate falling arrows at spawn points
            InstantiateFallingArrows();
        }

        // Update beat timer
        if (beatTimer > 0)
        {
            beatTimer -= Time.deltaTime;
        }
    }

    public bool IsBeatDetected()
    {
        // Analyze spectrum data for the high frequency band
        float highBandAmplitude = CalculateAverageAmplitude(startFrequency, endFrequency);

        // Check if beat is detected in the high frequency band based on threshold
        return highBandAmplitude > beatThreshold && beatTimer <= 0;
    }

    float CalculateAverageAmplitude(float startFrequency, float endFrequency)
    {
        int startIndex = (int)(startFrequency / (AudioSettings.outputSampleRate / 2) * sampleSize);
        int endIndex = (int)(endFrequency / (AudioSettings.outputSampleRate / 2) * sampleSize);

        float averageAmplitude = 0f;
        for (int i = startIndex; i < endIndex; i++)
        {
            averageAmplitude += spectrumData[i];
        }
        averageAmplitude /= (endIndex - startIndex);
        return averageAmplitude;
    }

    void InstantiateFallingArrows()
    {
        // Calculate time until next beat
        float timeUntilNextBeat = lastBeatTime + beatCooldown - Time.time;

        // Adjust spawn timing based on fall speed
        float fallTime = fallSpeed;
        float spawnTime = timeUntilNextBeat - 5f; // Reduce spawn time by 5 seconds

        // Randomly select a spawn point index
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Use the same index to select the corresponding arrow prefab
        GameObject selectedPrefab = fallingArrowPrefabs[spawnIndex % fallingArrowPrefabs.Length];

        // Check if the spawn time is less than or equal to 0
        if (spawnTime <= 0)
        {
            // If spawn time is non-positive, spawn the arrow immediately
            Instantiate(selectedPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        }
        else
        {
            // If spawn time is positive, spawn the arrow after the delay
            StartCoroutine(DelayedArrowSpawn(spawnTime, selectedPrefab, spawnPoints[spawnIndex].position));
        }
    }

    IEnumerator DelayedArrowSpawn(float delay, GameObject prefab, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(delay);

        // Spawn the arrow after the delay
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}















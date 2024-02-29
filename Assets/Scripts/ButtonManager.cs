using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class ButtonExtensions
{
    public static bool HasOnClickListeners(this Button button)
    {
        return button.onClick != null && button.onClick.GetPersistentEventCount() > 0;
    }
}

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;

    public AudioClip buttonClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Create an AudioSource component if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Register for scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unregister from scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find buttons and add listeners when a new scene is loaded
        FindButtonsAndAddListeners();
    }

    private void FindButtonsAndAddListeners()
    {
        // Find all GameObjects in the scene
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        // Iterate through each GameObject
        foreach (GameObject obj in allGameObjects)
        {
            // Check if the GameObject has a Button component (including inactive ones)
            Button[] buttons = obj.GetComponentsInChildren<Button>(true);

            // Add click listeners to all found Button components
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(PlayButtonClickSound);
            }
        }
    }

    public void PlayButtonClickSound()
    {
        // Play button click sound if audio clip is provided
        if (buttonClip != null)
        {
            audioSource.PlayOneShot(buttonClip);
        }
    }
}





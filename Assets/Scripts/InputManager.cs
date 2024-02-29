using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private EventSystem eventSystem;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("InputManager");
                    instance = obj.AddComponent<InputManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Check if the EventSystem exists in the current scene
        eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem == null)
        {
            // If EventSystem doesn't exist, create a new one
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystem = eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }
    }

    public void DisableInput()
    {
        if (eventSystem != null)
        {
            eventSystem.enabled = false;
        }
    }

    public void EnableInput()
    {
        if (eventSystem != null)
        {
            eventSystem.enabled = true;
        }
    }
}




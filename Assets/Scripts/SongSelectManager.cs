using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour
{
    // Function to load a scene by name
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}


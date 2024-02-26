using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string mainMenuSceneName;
    public string leaderboardSceneName;
    // private ScoreManager scoreManager;

    // Method to load the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ViewLeaderboard()
    {
        SceneManager.LoadScene(leaderboardSceneName);
    }
}




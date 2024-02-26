using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;
    public TMP_InputField playerNameInput;
    public Button submitScoreButton;
    // public Button viewLeaderboardButton;
    public GameObject submitScorePanel;
    public GameObject submitScoreQuestionPanel;
    public string leaderboardSceneName;

    private int score;

    private void Start()
    {
        Debug.Log("Score Manager is starting");
        score = 0;
        ResetScore();
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void EnableSubmitScorePanel()
    {
        submitScoreQuestionPanel.SetActive(false);
        submitScorePanel.SetActive(true);
    }

    public void SubmitScore()
    {
        string playerName = playerNameInput.text;
        if (!string.IsNullOrEmpty(playerName))
        {
            // Call method in LeaderboardManager to handle saving the score
            LeaderboardManager.Instance.AddScore(playerName, score);
            playerNameInput.text = string.Empty;
        }
    }

    public void ViewLeaderboard()
    {
        SceneManager.LoadScene(leaderboardSceneName);
    }
}

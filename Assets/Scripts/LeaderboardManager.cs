using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private const int maxLeaderboardEntries = 10;

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
    }

    private void Start()
    {
        LoadLeaderboard();
    }

    public List<LeaderboardEntry> GetLeaderboardEntries()
    {
        return leaderboardEntries.OrderByDescending(entry => entry.Score).ToList();
    }

    public void AddScore(string playerName, int score)
    {
        LeaderboardEntry entry = new LeaderboardEntry(playerName, score);
        leaderboardEntries.Add(entry);
        SortLeaderboard();
        SaveLeaderboard();

        // Debug log to check if score is added
        Debug.Log("Score added: " + playerName + ", " + score);
    }

    private void SortLeaderboard()
    {
        leaderboardEntries = leaderboardEntries.OrderByDescending(entry => entry.Score).ToList();
        if (leaderboardEntries.Count > maxLeaderboardEntries)
        {
            leaderboardEntries.RemoveAt(leaderboardEntries.Count - 1);
        }
    }

    private void LoadLeaderboard()
    {
        leaderboardEntries.Clear();
        for (int i = 0; i < maxLeaderboardEntries; i++)
        {
            string playerName = PlayerPrefs.GetString($"PlayerName_{i}", "");
            int score = PlayerPrefs.GetInt($"Score_{i}", 0);
            if (!string.IsNullOrEmpty(playerName) && score > 0)
            {
                leaderboardEntries.Add(new LeaderboardEntry(playerName, score));
            }
        }

        // Debug log to check if leaderboard data is loaded
        Debug.Log("Leaderboard loaded. Entries count: " + leaderboardEntries.Count);
    }

    private void SaveLeaderboard()
    {
        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            PlayerPrefs.SetString($"PlayerName_{i}", leaderboardEntries[i].PlayerName);
            PlayerPrefs.SetInt($"Score_{i}", leaderboardEntries[i].Score);
        }
        PlayerPrefs.Save();
    }
}

[Serializable]
public class LeaderboardEntry
{
    public string PlayerName;
    public int Score;

    public LeaderboardEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}




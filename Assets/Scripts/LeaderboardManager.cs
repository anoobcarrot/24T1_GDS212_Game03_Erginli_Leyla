using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Dan.Main; // Include the Leaderboard Creator namespace

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private const int maxLeaderboardEntries = 10;
    private string publicKey = "98390a283d0040b07613d18824e969758b3b208b49a84a41e3a0fa06261226b2";

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
        // SaveLeaderboard();

        // Upload the new entry to the leaderboard
        LeaderboardCreator.UploadNewEntry(publicKey, playerName, score, (success) =>
        {
            if (success)
            {
                Debug.Log("Score added: " + playerName + ", " + score);
            }
            else
            {
                Debug.LogError("Failed to add score: " + playerName + ", " + score);
            }
        });
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
        // Retrieve the leaderboard entries from the Leaderboard Creator
        LeaderboardCreator.GetLeaderboard(publicKey, (entries) =>
        {
            leaderboardEntries = entries.Select(entry =>
            {
                return new LeaderboardEntry(entry.Username, entry.Score);
            }).ToList();

            Debug.Log("Leaderboard loaded. Entries count: " + leaderboardEntries.Count);
        });
    }

    // Method to update the leaderboard during runtime
    // public void UpdateLeaderboard()
    // {
    //     // Call LoadLeaderboard again to refresh the leaderboard
    //     LoadLeaderboard();
    // }

    // private void SaveLeaderboard()
    // {
        
    // }
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






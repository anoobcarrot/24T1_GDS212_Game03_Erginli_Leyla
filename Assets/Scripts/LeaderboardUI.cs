using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    public TMP_Text leaderboardText;
    private LeaderboardManager leaderboardManager;

    void Start()
    {
        leaderboardManager = LeaderboardManager.Instance;
        if (leaderboardManager != null)
        {
            DisplayLeaderboard();
        }
        else
        {
            Debug.LogError("LeaderboardManager instance not found!");
        }
    }

    void DisplayLeaderboard()
    {
        List<LeaderboardEntry> leaderboardEntries = leaderboardManager.GetLeaderboardEntries();

        if (leaderboardEntries.Count > 0)
        {
            string leaderboardString = "Top 10:\n";
            for (int i = 0; i < leaderboardEntries.Count; i++)
            {
                leaderboardString += $"{i + 1}. {leaderboardEntries[i].PlayerName}: {leaderboardEntries[i].Score}\n";
            }
            leaderboardText.text = leaderboardString;
        }
        else
        {
            leaderboardText.text = "There is currently no one on the leaderboard!";
        }
    }
}


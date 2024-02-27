using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;

public class LeaderboardTesting : MonoBehaviour
{
    [SerializeField] private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();
    private const int maxLeaderboardEntries = 10;

    private string publicLeaderboardKey = "98390a283d0040b07613d18824e969758b3b208b49a84a41e3a0fa06261226b2";

    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((msg) =>
        {

        }));
    }
}
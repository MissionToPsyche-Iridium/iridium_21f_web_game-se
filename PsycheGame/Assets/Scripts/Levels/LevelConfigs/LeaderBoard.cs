using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private string filePath;

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int totalScore;
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }

    private LeaderboardData leaderboardData;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        Debug.Log($"Leaderboard file path: {filePath}");

        leaderboardData = LoadLeaderboard();
    }

    public void SaveScore(string playerName, int levelScore)
    {
        LeaderboardEntry playerEntry = leaderboardData.entries.Find(entry => entry.playerName == playerName);

        if (playerEntry == null)
        {
            playerEntry = new LeaderboardEntry { playerName = playerName, totalScore = 0 };
            leaderboardData.entries.Add(playerEntry);
        }

        playerEntry.totalScore += levelScore;

        leaderboardData.entries = leaderboardData.entries
            .OrderByDescending(entry => entry.totalScore)
            .Take(10)
            .ToList();

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetTopScores()
    {
        return leaderboardData.entries
            .OrderByDescending(entry => entry.totalScore)
            .ToList();
    }

    private void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboardData);
        File.WriteAllText(filePath, json);
    }

    private LeaderboardData LoadLeaderboard()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<LeaderboardData>(json);
        }
        else
        {
            return new LeaderboardData();
        }
    }

    public void DisplayLeaderboard()
    {
        List<LeaderboardEntry> topScores = GetTopScores();
        Debug.Log("Top 10 Players:");
        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"{i + 1}. {topScores[i].playerName}: {topScores[i].totalScore}");
        }
    }
}
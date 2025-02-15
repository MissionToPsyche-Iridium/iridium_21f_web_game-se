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
        public int score;
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public Dictionary<int, List<LeaderboardEntry>> levelLeaderboards = new Dictionary<int, List<LeaderboardEntry>>();
    }

    private LeaderboardData leaderboardData;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        Debug.Log($"Leaderboard file path: {filePath}");

        leaderboardData = LoadLeaderboard();
    }

    public void SaveScore(int level, string playerName, int score)
    {
        if (!leaderboardData.levelLeaderboards.ContainsKey(level))
        {
            leaderboardData.levelLeaderboards[level] = new List<LeaderboardEntry>();
        }

        LeaderboardEntry newEntry = new LeaderboardEntry { playerName = playerName, score = score };
        leaderboardData.levelLeaderboards[level].Add(newEntry);

        leaderboardData.levelLeaderboards[level] = leaderboardData.levelLeaderboards[level]
            .OrderByDescending(entry => entry.score)
            .Take(10)
            .ToList();

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetTopScores(int level)
    {
        if (leaderboardData.levelLeaderboards.ContainsKey(level))
        {
            return leaderboardData.levelLeaderboards[level];
        }
        else
        {
            return new List<LeaderboardEntry>();
        }
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

    public void DisplayLeaderboard(int level)
    {
        List<LeaderboardEntry> topScores = GetTopScores(level);
        Debug.Log($"Top 10 Scores for Level {level}:");
        for (int i = 0; i < topScores.Count; i++)
        {
            Debug.Log($"{i + 1}. {topScores[i].playerName}: {topScores[i].score}");
        }
    }
}
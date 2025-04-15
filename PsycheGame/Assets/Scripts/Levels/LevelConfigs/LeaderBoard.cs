using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    private string filePath;
    public static LeaderBoard Instance { get; private set; }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public int totalScore;
        public Dictionary<int, int> levelScores = new Dictionary<int, int>();
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }

    private LeaderboardData leaderboardData;
    private const string PlayerNameKey = "PlayerName"; 

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeLeaderBoard();
    }

    public void InitializeLeaderBoard(){
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        }
        leaderboardData = LoadLeaderboard();
        Debug.Log($"Leaderboard file path: {filePath}");
    }

    public void SaveScore(int levelScore, int level)
    {
        string playerName = PlayerPrefs.GetString(PlayerNameKey);
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogError("Attempted to set an empty or null player name!");
            return;
        }
        LeaderboardEntry playerEntry = leaderboardData.entries.Find(entry => entry.playerName == playerName);

        if (playerEntry == null)
        {
            playerEntry = new LeaderboardEntry { playerName = playerName, totalScore = 0 };
            leaderboardData.entries.Add(playerEntry);
        }
        Debug.Log($"Saving score for {playerName}: Score={levelScore}, Level={level}");

        playerEntry.totalScore += levelScore;

        if (playerEntry.levelScores.ContainsKey(level))
        {
            playerEntry.levelScores[level] = Math.Max(playerEntry.levelScores[level], levelScore);
        }
        else
        {
            playerEntry.levelScores[level] = levelScore;
        }

        leaderboardData.entries = leaderboardData.entries
            .OrderByDescending(entry => entry.totalScore)
            .ToList();

        SaveLeaderboard();
    }

    public List<LeaderboardEntry> GetTopScores(int count = 10)
    {
        return leaderboardData.entries
            .OrderByDescending(entry => entry.totalScore)
            .Take(count)
            .ToList();
    }

    private void SaveLeaderboard()
    {
        try
        {
            string json = JsonUtility.ToJson(leaderboardData, true); // Pretty print for readability
            File.WriteAllText(filePath, json);
            Debug.Log("Leaderboard saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save leaderboard: {e.Message}");
        }
    }

    private LeaderboardData LoadLeaderboard()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);
                if (data == null)
                    {
                        Debug.LogWarning("Loaded JSON was invalid, initializing new leaderboard.");
                        return new LeaderboardData();
                    }
                    Debug.Log("Leaderboard loaded successfully");
                    return data;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load leaderboard: {e.Message}");
                return new LeaderboardData();
            }
        }
        Debug.Log("No leaderboard file found, starting fresh.");
        return new LeaderboardData();
    }

    public string DisplayTotalLeaderboard()
    {
        List<LeaderboardEntry> topScores = GetTopScores();
        string leaderboardText = "Top 10 Players (Total Score):\n";

        for (int i = 0; i < topScores.Count; i++)
        {
            leaderboardText += $"{i + 1}. {topScores[i].playerName}: {topScores[i].totalScore}\n";
        }

        return leaderboardText;
    }

    public string DisplayLeaderboardByLevel(int level)
    {
        var levelScores = leaderboardData.entries
            .Where(entry => entry.levelScores.ContainsKey(level))
            .Select(entry => new
            {
                entry.playerName,
                levelScore = entry.levelScores[level]
            })
            .OrderByDescending(entry => entry.levelScore)
            .Take(10)
            .ToList();

        string leaderboardText = $"Top 10 Players (Level {level}):\n";
        for (int i = 0; i < levelScores.Count; i++)
        {
            leaderboardText += $"{i + 1}. {levelScores[i].playerName}: {levelScores[i].levelScore}\n";
        }
        return leaderboardText;
    }
}
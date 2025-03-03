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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("LeaderBoard initialized");

        filePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
        Debug.Log($"Leaderboard file path: {filePath}");

        leaderboardData = LoadLeaderboard();
        
    }

    public void SaveScore(int levelScore, int level)
    {
        Debug.Log($"Score: " + levelScore + " Level: " + level);
        string playerName = PlayerPrefs.GetString(PlayerNameKey);
        LeaderboardEntry playerEntry = leaderboardData.entries.Find(entry => entry.playerName == playerName);

        if (playerEntry == null)
        {
            playerEntry = new LeaderboardEntry { playerName = playerName, totalScore = 0 };
            leaderboardData.entries.Add(playerEntry);
        }

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

    public bool IsPlayerNameUnique(string playerName)
    {
        LeaderboardData data = LoadLeaderboard();
        if(data.entries.Count > 0){
            if(data.entries.Any(entry => entry.playerName == playerName)){
                return false; 
            }
            return true; 
        } 
        return true;
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
            .Select(entry => new
            {
                entry.playerName,
                levelScore = entry.levelScores.ContainsKey(level) ? entry.levelScores[level] : 0
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
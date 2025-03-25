using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class LeaderBoardTests
{
    private LeaderBoard leaderBoard;
    private string testFilePath;

    [SetUp]
    public void SetUp()
    {
        GameObject go = new GameObject("LeaderBoardTest");
        leaderBoard = go.AddComponent<LeaderBoard>();
        Assert.IsNotNull(leaderBoard, "LeaderBoard component was not created");

        testFilePath = Path.Combine(Application.temporaryCachePath, "test_leaderboard.json");

        var field = typeof(LeaderBoard).GetField("filePath", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        Assert.IsNotNull(field, "Could not find filePath field via reflection");
        field.SetValue(leaderBoard, testFilePath);
        
        string currentPath = (string)field.GetValue(leaderBoard);
        Assert.AreEqual(testFilePath, currentPath, "Failed to set test file path");

        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
        leaderBoard.InitializeLeaderBoard();
    }

    [TearDown]
    public void TearDown()
    {
    if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
        if (leaderBoard != null && leaderBoard.gameObject != null)
        {
            Object.DestroyImmediate(leaderBoard.gameObject);
        }
    }

    [Test]
    public void SaveScore_NewPlayer_AddsEntry()
    {
        PlayerPrefs.SetString("PlayerName", "TestPlayer");

        leaderBoard.SaveScore(100, 1);

        var topScores = leaderBoard.GetTopScores();
        Assert.AreEqual(1, topScores.Count);
        Assert.AreEqual("TestPlayer", topScores[0].playerName);
        Assert.AreEqual(100, topScores[0].totalScore);
        Assert.AreEqual(100, topScores[0].levelScores[1]);
    }

    [Test]
    public void SaveScore_ExistingPlayer_UpdatesScore()
    {
        PlayerPrefs.SetString("PlayerName", "TestPlayer");
        leaderBoard.SaveScore(100, 1);

        leaderBoard.SaveScore(150, 1);

        var topScores = leaderBoard.GetTopScores();
        Assert.AreEqual(1, topScores.Count);
        Assert.AreEqual("TestPlayer", topScores[0].playerName);
        Assert.AreEqual(250, topScores[0].totalScore); 
        Assert.AreEqual(150, topScores[0].levelScores[1]); 
    }

    [Test]
    public void SaveScore_EmptyPlayerName_DoesNotSave()
    {
        PlayerPrefs.SetString("PlayerName", "");
        LogAssert.Expect(LogType.Error, "Attempted to set an empty or null player name!");
        leaderBoard.SaveScore(100, 1);

        var topScores = leaderBoard.GetTopScores();
        Assert.AreEqual(0, topScores.Count);
    }

    [Test]
    public void GetTopScores_LimitsToCount()
    {
        PlayerPrefs.SetString("PlayerName", "Player1");
        leaderBoard.SaveScore(100, 1);
        PlayerPrefs.SetString("PlayerName", "Player2");
        leaderBoard.SaveScore(200, 1);
        PlayerPrefs.SetString("PlayerName", "Player3");
        leaderBoard.SaveScore(300, 1);

        var topScores = leaderBoard.GetTopScores(2);

        Assert.AreEqual(2, topScores.Count);
        Assert.AreEqual(300, topScores[0].totalScore);
        Assert.AreEqual(200, topScores[1].totalScore);
    }

    [Test]
    public void DisplayTotalLeaderboard_ReturnsFormattedString()
    {
        PlayerPrefs.SetString("PlayerName", "Player1");
        leaderBoard.SaveScore(100, 1);
        PlayerPrefs.SetString("PlayerName", "Player2");
        leaderBoard.SaveScore(200, 1);

        string result = leaderBoard.DisplayTotalLeaderboard();

        StringAssert.Contains("Top 10 Players (Total Score):", result);
        StringAssert.Contains("1. Player2: 200", result);
        StringAssert.Contains("2. Player1: 100", result);
    }

    [Test]
    public void DisplayLeaderboardByLevel_ReturnsCorrectLevelScores()
    {
        PlayerPrefs.SetString("PlayerName", "Player1");
        leaderBoard.SaveScore(100, 1);
        leaderBoard.SaveScore(150, 2);
        PlayerPrefs.SetString("PlayerName", "Player2");
        leaderBoard.SaveScore(200, 1);

        string result = leaderBoard.DisplayLeaderboardByLevel(1);

        StringAssert.Contains("Top 10 Players (Level 1):", result);
        StringAssert.Contains("1. Player2: 200", result);
        StringAssert.Contains("2. Player1: 100", result);
    }

    [Test]
    public void LoadLeaderboard_PersistsData()
    {
        PlayerPrefs.SetString("PlayerName", "Player1");
        leaderBoard.SaveScore(100, 1);

        GameObject newGo = new GameObject();
        LeaderBoard newLeaderBoard = newGo.AddComponent<LeaderBoard>();
        var field = typeof(LeaderBoard).GetField("filePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field.SetValue(newLeaderBoard, testFilePath);

        newLeaderBoard.InitializeLeaderBoard();
        var topScores = newLeaderBoard.GetTopScores();

        Assert.AreEqual(1, topScores.Count);
        Assert.AreEqual("Player1", topScores[0].playerName);
        Assert.AreEqual(100, topScores[0].totalScore);

        Object.DestroyImmediate(newGo);
    }
}
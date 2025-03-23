using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class MissionStateTests
{
    private MissionState GetFreshMissionState()
    {
        MissionState.Instance.GetType()
            .GetField("instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            .SetValue(null, null);
        return MissionState.Instance;
    }

    [Test]
    public void MissionState_Singleton_ReturnsSameInstance()
    {
        var instance1 = MissionState.Instance;
        var instance2 = MissionState.Instance;

        Assert.AreSame(instance1, instance2, "MissionState should return the same instance");
    }

    [Test]
    public void MissionState_Singleton_InitializesOnlyOnce()
    {
        GetFreshMissionState(); 
        var instance1 = MissionState.Instance;

        var instance2 = MissionState.Instance;

        Assert.AreSame(instance1, instance2, "Second call should not create a new instance");
    }

    [Test]
    public void Initialize_SetsObjectivesAndLevelName()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 10, description = "Collect 10 gases" }
        };
        string levelName = "TestLevel";

        missionState.Initialize(objectives, levelName);

        Assert.AreEqual(objectives, missionState.Objectives, "Objectives should match the input list");
        Assert.AreEqual(levelName, missionState.levelName, "Level name should be set correctly");
        Assert.IsFalse(missionState.IsMissionComplete, "Mission should not be complete initially");
    }

    [Test]
    public void UpdateObjectiveProgress_IncrementsCorrectObjective()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 10, description = "Collect 10 gases" }
        };
        missionState.Initialize(objectives, "TestLevel");

        missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectGases, 5);

        var progress = missionState.GetObjectiveProgress(MissionState.ObjectiveType.CollectGases);
        Assert.AreEqual(5, progress, "Progress should increment by the specified amount");
    }

    [Test]
    public void UpdateObjectiveProgress_DoesNotExceedTarget()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 5, description = "Collect 5 metals" }
        };
        missionState.Initialize(objectives, "TestLevel");

        missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals, 10);

        var progress = missionState.GetObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals);
        Assert.AreEqual(5, progress, "Progress should not exceed target amount");
    }

    [Test]
    public void IsMissionComplete_TrueWhenAllRequiredObjectivesMet()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 10, description = "Collect 10 gases" },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 5, description = "Collect 5 metals" }
        };
        missionState.Initialize(objectives, "TestLevel");

        missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectGases, 10);
        missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectRareMetals, 5);

        Assert.IsTrue(missionState.IsMissionComplete, "Mission should be complete when both gas and metal objectives are met");
    }

    [Test]
    public void IsMissionComplete_FalseWhenOnlyOneObjectiveMet()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 10, description = "Collect 10 gases" },
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectRareMetals, targetAmount = 5, description = "Collect 5 metals" }
        };
        missionState.Initialize(objectives, "TestLevel");

        missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectGases, 10);

        Assert.IsFalse(missionState.IsMissionComplete, "Mission should not be complete with only one objective met");
    }

    [Test]
    public void GetObjectiveTarget_Uninitialized_ReturnsDefault()
    {
        var missionState = GetFreshMissionState();

        var target = missionState.GetObjectiveTarget(MissionState.ObjectiveType.ScanObject);

        Assert.AreEqual(50, target, "Uninitialized state should return default target of 50");
    }

    [Test]
    public void UpdateObjectiveProgress_InvalidType_DoesNotCrash()
    {
        var missionState = GetFreshMissionState();
        var objectives = new List<MissionState.MissionObjective>
        {
            new MissionState.MissionObjective { objectiveType = MissionState.ObjectiveType.CollectGases, targetAmount = 10 }
        };
        missionState.Initialize(objectives, "TestLevel");

        Assert.DoesNotThrow(() => missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.ScanObject, 5),
            "Updating progress for a non-existent objective type should not throw an exception");
    }
}
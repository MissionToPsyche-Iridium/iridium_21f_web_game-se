using System.Collections.Generic;
using UnityEngine;

public class MissionState
{

    private static MissionState instance;
    public static MissionState Instance
    {
    get
    {
        if (instance == null)
        {
            Debug.Log("New MissionState initialized");
            instance = new MissionState();
        }
        return instance;
    }
    }
    

    public List<MissionObjective> Objectives { get; private set; }
    public bool IsMissionComplete { get; private set; }

    public delegate void MissionStateUpdated();
    public static event MissionStateUpdated OnMissionStateChanged;

    public void Initialize(List<MissionObjective> initialObjectives)
    {
        Objectives = new List<MissionObjective>(initialObjectives);
        ResetObjectives();
        IsMissionComplete = false;
    }

    public void ResetObjectives()
    {
        foreach (var objective in Objectives)
        {
            objective.currentProgress = 0;
        }
        IsMissionComplete = false;
        OnMissionStateChanged?.Invoke();
    }

    public void UpdateObjectiveProgress(ObjectiveType type, int amount)
    {
        Debug.Log($"Updating progress for {type}: {amount}");
        foreach (var obj in Objectives)
        {
            if (obj.objectiveType == type)
            {
                obj.IncrementProgress(amount);
                Debug.Log($"Updated Objective: {obj.description} ({obj.currentProgress}/{obj.targetAmount})");
            }
        }
        IsMissionComplete = Objectives.TrueForAll(obj => obj.isCompleted);
    }

    public int GetObjectiveProgress(ObjectiveType type)
    {
        var targetObjective = Objectives.Find(obj => obj.objectiveType == type);
        return targetObjective != null ? targetObjective.currentProgress : 0;
    }

    [System.Serializable]
    public class MissionObjective
    {
        public ObjectiveType objectiveType;
        public string description;
        public int targetAmount;
        public int currentProgress = 0;
        public bool isCompleted => currentProgress >= targetAmount;

        public void IncrementProgress(int amount)
        {
            currentProgress = Mathf.Min(currentProgress + amount, targetAmount);
        }
    }

    public enum ObjectiveType
    {
        CollectResource,
        ScanObject,
    }
}

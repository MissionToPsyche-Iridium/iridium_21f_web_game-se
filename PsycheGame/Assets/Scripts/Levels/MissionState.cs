using System;
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
    public String levelName {get; private set;}
    public delegate void MissionStateUpdated();
    public static event MissionStateUpdated OnMissionStateChanged;

    public void Initialize(List<MissionObjective> initialObjectives, String name)
    {
        levelName = name;
        Objectives = new List<MissionObjective>(initialObjectives);
        IsMissionComplete = false;
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
        Debug.Log("Is mission complete: " + IsMissionComplete);
    }

    public int GetObjectiveProgress(ObjectiveType type)
    {
        var targetObjective = Objectives.Find(obj => obj.objectiveType == type);
        return targetObjective != null ? targetObjective.currentProgress : 0;
    }

    public int GetObjectiveTarget(ObjectiveType type){
        if(Objectives == null || Objectives.Count == 0){
            return 50;
        }
        var targetObjective = Objectives?.Find(obj => obj.objectiveType == type);
        if (targetObjective == null)
        {
            Debug.Log($"Objective List: {Objectives.Count}! Ensure MissionState is initialized properly.");
            return 50;
        }
        return targetObjective.GetTargetAmount();
    }

    [System.Serializable]
    public class MissionObjective
    {
        public ObjectiveType objectiveType;
        public string description;
        public int targetAmount;
        public int currentProgress = 0;
        public bool isCompleted
        {
            get
            {
                bool completed = currentProgress >= targetAmount;
                Debug.Log($"Objective: {description}, Current Progress: {currentProgress}, Target: {targetAmount}, Completed: {completed}");
                return completed;
            }
        }

        public void IncrementProgress(int amount)
        {
            int previousProgress = currentProgress;
            currentProgress = Mathf.Min(currentProgress + amount, targetAmount);
            Debug.Log($"Objective Progress: {description}, Progress: {previousProgress} -> {currentProgress}, Target: {targetAmount}");
        }

        public int GetTargetAmount(){
            return targetAmount;
        }
    }

    public enum ObjectiveType
    {
        CollectGases,
        CollectRareMetals,
        ScanObject,
    }
}

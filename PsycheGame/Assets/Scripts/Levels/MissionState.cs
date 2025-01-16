using System.Collections.Generic;
using UnityEngine;

public class MissionState
{

   private static MissionState _instance;
    public static MissionState Instance => _instance ?? (_instance = new MissionState());

    public List<MissionObjective> Objectives { get; private set; }
    public bool IsMissionComplete { get; private set; }

    public delegate void MissionStateUpdated();
    public static event MissionStateUpdated OnMissionStateChanged;

    private MissionState()
    {
        Objectives = new List<MissionObjective>();
        IsMissionComplete = false;
    }

    public void Initialize(List<MissionObjective> initialObjectives)
    {
        Objectives = new List<MissionObjective>(initialObjectives);
        ResetObjectives();
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
        var targetObjective = Objectives.Find(obj => obj.objectiveType == type && !obj.isCompleted);
        if (targetObjective != null)
        {
            targetObjective.IncrementProgress(amount);
            Debug.Log($"Updated Objective: {targetObjective.description} ({targetObjective.currentProgress}/{targetObjective.targetAmount})");

            IsMissionComplete = Objectives.TrueForAll(obj => obj.isCompleted);
            OnMissionStateChanged?.Invoke();
        }
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

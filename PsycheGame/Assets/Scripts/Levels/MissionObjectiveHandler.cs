using System.Collections.Generic;
using UnityEngine;

public class MissionState : MonoBehaviour
{
    public enum ObjectiveType
    {
        CollectResource,
        ScanObject,
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

    public List<MissionObjective> objectives; 
    public bool IsMissionComplete = false;

    public delegate void MissionStateUpdated();
    public static event MissionStateUpdated OnMissionStateChanged;

    private void Awake()
    {
        ResetObjectives();
    }
    private void Update()
    {
        if (IsMissionComplete)
        {
            Debug.Log("Mission Complete! All objectives achieved.");
        }
    }

    public void ResetObjectives()
    {
        foreach (var objective in objectives)
        {
            objective.currentProgress = 0;
        }
    }

    public void UpdateObjectiveProgress(ObjectiveType type, int amount)
    {
        var targetObjective = objectives.Find(obj => obj.objectiveType == type && !obj.isCompleted);
        if (targetObjective != null)
        {
            targetObjective.IncrementProgress(amount);
            Debug.Log($"Updated Objective: {targetObjective.description} ({targetObjective.currentProgress}/{targetObjective.targetAmount})");
            OnMissionStateChanged?.Invoke();
        }
    }
}

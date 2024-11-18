using System.Collections.Generic;
using UnityEngine;

public class MissionObjectiveHandler : MonoBehaviour
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
        public int currentProgress;
        public bool isCompleted;  
    }

    public List<MissionObjective> objectives; 
    public bool missionComplete = false;

    public delegate void OnMissionProgress();
    public static event OnMissionProgress MissionProgressUpdated;

    void Start()
    {
        InitializeObjectives();
    }

    void Update()
    {
        CheckMissionStatus();
    }

    void InitializeObjectives()
    {
        foreach (var objective in objectives)
        {
            objective.currentProgress = 0;
            objective.isCompleted = false;
        }
    }

    public void UpdateObjectiveProgress(ObjectiveType type, int amount = 1)
    {
        foreach (var objective in objectives)
        {
            if (objective.objectiveType == type && !objective.isCompleted)
            {
                objective.currentProgress += amount;
                if (objective.currentProgress >= objective.targetAmount)
                {
                    objective.currentProgress = objective.targetAmount;
                    objective.isCompleted = true;
                    Debug.Log($"Objective Completed: {objective.description}");
                }

                MissionProgressUpdated?.Invoke();
            }
        }
    }

    void CheckMissionStatus()
    {
        missionComplete = true;
        foreach (var objective in objectives)
        {
            if (!objective.isCompleted)
            {
                missionComplete = false;
                break;
            }
        }

        if (missionComplete)
        {
            Debug.Log("Mission Complete! All objectives achieved.");
        }
    }

    public void TrackGasCollection(int collectedAmount)
    {
        UpdateObjectiveProgress(ObjectiveType.CollectResource, collectedAmount);
    }
}

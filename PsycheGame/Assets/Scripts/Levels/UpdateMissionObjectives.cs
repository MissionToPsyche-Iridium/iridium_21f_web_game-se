using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMissionObjectives : MonoBehaviour
{
    MissionState missionState;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    void Start()
    {
        missionState =  MissionState.Instance;
        Update();
    }

    void Update()
    {
        string objectivesContent = $"Level: {missionState.levelName}\n";

        foreach (var objective in missionState.Objectives)
        {
            Debug.Log("Mission Objectve: " + objective.description + " : " + objective.targetAmount);
            objectivesContent += $"{objective.description}: {objective.targetAmount}\n";
        }
        textMeshProUGUI.text = objectivesContent;
        textMeshProUGUI.fontSize = 30;
        textMeshProUGUI.alignment = TextAlignmentOptions.Center;
    }
}

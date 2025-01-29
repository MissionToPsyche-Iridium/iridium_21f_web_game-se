using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMissionObjectives : MonoBehaviour
{
    MissionState missionState;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    void Update()
    {
        missionState =  MissionState.Instance;
        string objectivesContent = $"Level: {missionState.levelName}\n";

        foreach (var objective in missionState.Objectives)
        {
            objectivesContent += $"{objective.description}: {objective.targetAmount}\n";
        }
        textMeshProUGUI.text = objectivesContent;
        textMeshProUGUI.fontSize = 30;
        textMeshProUGUI.alignment = TextAlignmentOptions.Center;
    }
}

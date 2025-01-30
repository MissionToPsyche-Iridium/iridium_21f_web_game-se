using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateMissionObjectives : MonoBehaviour
{
    MissionState missionState;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    

    private void OnEnable()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }

    public void OnLevelLoaded(LevelConfig config)
    {
        UpdateUI();
    }

    void UpdateUI()
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

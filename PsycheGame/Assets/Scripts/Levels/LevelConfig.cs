using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public int rareAsteroidCount;
    public float scaleMin;
    public float scaleMax;
    public float missionTimer;
    public List<MissionState.MissionObjective> objectives;
}

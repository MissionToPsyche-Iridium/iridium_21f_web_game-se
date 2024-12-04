using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public int rareAsteroidCount;
    public float RMscaleMin;
    public float RMscaleMax;
    public int asteroidSpawnInterval;
    public int asteroidObjectLimit;
    public int ateroidInitialPopulation;

    public float asteroidScaleMin;
    public float asteroidScaleMax;
    public float asteroidVelocityMax;
    public float asteroidVelocityMin;
    public float missionTimer;
    public List<MissionState.MissionObjective> objectives;
}

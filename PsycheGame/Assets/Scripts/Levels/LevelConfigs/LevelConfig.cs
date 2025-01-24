using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public int rareAsteroidCount;
    public float RMscaleMin;
    public float RMscaleMax;
    public float missionTimer;

    public ObjectSpawner.ObjectSpawnerConfig gasSpawnerConfig;
    public ObjectSpawner.ObjectSpawnerConfig asteroidSpawnerConfig;
    public List<MissionState.MissionObjective> objectives;
}
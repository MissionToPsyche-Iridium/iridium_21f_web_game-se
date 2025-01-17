using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    [System.Serializable]
    public record ObjectSpawnerConfig {
        public int spawnInterval;
        public int objectLimit;
        public int initialPopulation;
        public float scaleMin;
        public float scaleMax;
        public float velocityMin;
        public float velocityMax;
    }

    public string levelName;
    public int rareAsteroidCount;
    public float RMscaleMin;
    public float RMscaleMax;
    public float missionTimer;

    public ObjectSpawnerConfig gasSpawnerConfig;
    public ObjectSpawnerConfig asteroidSpawnerConfig;
    public List<MissionState.MissionObjective> objectives;

    public int gasCount;
}

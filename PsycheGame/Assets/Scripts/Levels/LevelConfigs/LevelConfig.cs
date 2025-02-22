using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public float missionTimer;

    public ShipConfig levelShipConfig = null;
    public ObjectSpawner.ObjectSpawnerConfig gasSpawnerConfig;
    public ObjectSpawner.ObjectSpawnerConfig asteroidSpawnerConfig;
    public ObjectSpawner.ObjectSpawnerConfig rareMetalSpawnerConfig;
    public List<MissionState.MissionObjective> objectives;
}
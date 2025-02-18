using Unity;
using UnityEngine;

/*
    ProbeCapController.cs

    This script is the controller for translating (selected) probe design into the explorer capabilities/behavior.  

    Date: Feb 17, 2025
    Version: 1.0
    :: wire up initial skeleton for iterations
    >> To-Do:
        1. Implement the logic to load the probe design
        2. Develop static classes based on the rule calculation for the components into the explorer capabilities
            (reference ShipConfig.cs)
            public float fuel;
            public float health;
            public float moveSpeed;
            public float fuelConsumptionRate;
            public float boostMultiplier;
            public float bostChangeRate;
            Future features --> tether types (need sprite assets)
            Note: Probe flyer behaviors - yaw speed, move speed
        3. Create runner method that handles static classes calls to aggregate a generalized probe capabilities objec 
        to hand back to the explorer game (caller of this script)

*/

public class ProbeCapController : MonoBehaviour
{
    private GameObject _player;
    private BuildManager _buildManager;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _buildManager = _player.GetComponent<BuildManager>();
    }

    public void LoadProbeDesign()
    {
        foreach (GameObject probeComponent in _buildManager.GetSpawnedProbeComponents())
        {
            ProbeComponent component = _buildManager.GetProbeComponentInfo(probeComponent);
           
        }
    }
    public void CalculateProbeCapabilities()
    {
        LoadProbeDesign();
        // for each capability, fuel, health, yaw speed, move speed, fuel consumption rate, boost multiplier, 
        // boost change rate, etc. Calculate the probe capabilities based on the probe design
    }

}
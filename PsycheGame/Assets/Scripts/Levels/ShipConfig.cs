using UnityEngine;

// Rather than moving each config record to it's specific class like
// some configs for the level manager we keep these all in once place
// so integration with the ship builder has single refrence to ship
// variables.
[CreateAssetMenu(fileName = "ShipConfig", menuName = "Game/ShipConfig", order = 1)]
public class ShipConfig : MonoBehaviour
{
    public record TetherConfig
    {
        [Tooltip("The total number of line segments that make up the tether rendering")]
        public int resolution;
        [Tooltip("The initial speed the tether launches at before it attaches to an object")]
        public float launchSpeed;
        [Tooltip("The distance an object is kept at after pulling it towards the probe with the tether")]
        public float probeObjectDistance;
        [Tooltip("The speed at which the tether becomes a straight line after attaching to an object")]
        public float straightLineSpeed;
        [Tooltip("The initial size of the wave when launching the probe")]
        public float startWaveSize;
        [Tooltip("The speed of the animation")]
        public float progressionSpeed;
    }
    public record ScanConfig
    {
        [Tooltip("The distance at which objects can be scanned in the scene")]
        public float distance;
        [Tooltip("The number of 'invisable' ray's fired from the probe for hit detection, this should go up with Arc Angle")]
        public float resolution;
        [Tooltip("The arc angle which ray's are fired in, with 360.0 being a full circle and 1.0 being a single line")]
        public float arcAngle;
    }
    public record ShipMovementConfig
    {
        public float moveSpeed;
        public float fuelConsumptionRate;
        public float boostMultiplier;
        public float bostChangeRate;
    }
}

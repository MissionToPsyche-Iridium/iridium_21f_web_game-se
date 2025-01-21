using UnityEngine;

// Singleton class containing representing the 'state' of the probe
public class ShipManager : MonoBehaviour {
    private static ShipManager instance;
    public static readonly string _SHIP_GAMEOBJECT_NAME = "Ship";

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            _obj = GameObject.Find(_SHIP_GAMEOBJECT_NAME);
            if (_obj == null) {
                Debug.LogError("Failed to find 'Ship' game object in scene");
                return;
            }

            tetherLogic = _obj.GetComponent<ShipTetherLogic>();
            if (tetherLogic == null) {
                Debug.LogError("Failed to find 'ShipTetherLogic' script on ship");
                return;
            }

            scanner = _obj.GetComponent<ShipScanBehavior>();
            if (scanner == null) {
                Debug.LogError("Failed to find 'ShipScanner' script on ship");
                return;
            }
        }
    }

    private static GameObject _obj; 

    private static float fuel = 150f;
    private static float health = 100f;
    private static ShipTetherLogic tetherLogic;
    private static ShipScanBehavior scanner;

    public static ShipManager Instance { get { return instance; } }
    public static float Fuel { get { return fuel; } set { fuel = value; } }
    public static float Health {get {return health; } set {health = value; }}

    // This function must be called outside of the 'awake' function
    // otherwise a value of null will be returned
    public static GameObject Ship { get { return _obj; } }

    private void Start() {
        // TODO start initializing things here
        // Need to keep in mind at some point this configuration may be
        // driven via JSON object
    }
}

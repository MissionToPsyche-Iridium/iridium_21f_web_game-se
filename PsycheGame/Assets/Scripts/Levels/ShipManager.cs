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
            }
        }
    }

    private static GameObject _obj; 
    private static float fuel = 150f;

    public static ShipManager Instance { get { return instance; } }
    public static float Fuel { get { return fuel; } set { fuel = value; } }

    // This function must be called outside of the 'awake' function
    // otherwise a value of null will be returned
    public static GameObject Ship { get { return _obj; } }

}

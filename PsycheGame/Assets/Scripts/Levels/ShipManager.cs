using UnityEngine;

public class ShipManager : MonoBehaviour {
    private static ShipManager instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
        } else {
            instance = this;
            _obj = GameObject.Find("Ship");
            if (_obj == null) {
                Debug.LogError("Failed to find 'Ship' game object in scene");
            }
        }
    }

    private static GameObject _obj; 
    private static float fuel = 150f;

    public static ShipManager Instance { get { return instance; } }
    public static float Fuel { get { return fuel; } set { fuel = value; } }
    public static GameObject Ship { get { return _obj; } }

}

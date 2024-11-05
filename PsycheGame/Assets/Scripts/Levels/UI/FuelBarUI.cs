using UnityEngine;
using UnityEngine.UI;

public class FuelBarUi : MonoBehaviour {
    [SerializeField] private GameObject fuelBarColor;
    [SerializeField] private Slider fuelBar;

    private void Start() {
        UpdateFuelDisplay();
    }

    private void Update() {
        UpdateFuelDisplay(); 
    }

    private void UpdateFuelDisplay() {
        float fuel = ShipManager.Fuel;
        fuelBar.value = fuel;

        if (fuel < 25f) {
            fuelBarColor.GetComponent<Image>().color = Color.red;
        } else if (fuel < 50f) { 
            fuelBarColor.GetComponent<Image>().color = Color.yellow;
        } else {
            fuelBarColor.GetComponent<Image>().color = Color.green;
        }
    }
}

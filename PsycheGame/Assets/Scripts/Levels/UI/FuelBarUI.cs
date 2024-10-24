using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarUI : MonoBehaviour {
    [SerializeField] private GameObject fuelBarColor;
    [SerializeField] private GameObject fuelBarPanel;
    [SerializeField] private Slider fuelBar;

    private Image fuelBarImage = null;
    private Image fuelBarPanelImage = null;

    private static readonly float FUEL_LOW_LEVEL = 25f;
    private static readonly float FUEL_MID_LEVEL = 50f;
    private static readonly float FUEL_LOW_THRESHOLD = 20f;

    private void Start() {
        this.fuelBarImage = fuelBarColor.GetComponent<Image>();
        this.fuelBarPanelImage = fuelBarPanel.GetComponent<Image>();
    }

    private void Update() {
        float fuel = ShipManager.Fuel;
        fuelBar.value = fuel;

        if (fuel < FUEL_LOW_LEVEL) {
            fuelBarImage.color = Color.red;
            StartCoroutine(FlashLowFuel());
        } else if (fuel < FUEL_MID_LEVEL) {
            fuelBarImage.color = Color.yellow;
        } else {
            fuelBarImage.color = Color.green;
        }
    }

    private IEnumerator FlashLowFuel() {
        while (ShipManager.Fuel < FUEL_LOW_THRESHOLD) {
            fuelBarPanelImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            fuelBarPanelImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }   
    }
}

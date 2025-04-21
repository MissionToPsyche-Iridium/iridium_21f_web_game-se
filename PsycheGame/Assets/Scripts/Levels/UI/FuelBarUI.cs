using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelBar : MonoBehaviour {
    [SerializeField] public GameObject fuelBarColor;
    [SerializeField] public Slider fuelBar;
    [SerializeField] public TextMeshProUGUI textDisplay;

    public Image fuelBarImage = null;
    private Coroutine flashCoroutine;

    private static readonly float FUEL_LOW_LEVEL = 25f;
    private static readonly float FUEL_MID_LEVEL = 50f;
    private static readonly float FUEL_LOW_THRESHOLD = 20f;

    private void Start() {
        this.fuelBarImage = fuelBarColor.GetComponent<Image>();
        UpdateIndicator(ShipManager.Fuel);
    }

    public void UpdateIndicator(float fuel) {
        fuelBar.value = fuel;
        textDisplay.text = $"{Mathf.FloorToInt(fuel)}";
        
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

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
            fuelBarImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            fuelBarImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        UpdateIndicator(ShipManager.Fuel);   
    }

    private void Awake()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }

    public void OnLevelLoaded(LevelConfig config)
    {
        UpdateIndicator(ShipManager.Fuel);
    }
}

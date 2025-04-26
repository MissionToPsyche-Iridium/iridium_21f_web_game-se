using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelBar : MonoBehaviour {
    [SerializeField] public GameObject fuelBarColor;
    [SerializeField] public Slider fuelBar;
    [SerializeField] public TextMeshProUGUI textDisplay;

    public Image fuelBarImage = null;
    private Coroutine flashCoroutine = null;

    private static readonly float FUEL_LOW_LEVEL = 25f;
    private static readonly float FUEL_MID_LEVEL = 50f;
    private static readonly float FUEL_LOW_THRESHOLD = 20f;

    private void Start() {
        this.fuelBarImage = fuelBarColor.GetComponent<Image>();
        UpdateIndicator(ShipManager.Fuel);
    }

    public void UpdateIndicator(float fuel) {
        Debug.Log("Fuel: " + fuel);
        fuelBar.value = fuel;
        textDisplay.text = $"{Mathf.FloorToInt(fuel)}";

        if (fuel < FUEL_LOW_LEVEL)
        {
            if (flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(FlashLowFuel());
            }
        }
        else if (fuel < FUEL_MID_LEVEL)
        {
            if (flashCoroutine != null) {  StopCoroutine(flashCoroutine); }
            fuelBarImage.color = Color.yellow;
        }
        else
        {
            if (flashCoroutine != null) {  StopCoroutine(flashCoroutine); }
            fuelBarImage.color = Color.green;
        }
    }

    private IEnumerator FlashLowFuel() {
        while (true)
        {
            fuelBarImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            fuelBarImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
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

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour {
    [SerializeField] private GameObject healthBarColor;
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI textDisplay;

    private Image healthBarImage = null;

    private static readonly float HEALTH_LOW_LEVEL = 25f;
    private static readonly float HEALTH_MID_LEVEL = 50f;
    private static readonly float HEALTH_LOW_THRESHOLD = 20f;

    private void Start() {
        this.healthBarImage = healthBarColor.GetComponent<Image>();
    }

    public void UpdateIndicator() {
        float health = ShipManager.Health;
        healthBar.value = health;
        textDisplay.text = $"{health}";

        if (health < HEALTH_LOW_LEVEL) {
            healthBarImage.color = Color.red;
            StartCoroutine(FlashLowHealth());
        } else if (health < HEALTH_MID_LEVEL) {
            healthBarImage.color = Color.yellow;
        } else {
            healthBarImage.color = Color.green;
        }
    }

    private IEnumerator FlashLowHealth() {
        while (ShipManager.Health < HEALTH_LOW_THRESHOLD) {
            healthBarImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            healthBarImage.color = Color.white;
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
        UpdateIndicator();
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private GameObject healthBarColor;
    [SerializeField] private GameObject healthBarPanel;
    [SerializeField] private Slider healthBar;

    private Image healthBarImage = null;
    private Image healthBarPanelImage = null;

    private static readonly float HEALTH_LOW_LEVEL = 25f;
    private static readonly float HEALTH_MID_LEVEL = 50f;
    private static readonly float HEALTH_LOW_THRESHOLD = 20f;

    private void Start() {
        this.healthBarImage = healthBarColor.GetComponent<Image>();
        this.healthBarPanelImage = healthBarPanel.GetComponent<Image>();
    }

    public void UpdateIndicator() {
        float health = ShipManager.Health;
        healthBar.value = health;

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
            healthBarPanelImage.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            healthBarPanelImage.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }   
    }
}
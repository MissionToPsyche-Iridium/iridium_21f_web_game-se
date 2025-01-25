using UnityEngine;
using UnityEngine.UI;

public class RareMetalCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject rareMetalCollectionBarColor;
    [SerializeField] private Slider rareMetalCollectBar;
    MissionState missionState;

    private Image rareMetalCollectBarImage = null;
    private float totalMined = 0;

    private float MID_LEVEL;
    private float targetAmount;

    private void Start() {
        this.rareMetalCollectBarImage = rareMetalCollectionBarColor.GetComponent<Image>();
        missionState = MissionState.Instance;
        targetAmount = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectRareMetals);
        MID_LEVEL = targetAmount * 50f;
    }

    public void UpdateIndicator(int minedAmount) {
        totalMined = minedAmount + totalMined;
        Debug.Log($"RareMetalStatusBar updating indicator: {minedAmount}");
        rareMetalCollectBar.value = totalMined;

        if (totalMined >= targetAmount) {
            rareMetalCollectBarImage.color = Color.green;
        } else if (totalMined >= MID_LEVEL) {
            rareMetalCollectBarImage.color = Color.yellow;
        } else {
            rareMetalCollectBarImage.color = Color.red;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class RareMetalCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject rareMetalCollectionBarColor;
    [SerializeField] private Slider rareMetalCollectBar;
    MissionState missionState;

    private Image rareMetalCollectBarImage = null;
    private float totalMined = 0;

    private float LOW_LEVEL;
    private float MID_LEVEL;

    private void Start() {
        this.rareMetalCollectBarImage = rareMetalCollectionBarColor.GetComponent<Image>();
        missionState = MissionState.Instance;
        int amount = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectRareMetals);
        LOW_LEVEL = amount * 33f;
        MID_LEVEL = amount * 66f;
    }

    public void UpdateIndicator(int minedAmount) {
        totalMined = minedAmount + totalMined;
        Debug.Log($"RareMetalStatusBar updating indicator: {minedAmount}");
        rareMetalCollectBar.value = totalMined;

        if (totalMined < LOW_LEVEL) {
            rareMetalCollectBarImage.color = Color.red;
        } else if (totalMined < MID_LEVEL) {
            rareMetalCollectBarImage.color = Color.yellow;
        } else {
            rareMetalCollectBarImage.color = Color.green;
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class RareMetalCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject rareMetalCollectionBarColor;
    [SerializeField] private GameObject rareMetalCollectionBarPanel;
    [SerializeField] private Slider rareMetalCollectBar;
    MissionState missionState;

    private Image rareMetalCollectBarImage = null;
    private Image rareMetalCollectPanelImage = null;
    private float totalMined = 0;

    private float LOW_LEVEL;
    private float MID_LEVEL;

    private void Start() {
        this.rareMetalCollectBarImage = rareMetalCollectionBarColor.GetComponent<Image>();
        this.rareMetalCollectPanelImage = rareMetalCollectionBarPanel.GetComponent<Image>();
        missionState = MissionState.Instance;
        int amount = missionState.GetObjectiveProgress(MissionState.ObjectiveType.CollectResource);
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
using UnityEngine;
using UnityEngine.UI;

public class RareMetalCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject rareMetalCollectionBarColor;
    [SerializeField] private GameObject rareMetalCollectionBarPanel;
    [SerializeField] private Slider rareMetalCollectBar;

    private Image rareMetalCollectBarImage = null;
    private Image rareMetalCollectPanelImage = null;

    private static readonly float LOW_LEVEL = 33f;
    private static readonly float MID_LEVEL = 66f;

    private void Start() {
        this.rareMetalCollectBarImage = rareMetalCollectionBarColor.GetComponent<Image>();
        this.rareMetalCollectPanelImage = rareMetalCollectionBarPanel.GetComponent<Image>();
    }

    public void UpdateIndicator() {
        float rareMetalCollected = MissionState.Instance.GetObjectiveProgress(MissionState.ObjectiveType.CollectResource);
        rareMetalCollectBar.value = rareMetalCollected;

        if (rareMetalCollected < LOW_LEVEL) {
            rareMetalCollectBarImage.color = Color.red;
        } else if (rareMetalCollected < MID_LEVEL) {
            rareMetalCollectBarImage.color = Color.yellow;
        } else {
            rareMetalCollectBarImage.color = Color.green;
        }
    }
}
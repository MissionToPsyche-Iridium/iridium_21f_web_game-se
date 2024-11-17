using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GasCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject gasCollectionBarColor;
    [SerializeField] private GameObject gasCollectionBarPanel;
    [SerializeField] private Slider gasCollectBar;

    private Image gasCollectBarImage = null;
    private Image gasCollectPanelImage = null;

    private static readonly float LOW_LEVEL = 33f;
    private static readonly float MID_LEVEL = 66f;

    private void Start() {
        this.gasCollectBarImage = gasCollectionBarColor.GetComponent<Image>();
        this.gasCollectPanelImage = gasCollectionBarPanel.GetComponent<Image>();
    }

    public void UpdateIndicator() {
        float gasCollected = MissionState.Instance.GetObjectiveProgress(MissionState.ObjectiveType.CollectResource);
        gasCollectBar.value = gasCollected;

        if (gasCollected < LOW_LEVEL) {
            gasCollectBarImage.color = Color.red;
        } else if (gasCollected < MID_LEVEL) {
            gasCollectBarImage.color = Color.yellow;
        } else {
            gasCollectBarImage.color = Color.green;
        }
    }
}
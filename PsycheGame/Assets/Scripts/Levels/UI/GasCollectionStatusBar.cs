using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GasCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject gasCollectionBarColor;
    [SerializeField] private Slider gasCollectBar; 
    MissionState missionState;
    float gasTotal = 0;

    private Image gasCollectBarImage = null;

    private float LOW_LEVEL;
    private float MID_LEVEL;

    private void Start() {
        this.gasCollectBarImage = gasCollectionBarColor.GetComponent<Image>();        
        missionState = MissionState.Instance;
        int amount = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectGases);
        LOW_LEVEL = amount * 33f;
        MID_LEVEL = amount * 66f;
    }

    public void UpdateIndicator(int amount) {
        gasTotal = gasTotal + amount;
        Debug.Log($"GasStatusBar updating indicator: {amount}");
        gasCollectBar.value = gasTotal;

        if (gasTotal >= MID_LEVEL) {
            gasCollectBarImage.color = Color.green;
        } else if (gasTotal > LOW_LEVEL) {
            gasCollectBarImage.color = Color.yellow;
        } else {
            gasCollectBarImage.color = Color.red;
        }
    }
}
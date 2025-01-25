using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GasCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject gasCollectionBarColor;
    [SerializeField] private Slider gasCollectBar; 
    MissionState missionState;
    float gasTotal = 0;

    private Image gasCollectBarImage = null;

    private float MID_LEVEL;
    private float targetAmount;

    private void Start() {
        this.gasCollectBarImage = gasCollectionBarColor.GetComponent<Image>();        
        missionState = MissionState.Instance;
        targetAmount = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectGases);
        MID_LEVEL = targetAmount * 50;
    }

    public void UpdateIndicator(int amount) {
        gasTotal = gasTotal + amount;
        Debug.Log($"GasStatusBar updating indicator: {amount}");
        gasCollectBar.value = gasTotal;

        if (gasTotal >= targetAmount) {
            gasCollectBarImage.color = Color.green;  
        } else if (gasTotal >= MID_LEVEL) {
            gasCollectBarImage.color = Color.yellow;
        } else {
            gasCollectBarImage.color = Color.red;
        }
    }
}
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
        ResetStatusBar();
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
        ResetStatusBar();
    }

    public void ResetStatusBar(){
        missionState = MissionState.Instance;
        gasTotal = 0;
        LOW_LEVEL = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectGases) * 33f;
        MID_LEVEL = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectGases) * 66f;
        UpdateIndicator(0);
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
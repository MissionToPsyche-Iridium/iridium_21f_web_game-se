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
    private void OnEnable() {
        CollectionEvents.OnMetalCollected += UpdateIndicator;
    }

    private void OnDisable() {
        CollectionEvents.OnMetalCollected -= UpdateIndicator;
    }


    public void OnLevelLoaded(LevelConfig config)
    {
        ResetStatusBar();
    }

    public void ResetStatusBar(){
        missionState = MissionState.Instance;
        totalMined = 0;
        LOW_LEVEL = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectRareMetals) * 33f;
        MID_LEVEL = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectRareMetals) * 66f;

        UpdateIndicator(0);
    }

    public void UpdateIndicator(int amount) {

        totalMined = amount + totalMined;
        Debug.Log($"RareMetalStatusBar updating indicator: {amount}");
        rareMetalCollectBar.value = totalMined;

        if (totalMined >= MID_LEVEL) {
            rareMetalCollectBarImage.color = Color.green;
        } else if (totalMined > LOW_LEVEL) {
            rareMetalCollectBarImage.color = Color.yellow;
        } else {
            rareMetalCollectBarImage.color = Color.red;
        }
    }
}
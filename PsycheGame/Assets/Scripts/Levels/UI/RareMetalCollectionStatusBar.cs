using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RareMetalCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject rareMetalCollectionBarColor;
    [SerializeField] private Slider rareMetalCollectBar;
    [SerializeField] private TextMeshProUGUI textDisplay;
    MissionState missionState;

    private Image rareMetalCollectBarImage = null;
    private string metalCollected = "";
    private float totalMined = 0;
    private float target;

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
        target = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectRareMetals);
        MID_LEVEL = target * .66f;

        UpdateIndicator(0);
    }

    public void UpdateIndicator(int amount) {

        totalMined = amount + totalMined;
        Debug.Log($"RareMetalStatusBar updating indicator: {amount}");
        rareMetalCollectBar.value = totalMined;
        textDisplay.text = $"{totalMined}/{target}";
        if (totalMined >= target){
            rareMetalCollectBarImage.color = Color.green;
        } else if (totalMined > MID_LEVEL) {
            rareMetalCollectBarImage.color = Color.yellow;
        } else {
            rareMetalCollectBarImage.color = Color.red;
        }
    }
}
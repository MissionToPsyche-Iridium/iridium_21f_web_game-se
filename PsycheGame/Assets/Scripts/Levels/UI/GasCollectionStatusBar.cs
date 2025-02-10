using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GasCollectionStatusBar : MonoBehaviour {
    [SerializeField] private GameObject gasCollectionBarColor;
    [SerializeField] private Slider gasCollectBar; 
    [SerializeField] private TextMeshProUGUI textDisplay;

    MissionState missionState;
    float gasTotal = 0;
    float target = 0;

    private Image gasCollectBarImage = null;

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

    private void OnEnable() {
        CollectionEvents.OnGasCollected += UpdateIndicator;
    }

    private void OnDisable() {
        CollectionEvents.OnGasCollected -= UpdateIndicator;
    }

    public void OnLevelLoaded(LevelConfig config)
    {
        ResetStatusBar();
    }

    public void ResetStatusBar(){
        missionState = MissionState.Instance;
        gasTotal = 0;
        target = missionState.GetObjectiveTarget(MissionState.ObjectiveType.CollectGases);
        MID_LEVEL =  target * .66f;
        UpdateIndicator(0);
    }

    public void UpdateIndicator(int amount) {
        gasTotal = gasTotal + amount;
        Debug.Log($"GasStatusBar updating indicator: {amount}");
        gasCollectBar.value = gasTotal;
        textDisplay.text = $"{gasTotal}/{target}";
        if (gasTotal >= target){
            gasCollectBarImage.color = Color.green;
        } else if (gasTotal > MID_LEVEL) {
            gasCollectBarImage.color = Color.yellow;
        } else {
            gasCollectBarImage.color = Color.red;
        }
    }
}
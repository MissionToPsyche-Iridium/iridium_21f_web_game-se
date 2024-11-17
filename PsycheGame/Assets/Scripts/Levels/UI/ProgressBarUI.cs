using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;

    public Progress progress;

    private void Awake() {
        mask.fillAmount = 0; 
    }

    private void Update() {
        if (progress != null) {
            float fillAmount = progress.Value / Progress.COMPLETE_PROGRESS;        
            mask.fillAmount = fillAmount;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;
    [SerializeField] private Color color;

    public Progress progress;

    void Update() {
        if (progress != null) {
            float fillAmount = progress.Value / Progress.COMPLETE_PROGRESS;        
            fill.color = color;
            mask.fillAmount = fillAmount;
        }
    }
}

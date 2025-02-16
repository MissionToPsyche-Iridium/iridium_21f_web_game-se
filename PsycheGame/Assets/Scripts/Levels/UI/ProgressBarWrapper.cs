using UnityEngine;
using UnityEngine.UI;

public class ProgressBarWrapper : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image mask; 
    
    private Progress progress = new Progress(Progress.NO_PROGRESS);
    private CanvasRenderer cr = null;
    private CanvasRenderer maskCr = null;

    private void Awake() {
        cr = this.GetComponent<CanvasRenderer>();
        maskCr = mask.GetComponent<CanvasRenderer>();
        mask.fillAmount = 0; 
    }
    public void UpdateProgress(float progressValue)
    {
        float fillAmount = Mathf.Clamp01(progressValue / 100f);
        mask.fillAmount = fillAmount;
    }
}
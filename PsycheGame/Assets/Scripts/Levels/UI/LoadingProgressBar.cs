using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;

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

        cr.SetAlpha(1.0f);
        maskCr.SetAlpha(1.0f);
    }

    public void HideProgressBar()
    {
        cr.SetAlpha(0.0f);
        maskCr.SetAlpha(0.0f);
    }

    public void DestroyProgressBar()
    {
        Destroy(this.gameObject);
    }
}
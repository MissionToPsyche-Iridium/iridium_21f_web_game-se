using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    public float fill = 0f;

    public void UpdateProgress(float progressValue)
    {
        float fillAmount = Mathf.Clamp01(progressValue / 100f);
        fill = fillAmount;
    }

    public void DestroyProgressBar()
    {
        Destroy(this.gameObject);
    }
}
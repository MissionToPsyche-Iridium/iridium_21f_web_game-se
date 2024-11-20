using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;

    private CanvasRenderer cr = null;
    private CanvasRenderer maskCr = null;
    private Coroutine hideCoroutine = null;
    private float lastProgressVal = -1;

    public ScannableObject scanning;

    private void Awake() {
        cr = this.GetComponent<CanvasRenderer>();
        maskCr = mask.GetComponent<CanvasRenderer>();
        mask.fillAmount = 0; 
    }

    private void Update() {
        if (scanning != null) {
            Progress progress = scanning.ScanProgress;

            if (progress.Value <= lastProgressVal) { // no progress gained
                hideCoroutine ??= StartCoroutine(HideSelf());
            } else { // progress is being made
                if (hideCoroutine != null) {
                    StopCoroutine(hideCoroutine);
                    hideCoroutine = null;
                }

                maskCr.SetAlpha(1.0f);
                cr.SetAlpha(1.0f);
                UpdateBarFill(progress);
            }
        }
    }

    private void UpdateBarFill(Progress progress) {
        if (progress.isComplete) {
            StartCoroutine(DestorySelf());
        } else {
            float fillAmount = progress.Value / Progress.COMPLETE_PROGRESS;
            mask.fillAmount = fillAmount;
            lastProgressVal = progress.Value;
        }
    }

    private IEnumerator HideSelf() {
        yield return new WaitForSeconds(4f);
        cr.SetAlpha(0.0f);
        maskCr.SetAlpha(0.0f);
    }

    // once this progress bar ui element has been filled this coroutine
    // is dispatched to destory itself after a peroid of waiting
    private IEnumerator DestorySelf() {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}

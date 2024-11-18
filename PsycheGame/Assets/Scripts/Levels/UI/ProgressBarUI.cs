using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour {
    [SerializeField] private Image fill;
    [SerializeField] private Image mask;

    public ScannableObject scanning;

    private void Awake() {
        mask.fillAmount = 0; 
    }

    private void Update() {
        if (scanning != null) {
            Progress progress = scanning.ScanProgress;
            
            if (progress.isComplete) {
                StartCoroutine(DestorySelf());
                progress = null;
            } else {
                float fillAmount = progress.Value / Progress.COMPLETE_PROGRESS;
                mask.fillAmount = fillAmount;
            }
        }
    }

    // once this progress bar ui element has been filled this coroutine
    // is dispatched to destory itself after a peroid of waiting
    private IEnumerator DestorySelf() {
        yield return new WaitForSeconds(4);
        Destroy(this.gameObject);
    }
}

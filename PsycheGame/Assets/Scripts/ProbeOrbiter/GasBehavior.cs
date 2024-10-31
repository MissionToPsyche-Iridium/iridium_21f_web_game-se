using System.Collections;
using UnityEngine;

public class GasBehavior : MonoBehaviour {
    private ParticleSystem ps;
    private Material psMaterial;

    [Header("Start Color")]
    [SerializeField] private Color startColor = Color.white;

    [Header("Color Change On Collision")]
    [SerializeField] private bool colorChangeCollision = false;
    [SerializeField] private bool fadeBack = false;
    [SerializeField] private Color finalColor;
    [SerializeField] private float changeSpeed = 1.0f;
    [SerializeField] private float fadeBackWaitTime = 2f;

    private void Awake() {
        FetchAndAssingAssets();
    }

    private void OnValidate() {
        FetchAndAssingAssets();
    }

    private void Start() {
        //FadeColors(Color.red, 0.5f);   
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!colorChangeCollision) return;
        Coroutine fadeRoutine = FadeColors(finalColor, changeSpeed);
        IEnumerator fadeBackRoutine = ColorChange(finalColor, startColor, changeSpeed);
        StartCoroutine(
            WaitDelayRun(fadeRoutine, fadeBackWaitTime, fadeBackRoutine)
        );
    }

    // Fade from one color to another
    protected Coroutine FadeColors(Color endColor, float duration) {
        Color startColor = psMaterial.color;
        IEnumerator fadeCoroutine = ColorChange(startColor, endColor, duration);
        return StartCoroutine(fadeCoroutine);
    }

    private IEnumerator ColorChange(Color startColor, Color endColor, float duration) {
        float tick = 0f;
        while (psMaterial.color != endColor) {
            tick += Time.deltaTime * duration;
            psMaterial.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }

    private IEnumerator WaitDelayRun(Coroutine wait, float delay, IEnumerator run) {
        while (wait != null) { }
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(run);
    }

    private void FetchAndAssingAssets() {
        ps = this.GetComponent<ParticleSystem>();
        psMaterial = ps.GetComponent<ParticleSystemRenderer>().sharedMaterial;
        psMaterial.color = startColor;
    }
} 
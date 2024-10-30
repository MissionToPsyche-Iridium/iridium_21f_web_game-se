using System.Collections;
using UnityEngine;

public class GasBehavior : MonoBehaviour {
    private ParticleSystem ps;
    private Material psMaterial;

    [Header("Color")]
    [SerializeField] private Color startColor = Color.white;

    private void Awake() {
        ps = this.GetComponent<ParticleSystem>();
        psMaterial = ps.GetComponent<ParticleSystemRenderer>().sharedMaterial;
        psMaterial.color = startColor;
    }

    private void Start() {
        FadeColors(Color.red, 0.5f);   
    }

    // Fade from one color to another
    protected void FadeColors(Color endColor, float duration) {
        Color startColor = psMaterial.color;
        IEnumerator fadeCoroutine = ColorChange(startColor, endColor, duration);
        StartCoroutine(fadeCoroutine);
    }

    private IEnumerator ColorChange(Color startColor, Color endColor, float duration) {
        float tick = 0f;
        while (psMaterial.color != endColor) {
            tick += Time.deltaTime * duration;
            psMaterial.color = Color.Lerp(startColor, endColor, tick);
            yield return null;
        }
    }
} 
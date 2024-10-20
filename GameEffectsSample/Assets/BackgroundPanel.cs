using UnityEngine;

public class BackgroundPanel : MonoBehaviour {

    // this material is in refrence to the star shader material which we
    // have attached to the sprite renderer. We can then grab named components
    // from this material to programatically update the properties of a background
    // panel
    private Material backgroundMat;

    [SerializeField] private bool reverseParallaxDirection = false;
    [SerializeField] private float parralaxStrength = 10.0f;

    private void Start() {
        this.backgroundMat = this.GetComponent<SpriteRenderer>().material;
    }

    private void Update() {
        // Modulate the brightness (makes a flickering star effect)
        float brightnessScale = backgroundMat.GetFloat("_StarBrightnessScale");
        backgroundMat.SetFloat("_StarBrightnessScale", brightnessScale + 0.001f % 10f);

        // should only do this if transform has changed
        Vector2 offset = backgroundMat.GetVector("_Offset");
        offset.x = transform.position.x / transform.localScale.x / parralaxStrength;
        offset.y = transform.position.y / transform.localScale.y / parralaxStrength;
        offset = reverseParallaxDirection ? offset : -offset;
        backgroundMat.SetVector("_Offset", offset);
    }

}

using UnityEngine;

// Control the parallax effect of sprite renderer using a star field
// material
[RequireComponent(typeof(SpriteRenderer))]
public class StarBackgroundController : MonoBehaviour {

    // A material has attributes defined on it which are fetched via handles
    // named in the shader. These are those handles which MUST not be changed
    private static readonly string _ptr_star_offset = "_Offset";
    private static readonly string _ptr_brightness_scale = "_StarBrightnessScale";

    // Material in refrence to the star shader attached to the sprite renderer
    // of this component. 
    private Material starMaterial;
    private SpriteRenderer sRenderer;
    private Transform camTransform;

    [SerializeField] private Camera cam;
    [SerializeField] private float parallaxStrength = 10.0f;
    [SerializeField] private bool reverseParallaxDirection = false;

    private void Start() {
        this.sRenderer = GetComponent<SpriteRenderer>();
        this.starMaterial = sRenderer.material;
        this.camTransform = cam.transform;
    }

    private void Update() {
        // Modulate brightness (creates a subtle star flicker effect
        float brightnessScale = starMaterial.GetFloat(_ptr_brightness_scale);
        starMaterial.SetFloat(_ptr_brightness_scale, brightnessScale + 0.0001f % 10f);

        if (camTransform.hasChanged) {
            ComputeParallax();
            ComputeSize();
        }
    }

    private void ComputeParallax() {
        // Here is where we actually offset the star material and create
        // the parallax effect
        Vector2 offset = starMaterial.GetVector(_ptr_star_offset);
        offset.x = this.transform.position.x / this.transform.localScale.x / parallaxStrength;
        offset.y = this.transform.position.y / this.transform.localScale.y / parallaxStrength;
        offset = reverseParallaxDirection ? offset : -offset;
        starMaterial.SetVector(_ptr_star_offset, offset);
    }

    private void ComputeSize() {
        // Scale this background to always fill the camera
        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;
        this.transform.localScale = new Vector3(worldWidth, worldHeight, 1f);
    }
}

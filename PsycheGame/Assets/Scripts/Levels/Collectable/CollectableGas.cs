using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class CollectableGas : Spawnable, ScannableObject {
    private ParticleSystem ps;
    private Material psMaterial;
    private Material psSharedMaterial;
    private List<ParticleSystem.Particle> particles = new();
    private bool collectStart = false;
    private MissionState missionState;


    [SerializeField] protected Color gasColor = Color.white;

    private void Awake() {
        ps = this.GetComponent<ParticleSystem>();

        psMaterial = ps.GetComponent<ParticleSystemRenderer>().material;
        psSharedMaterial = ps.GetComponent<ParticleSystemRenderer>().sharedMaterial;

        psMaterial.SetColor("_EmissionColor", gasColor); 
        psSharedMaterial.SetColor("_EmissionColor", gasColor); 
        ps.trigger.AddCollider(GameObject.Find(ShipManager._SHIP_GAMEOBJECT_NAME).transform);
    }

    private void OnValidate() {
        ps = this.GetComponent<ParticleSystem>();
        psSharedMaterial = ps.GetComponent<ParticleSystemRenderer>().sharedMaterial;
        psSharedMaterial.SetColor("_EmissionColor", gasColor); 
    }

    private void FixedUpdate() {
        if (ps.particleCount <= 0) {
            Destroy(this.gameObject);
            OnEndCollect();
        }
    }

    // Triggered when a gas particle collides with the probe
    private void OnParticleTrigger() {
        if (!collectStart) {
            OnStartCollect();
            collectStart = true;
        }

        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles); 
        for (int i = 0; i < triggeredParticles; i++) {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            particles[i] = p;

            #pragma warning disable CS0618 // Type or member is obsolete
                ps.maxParticles -= 5;
            #pragma warning restore CS0618 // Type or member is obsolete
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        if (missionState != null)
        {
            missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectResource, triggeredParticles);
        }
        this.OnCollect(triggeredParticles);
    }

/*--Inherited Functionality--------------------------------------------------------------------------*/

    // Fade from the current color to the specified [endColor] using a linear interpolation over the
    // given [duration] where duration is a divisor, so 0.1 will take longer that 1
    protected Coroutine FadeColors(Color endColor, float duration) {
        Color start = psMaterial.color;
        IEnumerator changeCorutine = ColorChangeCoroutine(start, endColor, duration);
        return StartCoroutine(changeCorutine);
    }

    private IEnumerator ColorChangeCoroutine(Color start, Color end, float duration) {
        float tick = 0f;
        while (psMaterial.color != end) {
            tick += Time.deltaTime * duration;
            psMaterial.SetColor("_EmissionColor", Color.Lerp(start, end, tick));
            yield return null;
        }
    }

/*--Abstract Interface-------------------------------------------------------------------------------*/

    // Called to bridge the gap between monobehavior and scannable object
    public GameObject GameObject => this.gameObject;

    // Called when the player picks up the first particle of this collectable
    // gas
    public abstract void OnStartCollect();

    // Called when the player has collected every single particle of this
    // collectable gas and the gas is now being despawned
    public abstract void OnEndCollect();

    // Called on update passing in the number of particles collected in that
    // 'game tick'/update
    public abstract void OnCollect(int particlesCollected);

    // Called when this collectable gas is scanned by the player probe
    public abstract void Scan();

    // Called by the probe to poll weather or not this gas clound should be
    // scanned again if the probe is still scanning this gas cloud
    public abstract Progress ScanProgress { get; }

    // Called by UI systems to display a description about this scannable gas
    public abstract string Description { get; }

    // Called by UI systems to display the image of this scannable gas
    public abstract Sprite Image { get; }
}

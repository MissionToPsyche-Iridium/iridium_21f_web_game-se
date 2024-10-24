using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class CollectableGas : MonoBehaviour, ScannableObject {
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> particles = new();
    private bool collectStart = false;

    [SerializeField] protected Color gasColor = Color.white;

    private void Awake() {
        ps = this.GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(GameObject.Find(ShipManager._SHIP_GAMEOBJECT_NAME).transform);
    }

    private void OnValidate() {
        ps = this.GetComponent<ParticleSystem>();
        ps.GetComponent<ParticleSystemRenderer>().sharedMaterial.SetColor("_EmissionColor", gasColor);
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
        this.OnCollect(triggeredParticles);
    }

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
    public abstract bool IsScanned { get; }

    // Called by UI systems to display a description about this scannable gas
    public abstract string Description { get; }

    // Called by UI systems to display the image of this scannable gas
    public abstract Sprite Image { get; }
}

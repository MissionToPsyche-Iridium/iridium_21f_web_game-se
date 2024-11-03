using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class CollectableGas : MonoBehaviour {
    private ParticleSystem ps;
    private List<ParticleSystem.Particle> particles = new();

    private void Start() {
        ps = this.GetComponent<ParticleSystem>(); 
    }

    private void FixedUpdate() {
        if (ps.particleCount <= 0) {
            Destroy(this.gameObject);
        }
    }

    // Triggered when a gas particle collides with the probe
    private void OnParticleTrigger() {
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

    public abstract void OnCollect(int particlesCollected);

}

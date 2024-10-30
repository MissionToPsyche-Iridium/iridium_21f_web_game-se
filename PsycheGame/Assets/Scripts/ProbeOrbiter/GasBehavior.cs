using UnityEngine;

public class GasBehavior : MonoBehaviour {
    private ParticleSystem ps;
    private Material psMaterial;

    private void Awake() {
        ps = this.GetComponent<ParticleSystem>();
        psMaterial = ps.GetComponent<ParticleSystemRenderer>().sharedMaterial;
    }
}

using UnityEngine;

public class HeilumGas : CollectableGas {
    private bool scanned = false;
    [SerializeField] private Sprite displayImage;

    public override bool IsScanned => scanned;
    public override string Description => "Heilum gas which emptys the probes fuel tank";
    public override Sprite Image => displayImage;

    public override void OnCollect(int particlesCollected) {
        ShipManager.Fuel -= particlesCollected / 2; // scale this so fuel does not go
                                                    // down so severly
    }

    public override void OnStartCollect() {
        this.FadeColors(Color.red, 0.1f);
    }

    public override void Scan() {
        Debug.Log("Scanning Heilum collectable gas with id: " + gameObject.GetInstanceID());
        scanned = true;
    }

    public override void OnEndCollect() { }
}

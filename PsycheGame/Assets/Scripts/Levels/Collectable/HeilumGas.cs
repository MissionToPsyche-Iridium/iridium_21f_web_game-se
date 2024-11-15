using UnityEngine;

public class HeilumGas : CollectableGas {
    [SerializeField] private float progressIncr = 0.5f; // ammount of progress to gain each scan
    [SerializeField] private Sprite displayImage;

    private Progress scanProgress = new(Progress.NO_PROGRESS);

    public override string Description => "Heilum gas which emptys the probes fuel tank";
    public override Sprite Image => displayImage;
    public override Progress ScanProgress => scanProgress;

    public override void OnCollect(int particlesCollected) {
        ShipManager.Fuel -= particlesCollected / 2; // scale this so fuel does not go
                                                    // down so severly
    }

    public override void OnStartCollect() {
        this.FadeColors(Color.red, 0.1f);
    }

    public override void Scan() {
        scanProgress = scanProgress.incr(progressIncr * Time.deltaTime);
        Debug.Log("[" + this.GetInstanceID() + "] Scanned Heilum with progress: " + scanProgress.Value + "/100");
    }

    public override void OnEndCollect() { }
}

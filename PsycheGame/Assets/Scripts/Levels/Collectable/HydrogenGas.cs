using UnityEngine;

public class HydrogenGas : CollectableGas {
    [SerializeField] private float progressIncr = 0.5f; // ammount of progress to gain each scan
    [SerializeField] private Sprite displayImage;

    private Progress scanProgress = new(Progress.NO_PROGRESS);

    public override void OnStartCollect() { /* on first collect logic here */ }
    public override void OnEndCollect() { /* on end collect logic here */ }

    public override void OnCollect(int particlesCollected) {
        ShipManager.Fuel += particlesCollected;
    }

    // For new just as a demo we print the instance id of the scanned gas
    // and then set 'scanned' to true telling the probe that this object no
    // longer needs to be scanned
    // 
    // in the future we implement scanning specific logic to this gas type
    // here
    public override void Scan() {
        scanProgress = scanProgress.incr(progressIncr);
        Debug.Log("[" + this.GetInstanceID() + "] Scanned Hydrogen gas with progress: " + scanProgress.Value + "/100");
    }

    public override string Description => "Hydrogen gas used to refill a ships fuel tank";
    public override Sprite Image => displayImage;
    public override Progress ScanProgress => scanProgress;
}

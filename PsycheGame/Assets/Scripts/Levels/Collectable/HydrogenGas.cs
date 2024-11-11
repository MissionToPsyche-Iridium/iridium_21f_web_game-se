using UnityEngine;

public class HydrogenGas : CollectableGas {
    [SerializeField] private Sprite displayImage;

    private bool scanned = false;

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
        Debug.Log("Scanning Hydrogen collectable gas with id: " + gameObject.GetInstanceID());
        scanned = true;
    }

    public override bool IsScanned => scanned;
    public override string Description => "Hydrogen gas used to refill a ships fuel tank";
    public override Sprite Image => displayImage;
}

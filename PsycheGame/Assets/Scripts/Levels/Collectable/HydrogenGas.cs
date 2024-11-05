using UnityEngine;

public class HydrogenGas : CollectableGas {
    public override void OnStartCollect() {
    }

    public override void OnEndCollect() { 
    }

    public override void OnCollect(int particlesCollected) {
        ShipManager.Fuel += particlesCollected;
    }
}

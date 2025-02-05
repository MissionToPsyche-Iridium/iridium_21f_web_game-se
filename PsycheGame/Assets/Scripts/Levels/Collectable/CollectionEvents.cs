using System;
public static class CollectionEvents {
    public static event Action<int> OnMetalCollected;
    public static event Action<int> OnGasCollected;

    public static void MetalCollected(int amount) {
        OnMetalCollected?.Invoke(amount);
    }

    public static void GasCollected(int amount) {
        OnGasCollected?.Invoke(amount);
    }
}
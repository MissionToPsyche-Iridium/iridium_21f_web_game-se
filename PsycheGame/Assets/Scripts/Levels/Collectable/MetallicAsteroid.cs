using UnityEngine;
public class MetallicAsteroid : MineralCollection {
    protected override string GenerateMineralName(int index) {
        return $"MetallicMineral{index + 1}";
    }

    protected override Color GenerateMineralColor(int index) {
        return Color.gray;
    }

    private Progress scanProgress = new(Progress.NO_PROGRESS);
}
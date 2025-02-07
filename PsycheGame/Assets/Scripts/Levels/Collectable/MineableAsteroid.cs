using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MineableAsteroid :  MineralCollection {
    private void Start() {
        DisplayAsteroidComposition();
    }

    private void DisplayAsteroidComposition() {
        Debug.Log($"Asteroid Composition: {string.Join(", ", metals.ConvertAll(m => $"{m.Name} ({m.Amount})"))}");
    }
}

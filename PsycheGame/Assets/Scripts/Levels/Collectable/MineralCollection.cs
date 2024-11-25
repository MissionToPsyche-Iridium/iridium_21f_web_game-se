using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MineralCollection : MonoBehaviour, ScannableObject {
    [System.Serializable]
    public class Mineral {
        public string Name;
        public Color Color;
        public int Amount;
    }

    [SerializeField] private List<Mineral> minerals = new();
    [SerializeField] private int maxMineralTypes = 3;
    [SerializeField] private int maxTotalAmount = 100;
    [SerializeField] private float drillRate = 5f;
    [SerializeField] private ParticleSystem fragmentParticles;

    private bool isDrilling = false;
    private MissionState missionState;

    private void Awake() {
        GenerateMinerals();
    }

    private void Update() {
        if (isDrilling) {
            Drill();
        }

        if (IsDepleted()) {
            OnAsteroidDepleted();
        }
    }

    private void GenerateMinerals() {
        minerals.Clear();
        int mineralCount = Random.Range(1, maxMineralTypes + 1);

        for (int i = 0; i < mineralCount; i++) {
            Mineral mineral = new() {
                Name = "Mineral" + (i + 1),
                Color = Random.ColorHSV(),
                Amount = Random.Range(10, maxTotalAmount / mineralCount + 1)
            };
            minerals.Add(mineral);
        }
    }

    private void Drill() {
        foreach (Mineral mineral in minerals) {
            if (mineral.Amount > 0) {
                int minedAmount = Mathf.FloorToInt(drillRate * Time.deltaTime);
                mineral.Amount -= minedAmount;

                SpawnFragments(mineral.Color, minedAmount);
                UpdateMissionProgress(minedAmount, mineral.Name);

                break; 
            }
        }
    }

    private void SpawnFragments(Color fragmentColor, int amount) {
        if (fragmentParticles != null) {
            var main = fragmentParticles.main;
            main.startColor = fragmentColor;

            fragmentParticles.Emit(amount);
        }
    }

    private void UpdateMissionProgress(int minedAmount, string mineralName) {
        if (missionState != null) {
            missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectResource, minedAmount);
        }
        Debug.Log($"Collected {minedAmount} of {mineralName}");
    }

    private bool IsDepleted() {
        foreach (Mineral mineral in minerals) {
            if (mineral.Amount > 0) {
                return false;
            }
        }
        return true;
    }

    private void OnAsteroidDepleted() {
        Debug.Log("Asteroid fully mined!");
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ship")) {
            isDrilling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ship")) {
            isDrilling = false;
        }
    }

    public GameObject GameObject => this.gameObject;

    public Progress ScanProgress { get; }

    public string Description { get; }

    public  Sprite Image { get; }

    public void Scan() {
        Debug.Log($"Scanning asteroid. Composition: {string.Join(", ", minerals.ConvertAll(m => m.Name))}");
    }
}

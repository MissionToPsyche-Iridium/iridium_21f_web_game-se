using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RareMetal {
    public string Name { get; protected set; }
    public Color Color { get; protected set; }
    public int Amount { get; set; }
    public string Description { get; protected set; }

    protected RareMetal(string name, Color color, string description, int amount) {
        Name = name;
        Color = color;
        Description = description;
        Amount = amount;
    }
}

public class Titanium : RareMetal {
    public Titanium(int amount) : base(
        "Titanium",
        Color.gray,
        "A strong and lightweight metal.",
        amount
    ) { }
}

public class Palladium : RareMetal {
    public Palladium(int amount) : base(
        "Palladium",
        Color.cyan,
        "A rare and precious metal used in electronics.",
        amount
    ) { }
}

public class Iridium : RareMetal {
    public Iridium(int amount) : base(
        "Iridium",
        Color.yellow,
        "One of the densest and most corrosion-resistant metals.",
        amount
    ) { }
}

[RequireComponent(typeof(Collider2D))]
public abstract class MineralCollection : MonoBehaviour, ScannableObject {
    [Header("Metal Properties")]
    [SerializeField] protected List<RareMetal> metals = new();
    [SerializeField] protected int maxMetalTypes = 3;
    [SerializeField] protected int maxTotalAmount = 100;
    [SerializeField] protected int minMetalTypes = 1;
    [SerializeField] protected int minTotalAmount = 100;
    [SerializeField] protected float drillRate = 5f;
    [Header("Visual & Progession")]
    [SerializeField] private ParticleSystem fragmentParticles;
    [SerializeField] private Progress scanProgress;
    [SerializeField] private string description;
    [SerializeField] private Sprite image;

    public Progress ScanProgress => scanProgress;
    public string Description => description;
    public Sprite Image => image;

    private bool isDrilling = false;
    private MissionState missionState;

    private void Awake() {
        GenerateMetals();
    }

    private void Update() {
        if (isDrilling) {
            Drill();
        }

        if (IsDepleted()) {
            OnAsteroidDepleted();
        }
    }

    private void GenerateMetals() {
        metals.Clear();
        int metalCount = Random.Range(1, maxMetalTypes + 1);
        List<System.Func<int, RareMetal>> metalGenerators = new() {
            amount => new Titanium(amount),
            amount => new Palladium(amount),
            amount => new Iridium(amount)
        };

        for (int i = 0; i < metalCount; i++) {
            int randomIndex = Random.Range(0, metalGenerators.Count);
            RareMetal metal = metalGenerators[randomIndex](
                Random.Range(10, maxTotalAmount / metalCount + 1)
            );
            metals.Add(metal);
            metalGenerators.RemoveAt(randomIndex);
        }
    }

    private void Drill() {
        foreach (RareMetal metal in metals) {
            if (metal.Amount > 0) {
                int minedAmount = Mathf.FloorToInt(drillRate * Time.deltaTime);
                metal.Amount -= minedAmount;

                SpawnFragments(metal.Color, minedAmount);
                UpdateMissionProgress(minedAmount, metal.Name);

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

    private void UpdateMissionProgress(int minedAmount, string metalName) {
        if (missionState != null) {
            missionState.UpdateObjectiveProgress(MissionState.ObjectiveType.CollectResource, minedAmount);
        }
        Debug.Log($"Collected {minedAmount} of {metalName}");
    }

    private bool IsDepleted() {
        foreach (RareMetal metal in metals) {
            if (metal.Amount > 0) {
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
        if (other.CompareTag("Player")) {
            isDrilling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isDrilling = false;
        }
    }

    public GameObject GameObject => this.gameObject;

    public void Scan() {
        scanProgress.incr(1);
        Debug.Log($"Scanning asteroid. Composition: {string.Join(", ", metals.ConvertAll(m => $"{m.Name} ({m.Amount})"))}");
    }
}
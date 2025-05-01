using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

[TestFixture]
public class CollectableGasTests
{
    private GameObject testObject;
    private ParticleSystem particleSystem;
    private HeilumGas heilumGas;
    private HydrogenGas hydrogenGas;

    [SetUp]
    public void Setup()
    {
        testObject = new GameObject("TestGas");
        particleSystem = testObject.AddComponent<ParticleSystem>();
        var renderer = testObject.GetComponent<ParticleSystemRenderer>();
        if (renderer.sharedMaterial == null)
        {
            renderer.sharedMaterial = new Material(Shader.Find("Particles/Standard Unlit"));
        }
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(testObject);
    }

    [Test]
    public void HeilumGas_InitializesWithCorrectDescription()
    {
        heilumGas = testObject.AddComponent<HeilumGas>();
        Assert.AreEqual("Heilum gas which emptys the probes fuel tank", heilumGas.Description);
    }

    [Test]
    public void HydrogenGas_InitializesWithCorrectDescription()
    {
        hydrogenGas = testObject.AddComponent<HydrogenGas>();
        Assert.AreEqual("Hydrogen gas used to refill a ships fuel tank", hydrogenGas.Description);
    }

    [Test]
    public void HeilumGas_OnCollect_ReducesFuel()
    {
        heilumGas = testObject.AddComponent<HeilumGas>();
        float initialFuel = 100f;
        ShipManager.Fuel = initialFuel;
        
        heilumGas.OnCollect(10);
        Assert.AreEqual(initialFuel - 5f, ShipManager.Fuel);
    }

    [Test]
    public void HydrogenGas_OnCollect_IncreasesFuel()
    {
        hydrogenGas = testObject.AddComponent<HydrogenGas>();
        float initialFuel = 100f;
        ShipManager.Fuel = initialFuel;
        
        hydrogenGas.OnCollect(10);
        Assert.AreEqual(initialFuel + 10f, ShipManager.Fuel);
    }

    [Test]
    public void ScanProgress_InitializesWithNoProgress()
    {
        heilumGas = testObject.AddComponent<HeilumGas>();
        Assert.AreEqual(0f, heilumGas.ScanProgress.Value);
    }

    private class TestGas : CollectableGas
    {
        public bool startCollectCalled = false;
        public bool endCollectCalled = false;
        public int collectedParticles = 0;

        public override void OnStartCollect() => startCollectCalled = true;
        public override void OnEndCollect() => endCollectCalled = true;
        public override void OnCollect(int particlesCollected) => collectedParticles = particlesCollected;
        public override void Scan() { }
        public override Progress ScanProgress => new Progress(0f);
        public override string Description => "Test Gas";
        public override Sprite Image => null;

        public void SetGasColor(Color color)
        {
            System.Reflection.FieldInfo field = typeof(CollectableGas).GetField("gasColor", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            field.SetValue(this, color);
        }

        public void InvokeAwake()
        {
            System.Reflection.MethodInfo awakeMethod = typeof(CollectableGas).GetMethod("Awake", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            awakeMethod.Invoke(this, null);
        }
    }
}
public static class CollectionEvents
{
    public static void GasCollected(int amount) { }
}
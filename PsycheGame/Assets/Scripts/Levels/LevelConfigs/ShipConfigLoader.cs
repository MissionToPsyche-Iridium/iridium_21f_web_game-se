using System.IO;
using UnityEngine;

public class ShipConfigLoader : MonoBehaviour {
    private static readonly string DATA_FILE_NAME = "ContainerGameData.json";
    private static readonly string DATA_PATH = Application.dataPath + Path.AltDirectorySeparatorChar + DATA_FILE_NAME;

    private class ProbeComponentList {
        public ProbeComponent[] components;
    }

    private ProbeComponentList LoadBuilderSaveData()
    {
        if (!File.Exists(DATA_PATH)) {
            Debug.LogError("ERROR: builder '.json' save data not found using default 'editor' variables for ship config");
            return null;
        }

        string fileText = File.ReadAllText(DATA_PATH);
        return JsonUtility.FromJson<ProbeComponentList>(fileText);
    }

    private void DebugPrintProbeComponent(ProbeComponent comp)
    {
        Debug.Log(
            "Probe Component:\n" +
            "---------------------\n" +
            $"ID: {comp.Id}\n" +
            $"Name: {comp.Name}\n" +
            $"Description: {comp.Description}\n" +
            $"Type: {comp.Type}\n" +
            $"Scanning Range: {comp.ScanningRange}\n" +
            $"Fuel Capacity: {comp.FuelCapacity}\n" +
            $"Speed: {comp.Speed}\n" +
            $"Armor: {comp.Armor}\n" +
            $"HP: {comp.Hp}\n" +
            $"Weight: {comp.Weight}\n" +
            $"Credits: {comp.Credits:F2}\n" +
            $"Grid Position: ({comp.GridPositionX}, {comp.GridPositionY})\n" +
            "---------------------\n"
        );
    }

    private void Awake()
    {
        ProbeComponentList probeComponents = LoadBuilderSaveData();
        foreach (ProbeComponent probeComponent in probeComponents.components)
        {
            DebugPrintProbeComponent(probeComponent);
        }
    }

    private void Start()
    {

    }

    void Update() {
        
    }
}

using System.IO;
using UnityEngine;

public class ShipConfigLoader : MonoBehaviour {
    public static readonly string DATA_FILE_NAME = "ContainerGameData.json";
    public static readonly string DATA_PATH = Application.dataPath + Path.AltDirectorySeparatorChar + DATA_FILE_NAME;

    [SerializeField] private ShipConfig defaultShipConfig;

    private class ProbeComponentList {
        // NOTE: due to how JSON is deserialized the public variable name below
        // should match that used when calling "FromJson"
        public ProbeComponent[] components;
    }

    private static ProbeComponentList LoadBuilderSaveData(string path)
    {
        if (!File.Exists(path)) {
            Debug.LogError("ERROR: builder '.json' save data not found using default 'editor' variables for ship config");
            return null;
        }

        string fileText = File.ReadAllText(path);
        return JsonUtility.FromJson<ProbeComponentList>("{\"components\":" + fileText + "}");
    }

    private static void DebugPrintProbeComponent(ProbeComponent comp)
    {
        Debug.Log(
            "Probe Component Found:\n" +
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
            $"Grid Position: ({comp.GridPositionX}, {comp.GridPositionY})\n"
        );
    }

    public static ShipConfig LoadBuilderConfig(string path, ShipConfig defaultShipConfig)
    {
        ShipConfig config = new ShipConfig();
        ProbeComponentList probeComponents = LoadBuilderSaveData(path);

        if (probeComponents == null) {
            return defaultShipConfig;
        }

        int totalScanRange = 0,
            totalFuelCapcity = 0,
            totalSpeed = 0,
            totalArmor = 0,
            totalHp = 0,
            totalWeight = 0;

        foreach (ProbeComponent probeComponent in probeComponents.components)
        {
            DebugPrintProbeComponent(probeComponent);
            totalScanRange += probeComponent.ScanningRange;
            totalFuelCapcity += probeComponent.FuelCapacity;
            totalSpeed += probeComponent.Speed;
            totalArmor += probeComponent.Armor;
            totalHp += probeComponent.Hp;
            totalWeight += probeComponent.Weight;
        }

        Debug.Log(
            "Ship config initialized with:\n"   +
            "  Scan Range:   " + totalScanRange   + "\n" +
            "  Fuel Capcity: " + totalFuelCapcity + "\n" +
            "  Speed:        " + totalSpeed       + "\n" +
            "  Armor:        " + totalArmor       + "\n" +
            "  Health:       " + totalHp          + "\n" +
            "  Weight:       " + totalWeight
        );

        // @note - here we are only loading the config with a couple of
        // the actual computed variables. Team should meet to discuss further
        // how we want to computes these
        config.shipMoveConfig.health = totalHp;
        config.shipMoveConfig.fuel = totalFuelCapcity;
        config.scanConfig.distance = totalScanRange;

        return config;
    }
}

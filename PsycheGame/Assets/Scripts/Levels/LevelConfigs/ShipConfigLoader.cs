using System.Collections;
using System.IO;
using UnityEngine;

public class ShipConfigLoader : MonoBehaviour {
    public static readonly string DATA_FILE_NAME = "ContainerGameData.json";
    public static readonly string DATA_PATH = Application.dataPath + Path.AltDirectorySeparatorChar + DATA_FILE_NAME;

    // multiplier for fuel capacity since values transfered from the builder side of the game
    // are typically lower than expected, only applied if incoming fuel value is lower than
    // fuel capacity min
    public static readonly int FUEL_CAPACITY_MULT = 5;
    public static readonly int FUEL_CAPACITY_MIN = 50;

    private class ProbeComponentList {
        // NOTE: due to how JSON is deserialized the public variable name below
        // should match that used when calling "FromJson"
        public ProbeComponent[] components;
    }

    private ProbeComponentList LoadBuilderSaveData(string path)
    {
        if (!File.Exists(path)) {
            Debug.LogError("ERROR: builder '.json' save data not found using default 'editor' variables for ship config");
            return null;
        }

        string fileText = File.ReadAllText(path);
        return JsonUtility.FromJson<ProbeComponentList>("{\"components\":" + fileText + "}");
    }

    private IEnumerator PopupUiAddComponents(ProbeComponentList comps)
    {
        ScannedColumn popupUi = (ScannedColumn)FindObjectOfType(typeof(ScannedColumn));
        popupUi.AddEntry(null, "Probe Components:", "", popupUi.GetHashCode());
        yield return new WaitForSeconds(0.5f);

        foreach (ProbeComponent comp in comps.components)
        {
            var header = comp.Name;
            var description = comp.Description;
            var id = comp.GetHashCode();
            var sprite = BuilderSpriteManager.GetComponentSprite(comp.Id);

            popupUi.AddEntry(sprite, header, description, id);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public ShipConfig LoadBuilderConfig(string path, ShipConfig defaultShipConfig)
    {
        ShipConfig config = ScriptableObject.CreateInstance<ShipConfig>();
        ProbeComponentList probeComponents = LoadBuilderSaveData(path);

        if (probeComponents == null) {
            return defaultShipConfig;
        }

        int totalScanRange = 0,
            totalFuelCapcity = 0,
            totalSpeed = 0,
            totalHealth = 0,
            totalWeight = 0;

        foreach (ProbeComponent probeComponent in probeComponents.components)
        {
            totalScanRange += probeComponent.ScanningRange;
            totalFuelCapcity += probeComponent.FuelCapacity;
            totalSpeed += probeComponent.Speed;
            totalHealth += probeComponent.Health;
            totalWeight += probeComponent.Weight;
        }

        if (totalFuelCapcity <= FUEL_CAPACITY_MIN)
        {
            totalFuelCapcity *= FUEL_CAPACITY_MULT;
        }

        Debug.Log(
            "Ship config initialized with:\n"   +
            "  Scan Range:   " + totalScanRange   + "\n" +
            "  Fuel Capcity: " + totalFuelCapcity + "\n" +
            "  Speed:        " + totalSpeed       + "\n" +
            "  Health:       " + totalHealth      + "\n" +
            "  Weight:       " + totalWeight
        );

        // @note - here we are only loading the config with a couple of
        // the actual computed variables. Team should meet to discuss further
        // how we want to computes these
        config.shipMoveConfig.health = totalHealth;
        config.shipMoveConfig.fuel = totalFuelCapcity;

        config.scanConfig.distance = totalScanRange;
        StartCoroutine(PopupUiAddComponents(probeComponents));
        return config;
    }
}

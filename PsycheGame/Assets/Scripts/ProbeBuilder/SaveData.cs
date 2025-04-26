using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public List<GameObject> SpawnedParts { get; set; }
    private static readonly string FilePath = Path.Combine(Application.dataPath, "ContainerGameData.json");

    public static string WriteToFile(List<GameObject> spawnedParts)
    {
        if (spawnedParts == null || spawnedParts.Count == 0)
        {
            Debug.LogWarning("No spawned parts to save.");
            return string.Empty;
        }

        BuildManager buildManager = GameObject.Find("/MasterCanvas")?.GetComponent<BuildManager>();
        if (buildManager == null)
        {
            Debug.LogError("BuildManager not found. Ensure it is attached to the MasterCanvas GameObject.");
            return string.Empty;
        }

        List<string> serializedComponents = new List<string>();
        foreach (GameObject part in spawnedParts)
        {
            ProbeComponent component = buildManager.GetProbeComponentInfo(part);
            if (component != null)
            {
                string json = JsonUtilityWrapper.ToJson(component);
                serializedComponents.Add(json);
            }
            else
            {
                Debug.LogWarning($"ProbeComponent info not found for GameObject: {part.name}");
            }
        }

        string content = $"[{string.Join(",", serializedComponents)}]";
        File.WriteAllText(FilePath, content);

        Debug.Log($"Saved {serializedComponents.Count} parts to file: {FilePath}");
        return content;
    }
}

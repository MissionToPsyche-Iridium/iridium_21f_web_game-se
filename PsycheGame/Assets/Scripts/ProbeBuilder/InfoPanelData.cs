using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InfoPanelData : MonoBehaviour
{
    [SerializeField] private TextAsset data; // JSON file containing parts data
    private Dictionary<string, string> partDescriptions; // Optimized for quick lookups

    private static InfoPanelData _instance;

    public static InfoPanelData Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("InfoPanelData instance is not initialized. Ensure the script is attached to a GameObject in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Multiple instances of InfoPanelData detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        _instance = this;
        InitializePartDescriptions();
    }

    private void InitializePartDescriptions()
    {
        partDescriptions = new Dictionary<string, string>();

        if (data == null)
        {
            Debug.LogError("Data file is not assigned in InfoPanelData.");
            return;
        }

        try
        {
            PartsList partsList = JsonUtility.FromJson<PartsList>(data.text);
            if (partsList != null && partsList.part != null)
            {
                foreach (Part part in partsList.part)
                {
                    if (!partDescriptions.ContainsKey(part.name))
                    {
                        partDescriptions.Add(part.name, part.description);
                    }
                    else
                    {
                        Debug.LogWarning($"Duplicate part name detected: {part.name}. Ignoring duplicate.");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to parse parts data. Ensure the JSON file is correctly formatted.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing parts data: {ex.Message}");
        }
    }

    public string GetDescription(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return "Invalid part name.";
        }

        if (partDescriptions.TryGetValue(name, out string description))
        {
            return description;
        }

        return "No description available.";
    }

    [Serializable]
    public class Part
    {
        public string name;
        public string description;
    }

    [Serializable]
    public class PartsList
    {
        public Part[] part;
    }
}
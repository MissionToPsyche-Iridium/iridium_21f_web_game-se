using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
    AttributeTracker.cs
    Description: this script is responsible for tracking the attributes of the probe components.  

    revision 1.3 (Mar 5)
    :: updated the attribute panel to display a bar for each attribute the represent the relative value to the max value.  Included
    gradient color to represent the value of the attribute.

    revision 1.4 (Mar 17)
    :: updated the code base using the new ProbeAttributeTotals class to calculate the attribute totals. 
*/


public class AttributeTracker : MonoBehaviour
{
    private Dictionary<string, int> attributes = new Dictionary<string, int>();
    private ProbeAttributeTotals attributeTotals;
    private BuildManager buildManager;
    private ContainerManager containerManager;

    private Dictionary<string, int> maxValues;
    private Color attributeColor;

    private void LoadMaxValues()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "maxValues.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            maxValues = JsonUtility.FromJson<Dictionary<string, int>>(json);
        }
        else
        {
            Debug.LogError("++AT++ maxValues.json file not found. Using Default.");
            maxValues = new Dictionary<string, int>
            {
                { "Hp", 177 },
                { "Armor", 177 },
                { "FuelCapacity", 68 },
                { "Speed", 29 },
                { "ScanningRange", 59 }
            };
        }
    }

    public void UpdateChildAttributes()
    {
        attributeTotals = buildManager.CalculateAttributeTotals();
        var transforms = gameObject.GetComponentsInChildren<Transform>();
        var gradientColor = attributeColor;
        attributeTotals = buildManager.CalculateAttributeTotals();

        foreach (var t in transforms)
        {
            var textComponent = t.GetComponent<TextMeshProUGUI>();
            var imageComponent = t.GetComponent<Image>();
            var rectTransform = t.GetComponent<RectTransform>();

            switch (t.name)
            {
                case "HealthVal":
                    textComponent.text = (attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Hp) + attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Armor)).ToString();
                    break;
                case "FuelVal":
                    textComponent.text = attributeTotals.GetAttributeTotal(ProbeComponentAttribute.FuelCapacity).ToString();
                    break;
                case "ThrusterVal":
                    textComponent.text = attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Speed).ToString();
                    break;
                case "ScannerVal":
                    textComponent.text = attributeTotals.GetAttributeTotal(ProbeComponentAttribute.ScanningRange).ToString();
                    break;
                case "Credits":
                    textComponent.text = buildManager.GetAvailableCredits().ToString();
                    break;
                case "HealthFill":
                    UpdateFill(imageComponent, rectTransform, attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Hp) + attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Armor), maxValues["Hp"] + maxValues["Armor"]);
                    break;
                case "FuelFill":
                    UpdateFill(imageComponent, rectTransform, attributeTotals.GetAttributeTotal(ProbeComponentAttribute.FuelCapacity), maxValues["FuelCapacity"]);
                    break;
                case "ThrusterFill":
                    UpdateFill(imageComponent, rectTransform, attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Speed), maxValues["Speed"]);
                    break;
                case "ScanFill":
                    UpdateFill(imageComponent, rectTransform, attributeTotals.GetAttributeTotal(ProbeComponentAttribute.ScanningRange), maxValues["ScanningRange"]);
                    break;
                default:
                    break;
            }
        }
    }

    // helper: determine gradient vector scalar and fill the attribute bar based on set color
    private void UpdateFill(Image imageComponent, RectTransform rectTransform, int attributeValue, int maxValue)
    {
        var gradientColor = attributeColor;
        gradientColor.a = (float)attributeValue / maxValue;
        imageComponent.color = gradientColor;
        rectTransform.localScale = new Vector3((float)attributeValue / maxValue, 1, 1);
    }

    void Start()
    {
        LoadMaxValues();      // load the max values for the attributes (maxValues.json)
        buildManager = GameObject.Find("MasterCanvas").GetComponent<BuildManager>();
        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        attributeColor = containerManager.GetAttribBarColor();
        UpdateChildAttributes();
    }

    void Update()
    {
        if (attributeColor == null || containerManager.GetAttribBarColor() != attributeColor)
        {
            attributeColor = containerManager.GetAttribBarColor();
        }
        UpdateChildAttributes();
    }
}
using System;
using System.Collections.Generic;
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

    revision 1.5 (Apr 7)
    :: replace the lazy switch statement with a dictionary to map the attribute names to their respective functions for cleaner code.
*/


public class AttributeTracker : MonoBehaviour
{
    private ProbeAttributeTotals attributeTotals;
    private BuildManager buildManager;
    private ContainerManager containerManager;
    private Color attributeColor;
    private Material originalMaterial;
    private Material specialMaterial;

    public void UpdateChildAttributes()
    {
        // calc attribute totals once and set text values
        attributeTotals = buildManager.CalculateAttributeTotals();
        var transforms = gameObject.GetComponentsInChildren<Transform>();
        var textMappings = new Dictionary<string, Func<int>>
        {
            { "HealthVal", () => attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Health) },
            { "FuelVal", () => attributeTotals.GetAttributeTotal(ProbeComponentAttribute.FuelCapacity) },
            { "ThrusterVal", () => attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Speed) },
            { "ScannerVal", () => attributeTotals.GetAttributeTotal(ProbeComponentAttribute.ScanningRange) },
            { "Credits", () => (int)buildManager.GetAvailableCredits() }
        };

        // define mappings for fill updates
        var fillMappings = new Dictionary<string, Func<(int value, int maxValue)>>
        {
            { "HealthFill", () => (
                attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Health),
                buildManager.GetAttributeMaxValue(ProbeComponentAttribute.Health)
            ) },
            { "FuelFill", () => (
                attributeTotals.GetAttributeTotal(ProbeComponentAttribute.FuelCapacity),
                buildManager.GetAttributeMaxValue(ProbeComponentAttribute.FuelCapacity)
            ) },
            { "ThrusterFill", () => (
                attributeTotals.GetAttributeTotal(ProbeComponentAttribute.Speed),
                buildManager.GetAttributeMaxValue(ProbeComponentAttribute.Speed)
            ) },
            { "ScanFill", () => (
                attributeTotals.GetAttributeTotal(ProbeComponentAttribute.ScanningRange),
                buildManager.GetAttributeMaxValue(ProbeComponentAttribute.ScanningRange)
            ) }
        };

        // iterate through transforms and update GUI elements
        foreach (var t in transforms)
        {
            if (textMappings.TryGetValue(t.name, out var textFunc))
            {
                var textComponent = t.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = textFunc().ToString();
                }
            }

            if (fillMappings.TryGetValue(t.name, out var fillFunc))
            {
                var imageComponent = t.GetComponent<Image>();
                var rectTransform = t.GetComponent<RectTransform>();
                if (imageComponent != null && rectTransform != null)
                {
                    (int value, int maxValue) = fillFunc();
                    UpdateFill(imageComponent, rectTransform, value, maxValue);
                }
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
        
        // if the localScale value is > 0.9, set the material to the special material
        if (imageComponent.material != null && imageComponent.material.name == originalMaterial.name)
        {
            if (rectTransform.localScale.x > 0.9f)
            {
                imageComponent.material = specialMaterial;
            }
            else
            {
                imageComponent.material = originalMaterial;
            }
        }
        else if (imageComponent.material != null && imageComponent.material.name == specialMaterial.name)
        {
            if (rectTransform.localScale.x <= 0.9f)
            {
                imageComponent.material = originalMaterial;
            }
        }
    }

    void Start()
    {
        buildManager = GameObject.Find("MasterCanvas").GetComponent<BuildManager>();
        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        attributeColor = containerManager.GetAttribBarColor();

        var rectTransform = gameObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            var imageComponent = rectTransform.GetComponent<Image>();
            if (imageComponent != null)
            {
                originalMaterial = imageComponent.material;
                imageComponent.material = new Material(originalMaterial);
                Debug.Log($"Material set to {imageComponent.material.name}");
            }
        }

        specialMaterial = Resources.Load<Material>("EFX/SparkMaterial3");

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
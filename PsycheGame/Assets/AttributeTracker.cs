using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

/*
    AttributeTracker.cs
    Description: this script is responsible for tracking the attributes of the probe components.  
    
    version: 1.0 

    This script updates the child components' attributes based on the current probe component's attributes using the call to the
    UpdateChildAttributes() method.

    v.1.1 - updated the UpdateChildAttributes() method to update the child components' attributes based on the current probe component's attributes. 
    :: additionally, the fill bar visual indicators are updated based on the current probe component's attributes.

    v.1.2 - incorporating the different color schemes for the attribute bars -- standard and alternate (colorblind friendly). gradient color is 
    based on the strength of the attribute relative to the maximum value of the attribute.
*/

public class AttributeTracker : MonoBehaviour
{

    private Dictionary<string, int> attributes = new Dictionary<string, int>();
    private BuildManager buildManager;
    private ContainerManager containerManager;

    const int MAX_SCANNING_RANGE = 59;
    const int MAX_FUEL_CAPACITY = 68;
    const int MAX_SPEED = 29;
    const int MAX_HEALTH = 177;

    private int scanningRange = 0;
    private int fuelCapacity = 0;
    private int speed = 0;
    private int health = 0;
    private int creditAvailable = 0;

    private Color attributeColor;

    public void UpdateChildAttributes()
    {
        attributes = buildManager.CalculateAttributeTotals();
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
        Color gradientColor =  attributeColor;
        foreach (Transform t in ts)
        {
            switch (t.name)
            {
                case "HealthVal":
                    t.GetComponent<TextMeshProUGUI>().text = (attributes["Hp"] + attributes["Armor"]).ToString();
                    break;
                case "FuelVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["FuelCapacity"].ToString();
                    break;
                case "ThrusterVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["Speed"].ToString();
                    break;
                case "ScannerVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["ScanningRange"].ToString();
                    break;
                case "Credits":
                    t.GetComponent<TextMeshProUGUI>().text = buildManager.GetAvailableCredits().ToString();
                    break;
                case "HealthFill":
                    gradientColor = attributeColor;
                    gradientColor.a = (float)(attributes["Hp"] + attributes["Armor"]) / MAX_HEALTH;
                    t.GetComponent<UnityEngine.UI.Image>().color = gradientColor;
                    t.GetComponent<RectTransform>().localScale = new Vector3((float)(attributes["Hp"] + attributes["Armor"]) / MAX_HEALTH, 1, 1);
                    break;
                case "FuelFill": 
                    gradientColor = attributeColor;
                    gradientColor.a = (float)attributes["FuelCapacity"] / MAX_FUEL_CAPACITY;
                    t.GetComponent<UnityEngine.UI.Image>().color = gradientColor;
                    t.GetComponent<RectTransform>().localScale = new Vector3((float)attributes["FuelCapacity"] / MAX_FUEL_CAPACITY, 1, 1);
                    break;
                case "ThrusterFill":
                    gradientColor = attributeColor;
                    gradientColor.a = (float)attributes["Speed"] / MAX_SPEED;
                    t.GetComponent<UnityEngine.UI.Image>().color = gradientColor;
                    t.GetComponent<RectTransform>().localScale = new Vector3((float)attributes["Speed"] / MAX_SPEED, 1, 1);
                    break;
                case "ScanFill":
                    gradientColor = attributeColor;
                    gradientColor.a = (float)attributes["ScanningRange"] / MAX_SCANNING_RANGE;
                    t.GetComponent<UnityEngine.UI.Image>().color = gradientColor;
                    t.GetComponent<RectTransform>().localScale = new Vector3((float)attributes["ScanningRange"] / MAX_SCANNING_RANGE, 1, 1);
                    break;
                default:
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildManager = GameObject.Find("MasterCanvas").GetComponent<BuildManager>();
        containerManager = GameObject.Find("ContainerPanel").GetComponent<ContainerManager>();
        attributeColor = containerManager.GetAttribBarColor();

        // Debug.Log(" <AT> +++Fetch Probe component attributes+++ ");
        UpdateChildAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        if (attributeColor == null) 
        {
            attributeColor = containerManager.GetAttribBarColor();
        }
        if (containerManager.GetAttribBarColor() != attributeColor)
        {
            attributeColor = containerManager.GetAttribBarColor();
        }
        //Debug.Log(" <AT2> +++Updateing Probe component attributes+++");
        UpdateChildAttributes();
    }
}
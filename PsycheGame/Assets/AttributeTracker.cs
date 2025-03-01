using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
    AttributeTracker.cs
    Description: this script is responsible for tracking the attributes of the probe components.  
    
    version: 1.0 

    This script updates the child components' attributes based on the current probe component's attributes using the call to the
    UpdateChildAttributes() method.
*/

public class AttributeTracker : MonoBehaviour
{

    private Dictionary<string, int> attributes = new Dictionary<string, int>();
    private BuildManager buildManager;

    const int MAX_SCANNING_RANGE = 59;
    const int MAX_FUEL_CAPACITY = 68;
    const int MAX_SPEED = 29;
    const int MAX_HEALTH = 177;

    private int scanningRange = 0;
    private int fuelCapacity = 0;
    private int speed = 0;
    private int health = 0;
    private int creditAvailable = 0;

    private void Awake()
    {
    }

    public void UpdateChildAttributes()
    {
        attributes = buildManager.CalculateAttributeTotals();
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
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
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        buildManager = GameObject.Find("MasterCanvas").GetComponent<BuildManager>();
        // Debug.Log(" <AT> +++Fetch Probe component attributes+++ ");
        UpdateChildAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(" <AT2> +++Updateing Probe component attributes+++");
        UpdateChildAttributes();
    }
}

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

    public void UpdateChildAttributes()
    {
        attributes = buildManager.CalculateAttributeTotals();
        Transform[] ts = gameObject.transform.GetComponentsInChildren<Transform>();
        foreach (Transform t in ts)
        {
            switch (t.name)
            {
                case "ScanRangeVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["ScanningRange"].ToString();
                    break;
                case "FuelCapacityVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["FuelCapacity"].ToString();
                    break;
                case "SpeedVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["Speed"].ToString();
                    break;
                case "ArmorVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["Armor"].ToString();
                    break;
                case "HpVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["Hp"].ToString();
                    break;
                case "WeightVal":
                    t.GetComponent<TextMeshProUGUI>().text = attributes["Weight"].ToString();
                    break;
                case "AvailableCredits":
                    t.GetComponent<TextMeshProUGUI>().text = buildManager.GetAvailableCredits().ToString();
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        buildManager = GameObject.Find("MasterCanvas").GetComponent<BuildManager>();
        Debug.Log(" <AT> +++Fetch Probe component attributes+++ ");
        UpdateChildAttributes();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(" <AT2> +++Updateing Probe component attributes+++");

        UpdateChildAttributes();
    }
}

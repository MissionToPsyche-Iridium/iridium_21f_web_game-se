using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;
    private int index;
    private int maxIndex;
    private static string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json";
    private GameObject uiDesignObject;
    
    void Start()
    {
        Debug.Log("Loading list of designs");
        designs = ContainerGameData.Instance.getDesigns();
        Debug.Log("Loading design at index 0");
        index = 0;
        maxIndex = designs.Count;
        loadDesign(index);
    }

    public void Start(List<ProbeDesign> test_designs) {
        Debug.Log("Loading list of designs");
        designs = test_designs;
        Debug.Log("Loading design at index 0");
        index = 0;
        maxIndex = designs.Count;
        uiDesignObject = Instantiate (Resources.Load("UI/Design") as GameObject);
        loadDesign(index);
    }

    public void loadDesign(int index) {
        if(maxIndex > 0){
        Debug.Log("Loading design at index: " + index);
        var design = designs[index];
        if(uiDesignObject == null) {
            uiDesignObject = Instantiate(GameObject.Find("Design")) as GameObject;
        }
        GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = design.sprite;
        GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = design.name;
        GameObject.Find("HealthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Health: " + design.totals.Health;
        GameObject.Find("ThrusterStrengthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Thruster Strength: " + design.totals.Speed;
        GameObject.Find("ScanningRangeText").GetComponentInChildren<TMPro.TMP_Text>().text = "Scanning Range: " + design.totals.ScanningRange.ToString();
        GameObject.Find("FuelCapacityText").GetComponentInChildren<TMPro.TMP_Text>().text = "Fuel Capacity: " + design.totals.FuelCapacity.ToString();
        //GameObject.Find("DesignParts").GetComponentInChildren<TMPro.TMP_Text>().text = "Parts: " + design.partsJson;
        } else {
            loadEmpty();
        }
    }

    public void loadEmpty() {
        GameObject uiDesignObject = Instantiate(GameObject.Find("Design")) as GameObject;
        GameObject.Find("DesignImage").GetComponentInChildren<Image>().enabled = false;
        GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = "No Saved Designs";
        GameObject.Find("HealthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Health: ";
        GameObject.Find("ThrusterStrengthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Thruster Strength: ";
        GameObject.Find("ScanningRangeText").GetComponentInChildren<TMPro.TMP_Text>().text = "Scanning Range: ";
        GameObject.Find("FuelCapacityText").GetComponentInChildren<TMPro.TMP_Text>().text = "Fuel Capacity: ";

    }

    public void nextDesign() {
        Debug.Log("Getting next design");
        if(index < maxIndex-1 && maxIndex != 0) {
            index++;
            loadDesign(index);
        }
    }

    public void backDesign() {
        Debug.Log("Getting previous design");
        if(index > 0) {
            index--;
            loadDesign(index);
        }
    }

    public void deleteShipDesign() {
        if(maxIndex > 1) {
            Debug.Log("Deleting design at index: " + index);
            ContainerGameData.Instance.deleteDesign(index);
            designs = ContainerGameData.Instance.getDesigns();
            index = 0;
            maxIndex = designs.Count;
            loadDesign(index);
        }
        else {
            Debug.Log("Design list is empty.");
            loadEmpty();
        }
        
    }

    public String selectShipDesign() {
        Debug.Log("Selected Design " + index + " and saved it to file.");
        File.WriteAllText(filePath, designs[index].partsJson); 
        return designs[index].partsJson;
    }


}

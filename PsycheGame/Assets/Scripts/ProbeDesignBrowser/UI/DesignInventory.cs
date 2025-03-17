using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;
    private int index;
    private int maxIndex;

    void Start()
    {
        Debug.Log("Loading list of designs");
        designs = ContainerGameData.Instance.getDesigns();
        Debug.Log("Loading design at index 0");
        index = 0;
        maxIndex = designs.Count;
        loadDesign(index);
    }

    public void loadDesign(int index) {
        Debug.Log("Loading design at index: " + index);
        var design = designs[index];
        GameObject uiDesignObject = Instantiate(GameObject.Find("Design")) as GameObject;
        GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = design.sprite;
        GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = design.name;
        GameObject.Find("HealthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Health: " + design.totals.Hp;
        GameObject.Find("ThrusterStrengthText").GetComponentInChildren<TMPro.TMP_Text>().text = "Thruster Strength: " + design.totals.Speed;
        GameObject.Find("ScanningRangeText").GetComponentInChildren<TMPro.TMP_Text>().text = "Scanning Range: " + design.totals.ScanningRange.ToString();
        GameObject.Find("FuelCapacityText").GetComponentInChildren<TMPro.TMP_Text>().text = "Fuel Capacity: " + design.totals.FuelCapacity.ToString();
        //GameObject.Find("DesignParts").GetComponentInChildren<TMPro.TMP_Text>().text = "Parts: " + design.partsJson;

    }

    public void nextDesign() {
        Debug.Log("Getting next design");
        if(index < maxIndex) {
            index++;
        }
        loadDesign(index);

    }

    public void backDesign() {
        Debug.Log("Getting previous design");
        if(index > 0) {
            index--;
        }
        loadDesign(index);
    }


}

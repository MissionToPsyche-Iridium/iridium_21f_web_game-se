using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;

    //private List<ProbeDesign> testDesigns;
    
    //public List<Sprite> testSprites;

    // Start is called before the first frame update
    void Start()
    {
        //createTestDesigns();
        designs = ContainerGameData.Instance.getDesigns();
        Debug.Log("designs count: " + designs.Count);
        foreach(ProbeDesign design in designs) {
            
            Debug.Log("design name: " + design.name);
            Debug.Log("design parts: " + design.partsJson);
            Debug.Log("totals: " + design.attributeTotalsJson);
            
            // Dictionary<string, string> totals = JsonParser.FromJson<Dictionary<string, string>>(design.attributeTotalsJson);
            // GameObject uiDesignObject = Instantiate(GameObject.Find("Design")) as GameObject;
            // uiDesignObject.transform.SetParent(GameObject.Find("Viewport").transform);
            // GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = design.name;
            // GameObject.Find("ScanningRangeText").GetComponentInChildren<TMPro.TMP_Text>().text = totals["ScanningRange"];
            //GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = testSprite.name;
            //GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = testSprite; 
        }

        
    }


    // private void createTestDesigns() {
    //     testDesigns = new List<ProbeDesign>();
    //     int num = 1;
    //     foreach(Sprite sprite in testSprites) {
    //         ProbeDesign pd = new ProbeDesign(sprite, "Ship " + num, "", new List<GameObject>());
    //         testDesigns.Add(pd);
    //     }


    // }
}

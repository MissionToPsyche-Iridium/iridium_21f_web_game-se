using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;

    private List<ProbeDesign> defaultDesigns;
    
    public List<Sprite> defaultSprites;

    // Start is called before the first frame update
    void Start()
    {
        createDefaultDesigns();
        addDefaultDesigns();
        
    }

    void OnEnable()
    {
        if(getPlayerDesigns()) {
            addPlayerDesigns();
        }
    }

    private bool getPlayerDesigns(){
        designs = ContainerGameData.Instance.getDesigns();
        if(designs.Count == 0) {
            return false;
        }return true;
    }

    private void addPlayerDesigns() {
        foreach(ProbeDesign design in designs) {
            if(GameObject.Find(design.designName) == null){
                GameObject uiDesignObject = Instantiate(GameObject.Find("Design"));
                uiDesignObject.transform.SetParent(GameObject.Find("DesignContent").transform);
                uiDesignObject.GetComponentInChildren<TMPro.TMP_Text>().text = design.designName;
                uiDesignObject.GetComponentInChildren<Image>().sprite = design.designSprite;
            }
        }
    }


    private void createDefaultDesigns() {
        defaultDesigns = new List<ProbeDesign>();
        int count = 1;
        foreach(Sprite sprite in defaultSprites) {
            // 12/5 default designs do not have jsons or parts lists
            ProbeDesign pd = new ProbeDesign(sprite, "Ship " + count, "", new List<GameObject>());
            defaultDesigns.Add(pd);
            count++;
        }

 }
 private void addDefaultDesigns() {
    foreach(ProbeDesign design in defaultDesigns) {
            GameObject uiDesignObject = Instantiate(GameObject.Find("Design"));
            uiDesignObject.transform.SetParent(GameObject.Find("DesignContent").transform);
            uiDesignObject.GetComponentInChildren<TMPro.TMP_Text>().text = design.designName;
            uiDesignObject.GetComponentInChildren<Image>().sprite = design.designSprite;
            }
            //GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = testSprite.name;
            //GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = testSprite;   
 }
}

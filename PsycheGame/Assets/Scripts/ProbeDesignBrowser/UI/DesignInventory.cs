using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;

    private List<ProbeDesign> testDesigns;
    
    public List<Sprite> testSprites;

    // Start is called before the first frame update
    void Start()
    {
        createTestDesigns();

        foreach(ProbeDesign design in designs) {
            GameObject uiDesignObject = Instantiate(GameObject.Find("Design")) as GameObject;
            uiDesignObject.transform.SetParent(GameObject.Find("DesignContent").transform);
            uiDesignObject.GetComponentInChildren<TMPro.TMP_Text>().text = design.name;
            uiDesignObject.GetComponentInChildren<Image>().sprite = design.sprite;

        }

        //GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = testSprite.name;
        //GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = testSprite; 
        
    }

    // Update is called once per frame
    void Update()
    {
        designs = ContainerGameData.Instance.getDesigns();
    }

    private void createTestDesigns() {
        testDesigns = new List<ProbeDesign>();
        int num = 1;
        foreach(Sprite sprite in testSprites) {
            ProbeDesign pd = new ProbeDesign(sprite, "Ship " + num, "", new List<GameObject>());
            testDesigns.Add(pd);
        }


    }
}

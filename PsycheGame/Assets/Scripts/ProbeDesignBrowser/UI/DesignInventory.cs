using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DesignInventory : MonoBehaviour
{
    public List<ProbeDesign> designs;
    
    public Sprite testSprite;

    // Start is called before the first frame update
    void Start()
    {
        //foreach(Sprite design in designs) {
        //GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = design.name;
        //GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = design.sprite;
        //}

        GameObject.Find("DesignName").GetComponentInChildren<TMPro.TMP_Text>().text = testSprite.name;
        GameObject.Find("DesignImage").GetComponentInChildren<Image>().sprite = testSprite; 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

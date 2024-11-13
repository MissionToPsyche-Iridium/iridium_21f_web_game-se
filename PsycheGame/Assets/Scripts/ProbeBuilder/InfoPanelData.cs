using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoPanelData: MonoBehaviour
{
    public TextAsset data;
    public PartsList parts = new PartsList();
    public Sprite[] spritesArr;
    
    //public GameObject currentObject; //TODO get most recent sprite that has been clicked

    [System.Serializable]
    public class Part {
        public string name;
        public string description;
       
    }

    [System.Serializable]
    public class PartsList {
        public Part[] part;
    }

    [System.Serializable] 
    public class Images{
        public Sprite[] sprites;
    }
    public void Start() {
        parts = JsonUtility.FromJson<PartsList>(data.text);
        
        GameObject.Find("PartName").GetComponentInChildren<TMP_Text>().text = parts.part[0].name;;
        GameObject.Find("PartDescription").GetComponentInChildren<TMP_Text>().text = parts.part[0].description;
        GameObject.Find("PartImage").GetComponentInChildren<UnityEngine.UI.Image>().sprite = spritesArr[0];
    }

    // void Update () {

    //     if (Input.GetMouseButtonDown(0) )
    //     {
           
    //          currentObject = UnityEngine.EventSystems.EventSystem.current.gameObject;
    //         //currentObject.GetComponent<Sprite>();
    //         Debug.Log("current sprite " + currentObject.name);   
    //     }

    // }

}

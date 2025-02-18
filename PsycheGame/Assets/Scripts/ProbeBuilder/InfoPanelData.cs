using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class InfoPanelData: MonoBehaviour
{
    public TextAsset data;
    public static PartsList jsonPartList = new PartsList();
     
    private static InfoPanelData _instance;

    void Start() {
        jsonPartList = JsonUtility.FromJson<PartsList>(data.text);
    }

    [System.Serializable]
    public class Part {
        public string name;
        public string description;
       
    }

    [System.Serializable]
    public class PartsList {
        public Part[] part;
    }

    public static String getDescription(String name) {
        foreach(Part p in jsonPartList.part) {
                if(String.Equals(p.name, name)) {
                    return p.description;
                }
        }return "No description available.";
    }

}
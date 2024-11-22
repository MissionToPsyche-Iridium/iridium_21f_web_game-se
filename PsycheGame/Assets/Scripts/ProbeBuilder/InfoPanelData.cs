using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

[Serializable]
public class InfoPanelData: MonoBehaviour
{
    private static TextAsset data;
    public static PartsList jsonPartList = new PartsList();
     
    private static InfoPanelData _instance;
    
    public static InfoPanelData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new InfoPanelData();
            data = (TextAsset) Resources.Load("InfoPanelJson.json");
            jsonPartList = JsonUtility.FromJson<PartsList>(data.text);

        }
        return _instance;
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

    public String getDescription(String name) {
        foreach(Part p in jsonPartList.part) {
                if(String.Equals(p.name, name)) {
                    return p.description;
                }
        }return "No description available.";
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoPanelData: MonoBehaviour
{
    public TextAsset data;
    public PartsList parts = new PartsList();
    //public SerializeField Image[] images;
    Canvas parent;

    [System.Serializable]
    public class Part {
        public string name;
        public string description;
       
    }

    [System.Serializable]
    public class PartsList {
        public Part[] part;
         //Image image;
    }

    public void Start() {
        parts = JsonUtility.FromJson<PartsList>(data.text);
        GetComponentInChildren<TMP_Text>().text = parts.part[0].name;

    }

    

}

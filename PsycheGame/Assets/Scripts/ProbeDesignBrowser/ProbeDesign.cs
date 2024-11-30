using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ProbeDesign : MonoBehaviour
{
    public Sprite sprite; //flattened image of design
    public String name; //name of design
    private String json; //contains the probe parts attached (names and locations) these are saved in the container game data class

    private List<GameObject> parts;
    
    //private List<ProbeComponent> parts;

    public ProbeDesign(Sprite sprite, String name, String json, List<GameObject> parts){
        this.sprite = sprite;
        this.name = name;
        this.json = json;
        this.parts = parts;
    }
   
}

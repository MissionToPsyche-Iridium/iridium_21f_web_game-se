using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ProbeDesign
{
    public Sprite sprite; //flattened image of design
    public String name; //name of design
    public String partsJson; //contains the probe parts attached (names and locations) these are saved in the container game data class
    public String attributeTotalsJson;
    public List<GameObject> parts;
    
    public ProbeDesign(Sprite sprite, String name, String partsJson, List<GameObject> parts, String totalsJson){
        this.sprite = sprite;
        this.name = name;
        this.partsJson = partsJson;
        this.parts = parts;
        this.attributeTotalsJson = totalsJson;
    }
   
}

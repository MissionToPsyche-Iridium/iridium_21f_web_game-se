using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public sealed class ContainerGameData
{
    private static ContainerGameData instance = null;
    private static readonly object padlock = new object();
    private List<Tile> tiles = new List<Tile>();
    private List<GameObject> spawnedParts;
    private List<ProbeDesign> probeDesigns = new List<ProbeDesign>();  
    private ContainerGameData() {}

    public static ContainerGameData Instance {
        get {
            lock(padlock) {
                if(instance == null) {
                    instance = new ContainerGameData();
                }return instance;
            }
        }
    }

    //TODO Add tiles as they are spawned in ContainerManager
    public void addTile(Tile tile) {
        tiles.Add(tile);
    }

    public void removeTile(Tile tile) {
        tiles.Remove(tile);
    }
    public List<Tile> getTiles() {
        return tiles;
    }

    //TODO Add parts as they are spawned in BuildManager(?) or appropriate file
    public void addPart(GameObject part) {
        spawnedParts.Add(part);
    }

    //TODO Remove parts when the user selects the undo button in BuildManager(?) or appropriate file
    public void removePart(GameObject part) {
        spawnedParts.Remove(part);
    }
    
    public List<GameObject> getParts() {
        return spawnedParts;
    }
    
    //TODO Remove all parts when the user selects the undo all button in BuildManager(?) or appropriate file
    public void removeAllParts() {
        spawnedParts.Clear();
    } 

    public bool saveProbeDesign(string name) {
        if(probeDesigns.Count < 10) {
        Sprite sprite = (new Snapshot(GameObject.Find("/MasterCanvas/SpawnArea").GetComponent<Canvas>())).Take();
        List<GameObject> parts = GameObject.Find("/MasterCanvas").GetComponent<BuildManager>().GetSpawnedProbeComponents(); //get current spawned parts
        string partsJson = SaveData.WriteToFile(parts); //saves design's parts to json
        string partsNames = GetPartsNames(parts);
        ProbeAttributeTotals totals = GameObject.Find("/MasterCanvas").GetComponent<BuildManager>().CalculateAttributeTotals(); //saves attribute totals
        ProbeDesign design = new ProbeDesign(sprite, name, partsJson, parts, totals, partsNames);
        probeDesigns.Add(design); //Adds current design to list of designs
        return true;
        } else {
            Debug.Log("Cannot save more than 10 designs.");
            return false;
        }
    }

    public List<ProbeDesign> getDesigns() {
        return probeDesigns;
    }

    public void deleteDesign(int index) {
        probeDesigns.RemoveAt(index);
    }

    public string GetPartsNames(List<GameObject> parts) {
        string names = "";
        foreach(GameObject part in parts) {
            names += "[" + part.name + "] ";
        }
        return names;
    }

}
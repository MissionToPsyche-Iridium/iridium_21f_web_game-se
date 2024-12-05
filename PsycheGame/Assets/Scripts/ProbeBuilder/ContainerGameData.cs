using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.SceneManagement;

public sealed class ContainerGameData
{
    private static ContainerGameData instance = null;
    private static readonly object padlock = new object();
    private List<Tile> tiles = new List<Tile>();
    private List<GameObject> spawnedParts; //TODO choose part type (GameObject, ProbeComponent, something else?)
    private static List<ProbeDesign> probeDesigns = new List<ProbeDesign>(); //TODO send to DesignInventory class    
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

    public void saveProbeDesign() {
        String name = "Design" + probeDesigns.Count;
        Screenshot.takeScreenshot(name);
        Sprite sprite = LoadNewSprite("Assets/Resources/Screenshots/"+name+".png"); 
        List<GameObject> parts = BuildManager.GetInstance().GetSpawnedProbeComponents(); //get current spawned parts
        String json = SaveData.WriteToFile(parts); //saves design's parts to json
        ProbeDesign design = new ProbeDesign(sprite, name, json, parts);
        probeDesigns.Add(design); //Adds current design to list of designs
    }

    public List<ProbeDesign> getDesigns() {
        return probeDesigns;
    }

     public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {      
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);
 
        return NewSprite;
    }
    public Texture2D LoadTexture(string FilePath)
    {
 
        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails
 
        Texture2D Tex2D;
        byte[] FileData;
 
        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}
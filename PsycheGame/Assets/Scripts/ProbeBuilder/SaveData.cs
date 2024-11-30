using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    //TODO 11/12 this class has not been tested yet as it depends on ContainerGameData being connected to the ContainerManager and the spawned parts
    public List<GameObject> spawnedParts;
    public System.String ToJson(List<GameObject> spawnedParts) {
        string json = "";
        
        foreach (var part in spawnedParts) {
            json += JsonUtility.ToJson(part);
        }
        
         //Debug.Log(json);

         using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json")) {
            writer.Write(json);
         }
         return json;
    }

    public static List<GameObject> LoadFromJson(string a_Json)
    {
        return JsonUtility.FromJson<List<GameObject>>(a_Json);
    }
    
}

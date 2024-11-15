using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //TODO 11/12 this class has not been tested yet as it depends on ContainerGameData being connected to the ContainerManager and the spawned parts
    public List<GameObject> spawnedParts;
    public void ToJson(List<GameObject> spawnedParts) {
        string json = "";
        
        foreach (var part in spawnedParts) {
            json += JsonUtility.ToJson(part);
        }
        
         //Debug.Log(json);

         using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json")) {
            writer.Write(json);
         }
    }

    public static List<GameObject> LoadFromJson(string a_Json)
    {
        return JsonUtility.FromJson<List<GameObject>>(a_Json);
    }
    
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public List<GameObject> spawnedParts;
    public static string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json"; // Path to save the text file

    public static string WriteToFile(List<GameObject> spawnedParts)
    {
        string content = ""; 
        foreach (GameObject part in spawnedParts) {
            content += part.name + "\n"; // Add each GameObject name to the string
        }

        File.WriteAllText(filePath, content); // Write the content to the text file

        //Debug.Log("content: " + content);
        return content;

    }

    

    // public static List<GameObject> LoadFromJson(string a_Json)
    // {
    //     return JsonUtility.FromJson<List<GameObject>>(a_Json);
    // }

       // public static string ToJson(List<GameObject> spawnedParts) {
    //     string json = "";
        
    //     foreach (var part in spawnedParts) {
    //         json += JsonUtility.ToJson(part);
    //     }

    //      using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json")) {
    //         writer.Write(json);
    //      }
    //      return json;
    // }
    
}

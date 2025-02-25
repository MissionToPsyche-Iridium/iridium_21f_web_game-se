using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData
{
    public List<GameObject> spawnedParts;
    public static string filePath = Application.dataPath + Path.AltDirectorySeparatorChar + "ContainerGameData.json"; // Path to save the text file

    public static string WriteToFile(List<GameObject> spawnedParts)
    {
        int count = 0;
        string content = "["; 
        foreach (GameObject part in spawnedParts) {
            ProbeComponent component = GameObject.Find("/MasterCanvas").GetComponent<BuildManager>().GetProbeComponentInfo(part);
            //content += component.Name + "\n"; // Add each GameObject name to the string
            content += JsonParser.ToJson(component);
            count++;
            if(count != spawnedParts.Count) {
                content += ",";
            }
        }
        content += "]";
        File.WriteAllText(filePath, content); // Write the content to the text file

        //Debug.Log("content: " + content);
        return content;

    }
    
}

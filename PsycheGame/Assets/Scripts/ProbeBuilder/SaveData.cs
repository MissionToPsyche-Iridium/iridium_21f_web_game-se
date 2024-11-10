using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    /* TODO: make this class work with current inventory setup
    public InventoryContainer inventory;
    public void ToJson(InventoryContainer inventory) {
        //ProbeComponent part = new ProbeComponent("testPart", "description", 1234);
        string json = "";
        foreach (var part in inventory._container) {
            json += JsonUtility.ToJson(part);
        }
         
         Debug.Log(json);
         using(StreamWriter writer = new StreamWriter(Application.dataPath + Path.AltDirectorySeparatorChar + "InventoryData.json")) {
            writer.Write(json);
         }
    }

    public static List<ProbeComponent> LoadFromJson(string a_Json)
    {
        return JsonUtility.FromJson<List<ProbeComponent>>(a_Json);
    }
    */
}

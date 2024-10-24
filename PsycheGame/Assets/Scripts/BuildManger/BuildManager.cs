using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

using Unity.VisualScripting;
using System.Collections;

public class BuildManager : MonoBehaviour
{
    public GameObject buttonPrefab;   
    public Transform buttonContainer;

    public GameObject[] availableShapes; 
    public Sprite[] partImages;

    private ContainerManager containerManager;

    public GameObject spawnPoint;

    public Stack spawnedPartsStack;
    [SerializeField] private int probePartScale;


    public void Start(){
        CreateInventoryButtons();
        // get container manager component attached to the BuildManager GameObject
        ContainerManager containerManager = GameObject.Find("ContainerManager").GetComponent<ContainerManager>();

        spawnPoint = GameObject.Find("SpawnArea");
        spawnedPartsStack = new Stack();
        Debug.Log($"Build Manager Initialized");
    }


    void CreateInventoryButtons()
    {
        for (int i = 0; i < availableShapes.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            Debug.Log(availableShapes.Length);
            button.GetComponentInChildren<TextMeshProUGUI>().text = availableShapes[i].name;
            int index = i;
            Image probePartImage = button.transform.Find("ProbePartImage").GetComponent<Image>();
            probePartImage.sprite = partImages[i];
            button.GetComponent<Button>().onClick.AddListener(() => SelectShape(availableShapes[index]));
        }
        Debug.Log($"Created {availableShapes.Length +1} buttons");
    }
    public void SelectShape(GameObject shape)
    {
        Debug.Log($"Selected shape: {shape.name}");
        SpawnShape(shape);
    }

    void SpawnShape(GameObject shapePrefab)
    {
        // foreach (Transform child in shapeSpawnArea)
        // {
        //     Destroy(child.gameObject);
        // }

        GameObject shape = Instantiate(shapePrefab, spawnPoint.transform);
        shape.transform.localPosition = new Vector3(500, 200, 0);
        //shape.transform.localScale = Vector3.one;
        shape.transform.localScale = new Vector3(probePartScale, probePartScale, 0);
        shape.tag = "Part"; //used for UndoAllOperation()
        spawnedPartsStack.Push(shape); //used for UndoOperation()

        Debug.Log($"Spawned shape: {shape.name} at position {shape.transform.localPosition}");
    }

    public void ExitProbeBuilder(){
        Debug.Log("Returning home!");
        SaveProbe(); //saves before exiting
        SceneManager.LoadScene("MainMenu"); //returns user to main menu
    }

    public void SaveProbe(){
        Debug.Log("Saving probe!");
        //saves all currently spawned and snapped probe parts to a persistant game object
    }

   
    public void UndoAllOperation(){
        //Destroys all spawned probe parts (snapped or not)
        Debug.Log("UndoAll operation");
        GameObject[] parts = GameObject.FindGameObjectsWithTag("Part");
        foreach(GameObject part in parts) {
        GameObject.Destroy(part);
       }
    }

    public void UndoOperation(){
        //Destroys last spawned probe part 
        Debug.Log("Undo operation");
        //if the last user action was to spawn a part...
        GameObject lastSpawned = (GameObject) spawnedPartsStack.Pop();
        GameObject.Destroy(lastSpawned);
    }

     public void RedoOperation(){
        //Spawns a probe part that was just destroyed?
        Debug.Log("Redo operation");
    }
}
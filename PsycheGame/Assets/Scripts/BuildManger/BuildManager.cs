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
    public GameObject spawnArea;
    public Stack spawnedPartsStack;

    private RectTransform shapeSpawnArea;
    [SerializeField] private int probePartScale;

    public void Start(){
        CreateInventoryButtons();

        // initialize master canvas to RectTransform
        shapeSpawnArea = GameObject.Find("MasterCanvas").GetComponent<RectTransform>();

        // get container manager component attached to the BuildManager GameObject
        //ContainerManager containerManager = GameObject.Find("ContainerManager").GetComponent<ContainerManager>();
        spawnArea = GameObject.Find("ContainerPanel");
        spawnedPartsStack = new Stack();
        Debug.Log($"Build Manager Initialized");
    }


    void CreateInventoryButtons()
    {
        for (int i = 0; i < availableShapes.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            Debug.Log(availableShapes.Length);
            button.GetComponentInChildren<TextMeshProUGUI>().text = availableShapes[i].name.Substring(0, availableShapes[i].name.IndexOf("_"));
            button.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            button.GetComponentInChildren<TextMeshProUGUI>().fontSize = 14;

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

        // get the master canvas rect transform and locate a relative position to spawn the shape
        int spawnX = (int)(shapeSpawnArea.rect.width * 0.22);
        int spawnY = (int)(shapeSpawnArea.rect.height / 2 * 0.8);
        float spawnSize = shapeSpawnArea.rect.width / 400.0f;

        GameObject shape = Instantiate(shapePrefab, spawnArea.transform);
        shape.transform.localPosition = new Vector3(spawnX, spawnY, 0);
        shape.transform.localScale = new Vector3(spawnSize, spawnSize, 100);

        shape.AddComponent<Rigidbody2D>().gravityScale = 0;
        shape.AddComponent<BoxCollider2D>().isTrigger = true;
        shape.GetComponent<BoxCollider2D>().size = new Vector2(10,10);
        shape.AddComponent<SpriteDragDrop>(); //adds drag and drop features
        shape.layer = 9;            //sets layer to 8 - equivalent to "ProbePart"
        shape.tag = "ProbePart";    //used for UndoAllOperation()
        spawnedPartsStack.Push(shape); //used for UndoOperation()

        /* do not merge the following
        //(dated code) GameObject shape = Instantiate(shapePrefab, spawnPoint.transform);
        //shape.transform.localPosition = new Vector3(0, 0, 0);
        //shape.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Debug.Log($"Spawned shape: {shapePrefab.name} at position {shapePrefab.transform.localPosition}");
        */

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
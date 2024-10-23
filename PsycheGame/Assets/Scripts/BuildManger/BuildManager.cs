using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

using Unity.VisualScripting;

public class BuildManager : MonoBehaviour
{
    public GameObject buttonPrefab;   
    public Transform buttonContainer;

    public GameObject[] availableShapes; 
    public Sprite[] partImages;

    private ContainerManager containerManager;

    public GameObject spawnPoint;

    public void Start(){
        CreateInventoryButtons();
        createContainer();

        spawnPoint = GameObject.Find("SpawnArea");
        Debug.Log($"Build Manager Initialized");
    }

    void createContainer(){
        //get the tile prefabs from ProbeBuildingContainer folder
        Tile tilePrefab = Resources.Load<Tile>("ProbeBuildingContainer");
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
        shape.transform.localPosition = new Vector3(0, 0, 0); 
        shape.transform.localScale = Vector3.one;

        Debug.Log($"Spawned shape: {shape.name} at position {shape.transform.localPosition}");
    }

    public void ExitProbeBuilder(){
        Debug.Log("Returning home!");
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveProbe(){
        Debug.Log("Saving probe!");
    }

    public void UndoOperation(){
        Debug.Log("Undo operation");
    }

     public void RedoOperation(){
        Debug.Log("Redo operation");
    }
}
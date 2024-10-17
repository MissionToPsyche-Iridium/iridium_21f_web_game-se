using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildManager : MonoBehaviour
{
    public GameObject buttonPrefab;   
    public Transform buttonContainer;

    public GameObject[] availableShapes; 
    public Sprite[] partImages;
    private GameObject selectedShape;


    public GameObject spawnPoint;

    public void Start(){
        CreateInventoryButtons();
        spawnPoint = GameObject.Find("SpawnArea");
    }


    void CreateInventoryButtons()
    {
           for (int i = 0; i < availableShapes.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            
            button.GetComponentInChildren<TextMeshProUGUI>().text = availableShapes[i].name;

            Image buttonImage = button.transform.Find("Image").GetComponent<Image>();
            buttonImage.sprite = partImages[i]; 
            button.GetComponent<Button>().onClick.AddListener(() => SelectShape(availableShapes[i]));
        }
    }

    void OnItemClick(string itemName)
    {
        Debug.Log($"Item {itemName} clicked");
    }

    public void SelectShape(GameObject shape)
    {
        selectedShape = shape;
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
}
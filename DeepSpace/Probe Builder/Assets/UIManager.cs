using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject shapeButton; 
    public Transform buttonContainer;   
    public GameObject[] availableShapes;  

    private GameObject selectedShape;    
    public Transform spawnPoint;  
    private ShapeBuilder shapeBuilder;

    void Start()
    {
        Debug.Log("UIManager started!"); 
        CreateShapeButtons();
    }

    void CreateShapeButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Cleared existing buttons.");
        foreach (var shape in availableShapes)
        {
            GameObject button = Instantiate(shapeButton, buttonContainer);        
            var textMeshPro = button.transform.Find("Button Text").GetComponent<TextMeshProUGUI>();
            textMeshPro.text = shape.name;

            var buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectShape(shape));
            Debug.Log($"Button created for shape: {shape.name}");
        }
    }

    public void SelectShape(GameObject shape)
    {
        selectedShape = shape;
        Debug.Log($"Selected shape: {shape.name}");
    }

    void SpawnShape()
    {
        if (selectedShape != null)
        {
            Instantiate(selectedShape, spawnPoint.position, Quaternion.identity);
        }
    }

    public void CreateShape(GameObject shape)
    {
        if (shape != null)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            GameObject createdShape = shapeBuilder.CreateShape(shape, spawnPosition);
            Debug.Log($"Created shape: {shape.name} at position: {spawnPosition}");
        }
        else
        {
            Debug.LogError("Shape prefab is null!");
        }
    }

    public GameObject GetSelectedShape()
    {
        return selectedShape;
    }
}
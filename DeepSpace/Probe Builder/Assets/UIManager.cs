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

    void Start()
    {
        CreateShapeButtons();
    }

    void CreateShapeButtons()
    {
        foreach (var shape in availableShapes)
        {
        GameObject button = Instantiate(shapeButton, buttonContainer);
        Debug.Log($"Created button for shape: {shape.name}");
        
        var textMeshPro = button.transform.Find("Button Text").GetComponent<TextMeshProUGUI>();
        textMeshPro.text = shape.name;

        var buttonComponent = button.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => SelectShape(shape));
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


    public GameObject GetSelectedShape()
    {
        return selectedShape;
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject shapeButton; 
    public Transform buttonContainer;   
    public GameObject[] availableShapes;  

    private GameObject selectedShape;    

    void Start()
    {
        CreateShapeButtons();
    }

    void CreateShapeButtons()
    {
        foreach (var shape in availableShapes)
        {
            GameObject button = Instantiate(shapeButton, buttonContainer);
            button.GetComponentInChildren<Text>().text = shape.name;
            button.GetComponent<Button>().onClick.AddListener(() => SelectShape(shape));
        }
    }

    public void SelectShape(GameObject shape)
    {
        selectedShape = shape;
        Debug.Log($"Selected shape: {shape.name}");
    }

    public GameObject GetSelectedShape()
    {
        return selectedShape;
    }
}
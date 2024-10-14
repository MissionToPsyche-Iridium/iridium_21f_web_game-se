using UnityEngine;
using UnityEngine.UI;

public class ShapeButton : MonoBehaviour
{
    public GameObject shape; 

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.SelectShape(shape); 
    }
}
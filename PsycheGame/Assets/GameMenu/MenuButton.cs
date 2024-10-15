using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public GameObject button; 

    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.selectedButton(button); 
    }
}
using Unity
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject menuButton; 
    public Transform buttonContainer;   
    public GameObject[] menuButtons;  

    private GameObject selectedButton;    

    void Start()
    {
        Debug.Log("UIManager started!"); 
        CreateMenuButtons();
    }

    void CreateMenuButtons()
    {
        foreach (var menuButton in menuButtons)
        {
            GameObject button = Instantiate(menuButton, buttonContainer);        
            var textMeshPro = button.transform.Find("Button Text").GetComponent<TextMeshProUGUI>();
            textMeshPro.text = button.name;

            var buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectButton(button));
            Debug.Log($"Menu button created: {button.name}");
        }
    }

    public void SelectButton(GameObject shape)
    {
        selectedButton = button;
        Debug.Log($"Selected: {button.name}");
    }

    public GameObject GetSelectedButton()
    {
        return selectedButton;
    }
}
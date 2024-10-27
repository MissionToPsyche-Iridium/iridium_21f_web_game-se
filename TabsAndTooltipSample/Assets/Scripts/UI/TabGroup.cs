using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TabGroup : MonoBehaviour {

    private TabButton selectedTab; 
    private List<TabButton> buttons;

    [SerializeField] private List<GameObject> pageObjects;
    [SerializeField] private Sprite hoveredImage;
    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite unselectedImage;

    public void Subscribe(TabButton button) {
        if (buttons == null) buttons = new List<TabButton>();
        buttons.Add(button);
    }

    public void OnTabEnter(TabButton button) {
        ResetTabs();
        button.SetImageIcon(hoveredImage);
    }

    public void OnTabExit(TabButton button) {
        ResetTabs();
    }

    public void OnTabSelect(TabButton button) {
        selectedTab = button;
        ResetTabs();
        button.SetImageIcon(selectedImage);

        PopulatePage(button);
    }

    private void ResetTabs() {
        foreach (TabButton button in buttons) {
            if (button == selectedTab) {
                button.SetImageIcon(selectedImage);
            } else {
                button.SetImageIcon(unselectedImage);
            }
        }
    }

    private void PopulatePage(TabButton button) {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < pageObjects.Count; i++) {
            pageObjects[i].SetActive(i == index);
        }
    }

}

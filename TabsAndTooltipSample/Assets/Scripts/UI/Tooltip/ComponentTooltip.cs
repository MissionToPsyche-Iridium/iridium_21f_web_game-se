using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComponentTooltip : Tooltip {
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI componentName;
    [SerializeField] private TextMeshProUGUI componentInfo;
    [SerializeField] private Image componentImage;

    public void SetContent(string name, string info, Sprite image) {
        this.componentName.text = name;
        this.componentInfo.text = info;
        this.componentImage.sprite = image;
    }

    private void Update() {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 pivot = this.CalculatePivot(mousePosition);
        rectTransform.pivot = pivot;
        this.transform.position = mousePosition;
    }
}

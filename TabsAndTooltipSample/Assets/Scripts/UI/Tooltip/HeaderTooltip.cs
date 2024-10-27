using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class HeaderToolTip : Tooltip {
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement widthLimitingElement;
    [SerializeField] private int characterColumnLimit;

    public void SetText(string content, string header = "") {
        this.headerField.gameObject.SetActive(!string.IsNullOrEmpty(header));
        this.headerField.text = header;
        this.contentField.text = content;

        ToggleWidthLimiting();
    }

    // enable or disable the charater column limiting element, this must be
    // done otherwise text shorter than the column limit will render a tool
    // tip not properly sized to the text
    private void ToggleWidthLimiting() {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;
        widthLimitingElement.enabled = headerLength > characterColumnLimit || 
                                       contentLength > characterColumnLimit;
    }

    private void Update() {
        if (Application.isEditor) { // Enabled for preview in the editor
            ToggleWidthLimiting();
        }

        Vector2 mousePosition = Input.mousePosition;
        Vector2 pivot = this.CalculatePivot(mousePosition);
        rectTransform.pivot = pivot;
        this.transform.position = mousePosition;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannedItemPopup : MonoBehaviour {
    [SerializeField] private Image imageField;
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;

    public void SetContent(Sprite image, string title, string info) {
        this.imageField.sprite = image;
        this.headerField.text = title;
        this.contentField.text = info;
    }
}

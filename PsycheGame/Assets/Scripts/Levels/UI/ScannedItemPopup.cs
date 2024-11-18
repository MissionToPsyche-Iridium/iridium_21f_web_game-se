using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This is a stub class attached to a prefab object in the editor
// the prefab object polls this class for its values. Maybe unity
// has a better way to represent pure data classes like this?
public class ScannedItemPopup : MonoBehaviour {
    [Header("UI Elements")]
    [SerializeField] private Image imageField;
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;

    public void SetContent(Sprite image, string title, string info) {
        this.imageField.sprite = image;
        this.headerField.text = title;
        this.contentField.text = info;
    }
}

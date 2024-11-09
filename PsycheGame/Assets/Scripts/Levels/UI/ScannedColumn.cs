using UnityEngine;
using UnityEngine.UI;

public class ScannedColumn : MonoBehaviour {
    [SerializeField] private ScannedItemPopup popupPrefab = null;

    private RectTransform rectTransform;

    private void Awake() {
        this.rectTransform = this.GetComponent<RectTransform>(); 
    }

    public void AddEntry(Sprite image, string title, string info) {
        var popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity, this.transform);
        popup.SetContent(image, title, info);

        // Hack to prevent weird overlapping behavior of vertical
        // layout group when adding new entry
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}

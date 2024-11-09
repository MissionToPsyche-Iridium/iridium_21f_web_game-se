using UnityEngine;

public class ScannedColumn : MonoBehaviour {

    [SerializeField] private ScannedItemPopup popupPrefab = null;

    public void AddEntry(Sprite image, string title, string info) {
        var popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity, this.transform);
        popup.SetContent(image, title, info);
    }

}

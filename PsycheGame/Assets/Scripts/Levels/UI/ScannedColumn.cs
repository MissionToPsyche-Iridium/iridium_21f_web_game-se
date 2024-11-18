using System.Collections.Concurrent;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// This is the column/data structure which holds, displays, and manages
// popups for scanned items
//
// In the future this could be extended to support other kinds of popups
// in the same manor as scannable popups
public class ScannedColumn : MonoBehaviour {
    [SerializeField] private float popupLifetimeSeconds = 5f;
    [SerializeField] private ScannedItemPopup popupPrefab = null;

    private RectTransform rectTransform;
    private ConcurrentDictionary<int, ScannedItemPopup> bag = new();

    private void Awake() {
        this.rectTransform = this.GetComponent<RectTransform>(); 
    }

    public void AddEntry(Sprite image, string title, string info, int id) {
        if (bag.ContainsKey(id)) {
            // this object has already been added and is currently
            // being maintained
            return;
        }

        var popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity, this.transform);
        popup.SetContent(image, title, info);
        bag.TryAdd(id, popup);

        StartCoroutine(DestroyEntry(id));

        // Hack to prevent weird overlapping behavior of vertical
        // layout group when adding new entry
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private IEnumerator DestroyEntry(int id) {
        yield return new WaitForSeconds(popupLifetimeSeconds);
        bag.TryRemove(id, out ScannedItemPopup entry);
        Destroy(entry.gameObject);
    }
}

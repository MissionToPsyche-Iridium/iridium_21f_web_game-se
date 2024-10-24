using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScannedItemPopup : MonoBehaviour {
    [SerializeField] private float displayForSecs = 5;

    [Header("UI Elements")]
    [SerializeField] private Image imageField;
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;

    private void Start() {
        StartCoroutine(DestroySelf()); 
    }

    public void SetContent(Sprite image, string title, string info) {
        this.imageField.sprite = image;
        this.headerField.text = title;
        this.contentField.text = info;
    }

    private IEnumerator DestroySelf() {
        yield return new WaitForSeconds(displayForSecs);
        Destroy(this.gameObject);
    }
}

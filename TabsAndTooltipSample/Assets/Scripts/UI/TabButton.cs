using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {

    [SerializeField] private TabGroup group;

    private Image imageIcon;

    public void SetImageIcon(Sprite sprite) {
        this.imageIcon.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData) {
        group.OnTabSelect(this);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        group.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        group.OnTabExit(this);
    }

    private void Start() {
        imageIcon = GetComponent<Image>();
        group.Subscribe(this);
    }



}

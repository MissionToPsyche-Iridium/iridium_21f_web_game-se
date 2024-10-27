using UnityEngine;

public class TooltipSystem : MonoBehaviour {
    [SerializeField] private HeaderToolTip headerToolTip;
    [SerializeField] private ComponentTooltip componentTooltip;

    private static TooltipSystem instance;

    private void Awake() {
        instance = this; 
    }

    public static void ShowTextTooltip(string content, string header = "") {
        instance.headerToolTip.SetText(content, header);
        instance.headerToolTip.gameObject.SetActive(true);
    }

    public static void HideTextTooltip() {
        instance.headerToolTip.gameObject.SetActive(false);
    }

    public static void ShowImageTooltip(string name, string info, Sprite image) {
        instance.componentTooltip.SetContent(name, info, image);
        instance.componentTooltip.gameObject.SetActive(true);
    }

    public static void HideImageTooltip() {
        instance.componentTooltip.gameObject.SetActive(false);
    }
}

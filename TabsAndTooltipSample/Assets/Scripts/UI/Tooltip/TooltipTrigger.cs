using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public enum ToolTipType { 
        Text,
        Image,
    }

    [SerializeField] private ToolTipType tooltipType;

    // Text tooltip fields
    [HideInInspector] [SerializeField] private string header;
    [HideInInspector] [SerializeField] private string content;

    // Component tooltip fields
    [HideInInspector][SerializeField] private string componentName;
    [HideInInspector][SerializeField] private string componentInfo;
    [HideInInspector][SerializeField] private Sprite componentImage;

    public void OnPointerEnter(PointerEventData eventData) {
        switch (tooltipType) {
            case ToolTipType.Text: 
                TooltipSystem.ShowTextTooltip(content, header); 
                break;
            case ToolTipType.Image:
                TooltipSystem.ShowImageTooltip(componentName, componentInfo, componentImage); 
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        switch (tooltipType) {
            case ToolTipType.Text: 
                TooltipSystem.HideTextTooltip(); break;
            case ToolTipType.Image:
                TooltipSystem.HideImageTooltip(); break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
  [SerializeField] private Canvas canvas;

  private RectTransform rectTransform;
  private CanvasGroup canvasGroup;

  //Vector2 objectInitPosition; //for snapping back into original position if not dropped in container


  private void Awake() {
    rectTransform = GetComponent<RectTransform>();
    canvasGroup = GetComponent<CanvasGroup>();
  }
    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta /canvas.scaleFactor;
    }

  public void OnEndDrag(PointerEventData eventData) {
    Debug.Log("OnEndDrag");
    canvasGroup.alpha = 1f;
    canvasGroup.blocksRaycasts = true;
    //rectTransform.anchoredPosition = objectInitPosition;
  }

  public void OnPointerDown(PointerEventData eventData) {
    Debug.Log("OnPointerDown");
    //objectInitPosition = rectTransform.anchoredPosition;
  }

}
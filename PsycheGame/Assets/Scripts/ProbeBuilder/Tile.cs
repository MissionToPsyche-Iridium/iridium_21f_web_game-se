using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color color1, color2;
    [SerializeField] private SpriteRenderer render;
    [SerializeField] private GameObject highlight;

    //paints tile
    public void Init(bool isOffset) {
       render.color = isOffset ? color1 : color2;
    }

    //Highlights tile on hover
    void OnMouseEnter() {
        Debug.Log("tile active");
        highlight.SetActive(true);
    }

    void OnMouseExit() {
        Debug.Log("tile inactive");
        highlight.SetActive(false);
    }

}

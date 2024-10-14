using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeBuilder : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public float attachThreshold = 0.5f;
    public GameObject attachedShape;  

    void OnMouseDown()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, 0);
        isDragging = true;
    }   

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, 0) + offset;
        }
    }
    void OnMouseUp()
    {
        isDragging = false;
    }

    void AttachShape(GameObject otherShape)
    {
        if (attachedShape == null)
        {
            otherShape.transform.SetParent(this.transform);
            otherShape.transform.localPosition = Vector3.zero; 
            attachedShape = otherShape; 
        }
    }

    public void DetachShape()
    {
        if (attachedShape != null)
        {
            attachedShape.transform.SetParent(null);
            attachedShape = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ProbePart"))
        {
            if (Vector3.Distance(this.transform.position, other.transform.position) <= attachThreshold)
            {
                AttachShape(other.gameObject);
            }
        }
    }
}

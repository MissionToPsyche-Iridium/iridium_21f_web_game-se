using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    GameObject objSelected = null;

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is clicked on the object
        if (Input.GetMouseButtonDown(0))
            CheckHitObject();
            Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (Input.GetMouseButton(0) && objSelected != null)
        {
            DragObject();
        }
        if (Input.GetMouseButtonUp(0) && objSelected != null)
        {
            DropObject();
        }

    }

    void CheckHitObject()
    {
        RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

        if (hit.collider != null) {
            objSelected = hit.transform.gameObject;
            Debug.Log("collider");
        }
    }

    void DragObject() 
    {
        objSelected.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
            Camera.main.nearClipPlane + 10.0f));
    }

    void DropObject() 
    {
        objSelected = null;
    }

}

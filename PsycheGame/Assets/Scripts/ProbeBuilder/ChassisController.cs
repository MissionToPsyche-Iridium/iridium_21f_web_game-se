using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChassisMap : MonoBehaviour
{

    public string probeTag = "ProbeItem";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // work in process -- need tilemap collision units to work 
    void OnCollisionEnter2D(Collision2D item)
    {
        Debug.Log("<<Collision Enter>>");
        if (item.gameObject.tag == probeTag)
        {
            Vector3 tilePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tilePosition.z = 0;

            Grid gridLayout = transform.parent.GetComponentInParent<Grid>();
            Vector3Int cellPosition = gridLayout.WorldToCell(tilePosition);
            Debug.Log("[Tile Position: " + cellPosition + "]");

            item.transform.position = cellPosition; 

            // transform.position = collision.transform.position + new Vector3(0, 0, -0.01f);
        }
    }    
}

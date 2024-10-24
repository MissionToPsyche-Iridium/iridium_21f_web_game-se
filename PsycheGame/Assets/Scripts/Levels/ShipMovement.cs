using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{

    public float moveSpeed = 5f; 

    void Start()
    {
        
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");     

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}

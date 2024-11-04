using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour {
    [SerializeField] private bool fuelEnabled = true;
    [SerializeField] protected GameObject fuelBarColor;
    
    public float moveSpeed = 5f; 
    public float fuel = 100f;
    public float fuelConsumptionRate = 1f;
    public Slider fuelBar; 
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuelBarColor = GameObject.Find("FuelBarFill");
        UpdateFuelDisplay(); 
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");     

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        if (fuel > 0f && movement != Vector3.zero || !fuelEnabled)
        {
            fuel -= fuelConsumptionRate * Time.deltaTime;
            rb.velocity = movement * moveSpeed;
            RotateShip(movement);
            fuel = Mathf.Max(fuel, 0f);
            UpdateFuelDisplay(); 
        }
        else if (fuelEnabled)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void RotateShip(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }

    public void UpdateFuelDisplay()
    {
        fuelBar.value = fuel;
        if(fuel < 25f){
            fuelBarColor.GetComponent<Image>().color = Color.red;
        } else if(fuel < 50f) {
            fuelBarColor.GetComponent<Image>().color  = Color.yellow;
        } else {
            fuelBarColor.GetComponent<Image>().color = Color.green;
        }
    }
}

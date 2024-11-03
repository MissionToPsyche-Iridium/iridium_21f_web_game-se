using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour {
    [SerializeField] private bool fuelEnabled = true;
    
    public float moveSpeed = 5f; 
    public float fuel = 150f;
    public float fuelConsumptionRate = 1f;
    public TextMeshProUGUI fuelText; 
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

      void UpdateFuelDisplay()
    {
        fuelText.text = "Fuel: " + fuel.ToString("F1");
    }
}

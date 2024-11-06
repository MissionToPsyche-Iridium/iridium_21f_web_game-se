using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class ShipMovement : MonoBehaviour {
    [SerializeField] private bool fuelEnabled = true;

    [SerializeField] private GameObject fuelBarColor;
    [SerializeField] private Slider fuelBar;
    [SerializeField] protected GameObject fuelBarPanel;

    private float fuelLowThreshold = 20f;
    public float moveSpeed = 5f; 
    public float fuelConsumptionRate = 1f;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        fuelBarColor = GameObject.Find("FuelBarFill");
        fuelBarPanel = GameObject.Find("HUD Panel");
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");
        float fuel = ShipManager.Fuel;

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);

        if (fuel > 0f && movement != Vector3.zero || !fuelEnabled)
        {
            ShipManager.Fuel -= fuelConsumptionRate * Time.deltaTime;
            ShipManager.Fuel = Mathf.Max(ShipManager.Fuel, 0f);
            rb.velocity = movement * moveSpeed;
            RotateShip(movement);
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

    
    private void UpdateFuelDisplay() {
        float fuel = ShipManager.Fuel;
        fuelBar.value = fuel;

        if (fuel < 25f) {
            fuelBarColor.GetComponent<Image>().color = Color.red;
            StartCoroutine(FlashLowFuel());
        } else if (fuel < 50f) { 
            fuelBarColor.GetComponent<Image>().color = Color.yellow;
        } else {
            fuelBarColor.GetComponent<Image>().color = Color.green;
        }
    }

    IEnumerator FlashLowFuel()
    {
        while (ShipManager.Fuel < fuelLowThreshold)
        {
            fuelBarPanel.GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            fuelBarPanel.GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
    }
}


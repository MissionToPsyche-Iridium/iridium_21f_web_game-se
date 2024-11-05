using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour {
    [SerializeField] private bool fuelEnabled = true;
    [SerializeField] protected GameObject fuelBarColor;
    
    public float moveSpeed = 5f; 
    public float fuelConsumptionRate = 1f;
    public Slider fuelBar; 
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        fuelBarColor = GameObject.Find("FuelBarFill");
        UpdateFuelDisplay(); 
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

    public void UpdateFuelDisplay() {
        float fuel = ShipManager.Fuel;
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

using System.Collections;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    [SerializeField] private FuelBar fuelBarUI;
    [SerializeField] private GameObject boost;

    private bool isBoosting = false;
    private float targetSpeed; 
    private float baseSpeed = 15f;
    public float moveSpeed = 15f; 
    public float fuelConsumptionRate = 1f;
    public float boostMultiplier = 2f;
    public float boostSpeedChangeRate = 4f;
    private Rigidbody2D rb;

    public void initWithConfig(ShipConfig.ShipMovementConfig config)
    {
        //Incoming ship config move speed is way too slow, fuel consumption isn't working either
        boostMultiplier = config.boostMultiplier;
        boostSpeedChangeRate = config.bostChangeRate; //Teague, there is a typo in boost that needs addressing in ShipConfig
    }

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        LevelManager.OnLevelLoaded += OnLevelLoaded;
    }

    private void OnDestroy()
    {
        LevelManager.OnLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(LevelConfig config)
    {
        ResetPosition();
    }

    public void ResetPosition()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;
    }


    public void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal"); 
        float moveVertical = Input.GetAxis("Vertical");
        float fuel = ShipManager.Fuel;

        if (PauseHandler.IsGamePaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if(fuel <= 0f){
            LevelManager.Instance.RestartLevel();
        } 

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f).normalized;

        if (movement == Vector3.zero)
        {
            rb.angularVelocity *= 0.95f;
        }
        
        if (fuel > 0f && movement != Vector3.zero)
        {
            ShipManager.Fuel -= fuelConsumptionRate * Time.deltaTime;
            ShipManager.Fuel = Mathf.Max(ShipManager.Fuel, 0f);
            rb.velocity = movement * moveSpeed;
            RotateShip(movement);
            HandleBoostInput();
            UpdateSpeed();
            fuelBarUI.UpdateIndicator(fuel);
        }
    }

    void RotateShip(Vector2 direction)
    {
        rb.angularVelocity = 0f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90f;
    }


    private void HandleBoostInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isBoosting = true;
            targetSpeed = baseSpeed * boostMultiplier; 
            boost.SetActive(true); 
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isBoosting = false;
            targetSpeed = baseSpeed; 
            boost.SetActive(false);
        }
    }

    private void UpdateSpeed()
    {
        moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, boostSpeedChangeRate * Time.deltaTime);
        moveSpeed = Mathf.Clamp(moveSpeed, baseSpeed, baseSpeed * boostMultiplier);
    }
}

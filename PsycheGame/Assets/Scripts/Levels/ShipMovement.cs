using System.Collections;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    [SerializeField] private bool fuelEnabled = true;
    [SerializeField] private FuelBar fuelBarUI;
    [SerializeField] GameObject boost;

    public float moveSpeed = 5f; 
    public float fuelConsumptionRate = 1f;
    public float boostMultiplier = 2f;
    public float boostSpeedChangeRate = 2f;
    private Coroutine boostCoroutine;
    private bool isBoosting = false;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
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
            fuelBarUI.UpdateIndicator(fuel);
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

    void HandleBoostInput() {
        if (Input.GetKeyDown(KeyCode.Space) && boostCoroutine == null) {
            boostCoroutine = StartCoroutine(BoostSpeed(boostMultiplier));
            isBoosting = true;
            boost.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Space) && isBoosting) {
            if (boostCoroutine != null) {
                StopCoroutine(boostCoroutine);
            }
            boostCoroutine = StartCoroutine(BoostSpeed(1f / boostMultiplier));
            isBoosting = false;
            boost.SetActive(false);
        }
    }

    IEnumerator BoostSpeed(float targetMultiplier) {
        float targetSpeed = moveSpeed * targetMultiplier;

        while (Mathf.Abs(moveSpeed - targetSpeed) > 0.01f) {
            moveSpeed = Mathf.MoveTowards(moveSpeed, targetSpeed, boostSpeedChangeRate * Time.deltaTime);
            yield return null;
        }

        moveSpeed = targetSpeed; 
        boostCoroutine = null;
    }
}

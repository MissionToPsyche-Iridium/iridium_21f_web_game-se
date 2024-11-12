using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipCollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] GameObject modalPanel;
    [SerializeField] private HealthBar healthBarUI;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Ship hit by asteroid!");
            ShipManager.Health -= CalculateDamage(collision);
            Debug.Log("Ship health at " + ShipManager.Health);
            healthBarUI.UpdateIndicator();
            if (ShipManager.Health <= 0)
            {
                DestroyShip();
            }

        }
    }

    private int CalculateDamage(Collision2D collision)
    {
        Vector2 asteroidVelocity = collision.relativeVelocity.normalized;
        Vector2 collisionNormal = collision.contacts[0].normal;

        float angle = Vector2.Angle(asteroidVelocity, -collisionNormal);

        float[] directHitAngles = { 0f, 90f, 180f, 270f };
        float minAngleDifference = Mathf.Min(
            Mathf.Abs(angle - directHitAngles[0]),
            Mathf.Abs(angle - directHitAngles[1]),
            Mathf.Abs(angle - directHitAngles[2]),
            Mathf.Abs(angle - directHitAngles[3])
        );

        float damageScale = Mathf.InverseLerp(0, 90, minAngleDifference); 
        return Mathf.RoundToInt(Mathf.Lerp(100, 15, damageScale));
    }

    private void DestroyShip()
    {
        modalPanel.SetActive(true);
        Destroy(ship);
    }
}
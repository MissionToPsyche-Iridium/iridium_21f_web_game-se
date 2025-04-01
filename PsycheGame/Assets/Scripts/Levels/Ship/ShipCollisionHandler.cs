using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipCollisionHandler : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] GameObject modalPanel;
    [SerializeField] HealthBar healthBarUI;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            HandleAsteroidCollision(collision.relativeVelocity, collision.contacts[0].normal);
        }
    }

    public void InitializeForTest(GameObject testShip, GameObject testModalPanel, HealthBar testHealthBarUI)
    {
        ship = testShip;
        modalPanel = testModalPanel;
        healthBarUI = testHealthBarUI;
    }
    public void HandleAsteroidCollision(Vector2 relativeVelocity, Vector2 collisionNormal)
    {
        Debug.Log("Ship hit by asteroid!");
        ShipManager.Health -= CalculateDamage(relativeVelocity, collisionNormal);
        Debug.Log("Ship health at " + ShipManager.Health);
        healthBarUI.UpdateIndicator();
        if (ShipManager.Health <= 0)
        {
            DestroyShip();
        }
    }

    private int CalculateDamage(Vector2 relativeVelocity, Vector2 collisionNormal)
    {
        float angle = Vector2.Angle(relativeVelocity.normalized, -collisionNormal);
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
        LevelManager.Instance.RestartLevel();
    }
}
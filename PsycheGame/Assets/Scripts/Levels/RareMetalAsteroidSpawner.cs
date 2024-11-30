using UnityEngine;

public class RareMetalAsteroidSpawner : Spawnable {
    [SerializeField] private Sprite[] asteroidSprites;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (asteroidSprites != null && asteroidSprites.Length > 0) {
            spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Destroy(this.gameObject);
            Spawner.ChildDestroyed();
        }
    }
}

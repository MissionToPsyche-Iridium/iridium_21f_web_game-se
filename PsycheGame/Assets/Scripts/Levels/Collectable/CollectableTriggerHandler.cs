using System.Collections;
using UnityEngine;

public class CollectableTriggerHandler : MonoBehaviour {

    private Collectable collectable;

    private void Awake() {
        collectable = this.GetComponent<Collectable>(); 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            collectable.Collect(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}

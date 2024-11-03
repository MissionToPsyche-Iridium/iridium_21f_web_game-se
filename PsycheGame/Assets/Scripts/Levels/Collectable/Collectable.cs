using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(CollectableTriggerHandler))]
public class Collectable : MonoBehaviour {
    [SerializeField] private CollectableSO collectable;

    private void Reset() {
        GetComponent<CircleCollider2D>().isTrigger = true; 
    }

    public void Collect(GameObject collectedObject) {
        collectable.Collect(collectedObject);
    }
}

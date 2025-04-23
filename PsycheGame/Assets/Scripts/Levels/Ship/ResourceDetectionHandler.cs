using UnityEngine;
using System.Collections.Generic;

public class ResourceDetector : MonoBehaviour
{
    [SerializeField] private GameObject resourceIndicator; 
    [SerializeField] private GameObject arrow;
    private List<GameObject> detectedResources = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mineral") || other.CompareTag("Resource"))
        {
            if (!detectedResources.Contains(other.gameObject))
            {
                detectedResources.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Mineral") || other.CompareTag("Resource"))
        {
            detectedResources.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        GameObject closestResource = null;
        float closestDistanceSqr = float.MaxValue;

        foreach (var resource in detectedResources)
        {
            if (resource == null) continue;

            Vector2 direction = resource.transform.position - transform.position;
            float distanceSqr = direction.sqrMagnitude;

            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestResource = resource;
            }
        }

        if (closestResource != null)
        {
            Vector2 direction = closestResource.transform.position - transform.position;
            float angle = Vector2.SignedAngle(transform.up, direction);
            float displayAngle = angle < 0 ? angle + 360 : angle;
            UpdateIndicator(displayAngle);
        }
        else
        {
            resourceIndicator.gameObject.SetActive(false);
        }
    }

    private void UpdateIndicator(float angle)
    {
        resourceIndicator.gameObject.SetActive(true);
        arrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    private void LateUpdate()
    {
        detectedResources.RemoveAll(resource => resource == null);
    }
}
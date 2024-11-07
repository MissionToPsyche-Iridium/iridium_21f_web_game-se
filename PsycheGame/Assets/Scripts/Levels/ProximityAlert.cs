using System.Collections;
using TMPro;
using UnityEngine;

public class ProximityAlert : MonoBehaviour
{
    public TextMeshPro warningText;           
    public float flashInterval = 0.5f;
    private int nearbyAsteroids = 0;
    private Coroutine flashCoroutine;

    void Start()
    {
        warningText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            nearbyAsteroids++;
            UpdateWarningIndicator();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            nearbyAsteroids--;
            UpdateWarningIndicator();
        }
    }

    private void UpdateWarningIndicator()
    {
        if (nearbyAsteroids > 0)
        {
            if (flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(FlashWarning());
            }
        }
        else
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
            warningText.gameObject.SetActive(false);  
        }
    }

    private IEnumerator FlashWarning()
    {
        while (true)
        {
            warningText.gameObject.SetActive(!warningText.gameObject.activeSelf);
            yield return new WaitForSeconds(flashInterval);
        }
    }
}

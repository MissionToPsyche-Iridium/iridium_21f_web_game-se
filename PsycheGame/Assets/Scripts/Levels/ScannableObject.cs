using UnityEngine;

public class ScannableObject : MonoBehaviour
{
    public bool isScanned = false;

    public void Scan()
    {
        if (!isScanned)
        {
            isScanned = true;
            Debug.Log("Scanned object: " + gameObject.name);
        }
    }
}

using UnityEngine;

public class ShipScanner : MonoBehaviour
{
    private Animator animator;
    private bool isScanning = false;
    [SerializeField] GameObject scanner;

    void Start()
    {
        animator = GetComponent<Animator>();
        scanner.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isScanning = !isScanning;  
            scanner.GetComponent<SpriteRenderer>().enabled = isScanning;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ScannableObject scannable = other.GetComponent<ScannableObject>();
        if (scannable != null && scannable.CompareTag("Scannable"))
        {
            Debug.Log("Scanning " + scannable.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ScannableObject scannable = other.GetComponent<ScannableObject>();
        if (scannable != null)
        {
            Debug.Log("Exiting scan of  " + scannable.name);

        }
    }
}
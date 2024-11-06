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
            animator.SetBool("IsScanning", isScanning);
            scanner.GetComponent<SpriteRenderer>().enabled = isScanning;
        }
    }
}
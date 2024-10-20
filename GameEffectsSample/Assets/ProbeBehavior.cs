using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class ShipBehavior : MonoBehaviour {

    private Rigidbody2D _shipRB;

    [SerializeField] private float maxVelocity = 3f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private ParticleSystem thrusterParticleLeft;
    [SerializeField] private ParticleSystem thrusterParticleRight;

    private void ClampVelocity() {
        float xClamp = Mathf.Clamp(_shipRB.velocity.x, -maxVelocity, maxVelocity);        
        float yClamp = Mathf.Clamp(_shipRB.velocity.y, -maxVelocity, maxVelocity);
        _shipRB.velocity = new Vector2(xClamp, yClamp);
    }

    private void AccelerateForward(float ammount) {
        Vector2 force = transform.up * ammount;
        _shipRB.AddForce(force);
    }

    private void Rotate(Transform t, float ammount) {
        t.Rotate(0, 0, ammount * -rotationSpeed);
    }

    private void ToggleThrusterParticles() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            thrusterParticleLeft.Play();
            thrusterParticleRight.Play();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            thrusterParticleLeft.Stop();
            thrusterParticleRight.Stop();
        }
    }

    private void Start() {
        this._shipRB = this.GetComponent<Rigidbody2D>(); 
    }

    private void Update() {
        float yAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");

        AccelerateForward(yAxis);
        Rotate(transform, xAxis);
        ClampVelocity();

        ToggleThrusterParticles();
    }

}

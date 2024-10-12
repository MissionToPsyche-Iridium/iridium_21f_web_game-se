using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Planet : MonoBehaviour
{
    [SerializeField] Vector2 initialVelocity;

    public Rigidbody2D planetRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        GravityHandler.bodies.Add(planetRigidBody);
        planetRigidBody.AddForce(initialVelocity);
    }
}

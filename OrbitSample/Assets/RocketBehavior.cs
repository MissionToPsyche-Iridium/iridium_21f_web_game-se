using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    public Rigidbody2D rocketRigidBody;

    public float thrustForce = 1f;
    public float rotationSpeed = 200f;
    public float maxThrottle = 1f;
    public float throttleChangeRate = 100000f;

    public float throttle = 0f;
    public float rotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GravityHandler.bodies.Add(rocketRigidBody); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) {
            throttle += throttleChangeRate * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.DownArrow)) {
            throttle -= throttleChangeRate * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            rotation = rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            rotation = -rotationSpeed * Time.deltaTime;
        }

        rocketRigidBody.MoveRotation(rotation + rocketRigidBody.rotation);

        //throttle = Mathf.Clamp(throttle, 0f, maxThrottle);
        
        if (Input.GetKey(KeyCode.Space)) {
            Vector2 thrust = transform.up * throttle * thrustForce;
            rocketRigidBody.AddForce(thrust);
        }
    }
}

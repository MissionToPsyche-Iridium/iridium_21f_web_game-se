using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    [SerializeField] float gravitationalConstant = 1f;

    public static float gravityConstant;
    public static List<Rigidbody2D> bodies = new List<Rigidbody2D>();
    
    // Compute the gravitational force as a vector between between two rigid bodies
    // in the direction from one body to the other.
    public static Vector2 gravitationalForce(Rigidbody2D from, Rigidbody2D to)
    {
        Vector2 distanceVector = to.position - from.position;
        Vector2 unitDistanceVector = distanceVector.normalized;
        
        float massProduct = from.mass * to.mass;
        float distance = distanceVector.magnitude;
        float distanceSqrd = distance * distance;

        // Newton's law of gravity used to compute the force of the two
        // bodies acting on one another.
        float forceMagnitude = (massProduct * -gravityConstant) / distanceSqrd;

        // The unit vector is a vector of length one which points in the
        // direction the two bodies act on each other. This vector is
        // scaled by the magnitude of the force between the two bodies
        return unitDistanceVector * forceMagnitude;
    }

    // Compute the gravitational force of n-bodies acting on a single body as
    // a vector in 2D space.
    public static Vector2 gravitationalForce(Rigidbody2D body, List<Rigidbody2D> bodies)
    {
        Vector2 sum = Vector2.zero;

        // The force of n-bodies acting on a single body is simply the summation
        // of each of the acting forces
        foreach (Rigidbody2D actingBody in bodies) {
            if (actingBody != body) {
                sum += gravitationalForce(actingBody, body);
            }
        }

        return sum;
    }

    public static Vector2 acceleration(Rigidbody2D body, List<Rigidbody2D> bodies)
    {
        return gravitationalForce(body, bodies) / body.mass;
    }

    public static (Vector3[], int) trajectory(Rigidbody2D body, List<Rigidbody2D> bodies, int steps)
    {
        Vector2 originalPos = body.position;
        Vector3 originalVelocity = body.velocity;
        Vector3[] trajoctoryPoints = new Vector3[steps];

        int count = 0;
        for (; count < steps; count++) {
            foreach (Rigidbody2D bdy in bodies) {
                GameObject parentObj = bdy.gameObject;
                CircleCollider2D collider = parentObj.GetComponent<CircleCollider2D>();
                if (collider == null) { 
                    continue; 
                }

                if (collider.OverlapPoint(body.position)) {
                    goto __CleanUp__;
                }
            }

            trajoctoryPoints[count] = body.position;
            body.position += body.velocity * Time.fixedDeltaTime;
            body.velocity += acceleration(body, bodies) * Time.fixedDeltaTime;
        }

    __CleanUp__:
        body.position = originalPos;
        body.velocity = originalVelocity;
        return (trajoctoryPoints, count); 
    }

    // Inheritied from 'MonoBehavior' this fuction is called by unity independent of
    // the current frame-rate and is intended for physics calculations.
    void FixedUpdate()
    {
        gravityConstant = gravitationalConstant;
        foreach (Rigidbody2D body in bodies) {
            Vector2 forceVector = gravitationalForce(body, bodies);
            body.AddForce(forceVector * Time.fixedDeltaTime, ForceMode2D.Impulse);

            LineRenderer lineRenderer = body.gameObject.GetComponent<LineRenderer>();
            if (lineRenderer != null) {
                var (trajectoryPoints, count) = trajectory(body, bodies, 1000);
                lineRenderer.enabled = true;
                lineRenderer.positionCount = count; 
                lineRenderer.SetPositions(trajectoryPoints);
            }
        }
    }
}

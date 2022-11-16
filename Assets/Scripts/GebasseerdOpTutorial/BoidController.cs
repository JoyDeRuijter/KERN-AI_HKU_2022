using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class BoidController : MonoBehaviour
{
    public int swarmIndex;
    public float noClumpingRadius;
    public float localAreaRadius;
    public float speed;
    public float steeringSpeed;
    private Vector3 steering;

    public void SimulateMovement(List<BoidController> _boids, float _time)
    {
        steering = Vector3.zero;
        Vector3 separationDirection = Vector3.zero;
        float separationCount = 0;
        Vector3 alignmentDirection = Vector3.zero;
        float alignmentCount = 0;
        Vector3 cohesionDirection = Vector3.zero;
        float cohesionCount = 0;
        BoidController leaderBoid = _boids[0];
        float leaderAngle = 180f;

        foreach (BoidController b in _boids.Where(b => b != this))
        {
            float distance = Vector3.Distance(b.transform.position, this.transform.position);

            if (distance < noClumpingRadius)
            {
                separationDirection += b.transform.position - transform.position;
                separationCount++;
            }

            if (distance < localAreaRadius && b.swarmIndex == this.swarmIndex)
            {
                alignmentDirection += b.transform.forward;
                alignmentCount++;

                cohesionDirection += b.transform.position - transform.position;
                cohesionCount++;

                float angle = Vector3.Angle(b.transform.position - transform.position, transform.forward);
                if (angle < leaderAngle && angle < 90f)
                {
                    leaderBoid = b;
                    leaderAngle = angle;
                }
            }
        }

        if (separationCount > 0)
            separationDirection /= separationCount;

        separationDirection = -separationDirection;

        if (alignmentCount > 0)
            alignmentDirection /= alignmentCount;

        if (cohesionCount > 0)
            cohesionDirection /= cohesionCount;

        cohesionDirection -= transform.position;

        steering += separationDirection.normalized;
        steering += alignmentDirection.normalized;
        steering += cohesionDirection.normalized;

        if (leaderBoid != null)
            steering += (leaderBoid.transform.position - transform.position).normalized;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, localAreaRadius, LayerMask.GetMask("Default")))
            steering = ((hitInfo.point + hitInfo.normal) - transform.position).normalized;

        if (steering != Vector3.zero)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(steering), steeringSpeed * _time);

        transform.position += transform.TransformDirection(new Vector3(0, 0, speed)) * _time;
    }
}

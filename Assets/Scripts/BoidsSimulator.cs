using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidsSimulator : MonoBehaviour
{
    [Header("BOID SETTINGS")]
    [SerializeField] private int numOfBoids = 10;
    [SerializeField] private float speedLimit = 1;
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private Transform boidParent;

    [Space]
    [Header("BOID BORDERS")]
    [SerializeField] private int minX;
    [SerializeField] private int maxX;
    [SerializeField] private int minY;
    [SerializeField] private int maxY;

    private List<Boid> boids = new List<Boid>();

    private void Awake()
    {
        InitializeBoids();  
    }

    private void Update()
    {
        MoveBoids();
    }

    private void InitializeBoids()
    {
        for (int i = 0; i < numOfBoids; i++)
        {
            GameObject boidObject = Instantiate(boidPrefab, boidParent);
            Boid boid = boidObject.AddComponent<Boid>();
            boid.ID = i;
            boid.SetRandomPosition();
            boid.SetRandomVelocity();
            boids.Add(boid);
        }
    }

    private void MoveBoids()
    {
        Vector2 v1, v2, v3, v4;

        foreach (Boid b in boids)
        { 
            v1 = AllignmentRule(b);
            v2 = CohesionRule(b);
            v3 = SeparationRule(b);
            v4 = BoundPosition(b);

            b.velocity = b.velocity + v1 + v2 + v3 + v4;
            LimitVelocity(b);
            b.position += b.velocity;
        }
    }

    private Vector2 AllignmentRule(Boid _boid)
    {
        Vector2 perceivedCenter = Vector2.zero;

        foreach (Boid b in boids.Where(b => b != _boid))
            perceivedCenter += b.position;

        perceivedCenter /= (boids.Count - 1);

        return (perceivedCenter - _boid.position) / 100;
    }

    private Vector2 CohesionRule(Boid _boid)
    {
        Vector2 perceivedVelocity = Vector2.zero;
        
        foreach (Boid b in boids.Where(b => b != _boid))
            perceivedVelocity += b.velocity;

        perceivedVelocity /= (boids.Count - 1);

        return (perceivedVelocity - _boid.velocity) / 100;
    }

    private Vector2 SeparationRule(Boid _boid)
    {
        Vector2 separationVector = Vector2.zero;

        foreach (Boid b in boids.Where(b => b != _boid))
        {
            if (Vector2.Distance(b.position, _boid.position) < 1)
                separationVector -= b.position - _boid.position;
        }
        return separationVector;
    }

    private void LimitVelocity(Boid _boid)
    {
        if (_boid.velocity.magnitude > speedLimit)
            _boid.velocity = (_boid.velocity / _boid.velocity.magnitude) * speedLimit;
    }

    private Vector2 BoundPosition(Boid _boid)
    {
        Vector2 v = Vector2.zero;

        if (_boid.position.x < minX)        v.x = 10;
        else if (_boid.position.x > maxX)   v.x = -10;
        if (_boid.position.y < minY)        v.y = 10;
        else if (_boid.position.y > maxY)   v.y = -10;

        return v;
    }
}

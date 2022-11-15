using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidsSimulator : MonoBehaviour
{
    [SerializeField] private int numOfBoids = 10;
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private Transform boidParent;
    private List<Boid> boids = new List<Boid>();

    private void Awake()
    {
        InitializeBoids();
        InitializePositions();        
    }

    private void Start()
    {
        SpawnBoids();
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
            boids.Add(boid);
        }
    }

    private void InitializePositions()
    { 
        
    }

    private void SpawnBoids()
    {
        foreach (Boid b in boids)
        {
            
        }
    }

    private void MoveBoids()
    {
        Vector2 v1, v2, v3;

        foreach (Boid b in boids)
        { 
            v1 = AllignmentRule(b);
            v2 = CohesionRule(b);
            v3 = SeparationRule(b);

            b.velocity = b.velocity + v1 + v2 + v3;
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

        return (perceivedVelocity - _boid.velocity) / 8;
    }

    private Vector2 SeparationRule(Boid _boid)
    {
        Vector2 separationVector = Vector2.zero;

        foreach (Boid b in boids.Where(b => b != _boid))
        {
            if (Vector2.Distance(b.position, _boid.position) < 100)
                separationVector -= b.position - _boid.position;
        }
        return separationVector;
    }

}

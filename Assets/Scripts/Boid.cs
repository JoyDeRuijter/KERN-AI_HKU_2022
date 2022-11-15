using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public int ID;

    private void Update()
    {
        Vector3 newPosition = new Vector3(position.x, position.y, 0);
        transform.position = newPosition;
    }

    public void SetRandomPosition() => position = new Vector2(Random.Range(-69, 69), Random.Range(-39, 39));

    public void SetRandomVelocity() => velocity = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
}

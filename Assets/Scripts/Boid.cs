using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity = new Vector2(1, 1);
    public int ID;

    private void Update()
    {
        Vector3 newPosition = new Vector3(position.x, position.y, 0);
        transform.position = newPosition;
    }
}

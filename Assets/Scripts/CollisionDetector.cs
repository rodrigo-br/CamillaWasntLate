using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] BoxCollider2D colliderToCast;
    [SerializeField] private float defaultDistance = 0.03f;

    public RaycastHit2D[] collisionResults { get; private set; }

    void Start()
    {
        Debug.Log("colliderToCast.size.x: " + colliderToCast.size.x);
        Debug.Log("colliderToCast.size.y: " + colliderToCast.size.y);
    }

    public bool CheckCollisionIn(Vector2 direction)
    {
        collisionResults = new RaycastHit2D[2];
        
        return colliderToCast.Cast(direction, collisionResults, defaultDistance) > 0;
    }

    public bool CheckCollisionIn(Vector2 direction, float distance)
    {
        collisionResults = new RaycastHit2D[2];
        
        return colliderToCast.Cast(direction, collisionResults, distance) > 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    [SerializeField] BoxCollider2D colliderToCast;
    [SerializeField] private float defaultDistance = 0.03f;
    public RaycastHit2D[] collisionResults { get; private set; }
    public BoxCollider2D ColliderToCast => colliderToCast;

    public bool CheckCollisionIn(Vector2 direction)
    {
        int results;
        collisionResults = new RaycastHit2D[3];
        
        results = colliderToCast.Cast(direction, collisionResults, defaultDistance);
        for (int i = 0; i < results; i++)
        {
            if (collisionResults[i].collider != null && collisionResults[i].collider.isTrigger)
            {
                results--;
            }
        }
        return results > 0;
    }

    public bool CheckCollisionIn(Vector2 direction, float distance)
    {
        int results;
        collisionResults = new RaycastHit2D[3];
        
        results = colliderToCast.Cast(direction, collisionResults, distance);
        for (int i = 0; i < results; i++)
        {
            if (collisionResults[i].collider != null && collisionResults[i].collider.isTrigger)
            {
                results--;
            }
        }
        return results > 0;
    }
}

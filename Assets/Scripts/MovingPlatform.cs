using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float moveSpeed = 10f;
    Vector3 startPosition;
    Vector3 currentTarget;

    void Awake() 
    {
        startPosition = transform.position;
        currentTarget = target.position;
    }

    void FixedUpdate()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.1f)
        {
            currentTarget = (Vector3.Distance(currentTarget, startPosition) < 0.1f) ? target.position : startPosition;
        }
    }
}

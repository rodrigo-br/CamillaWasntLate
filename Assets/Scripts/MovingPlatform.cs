using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 10f;
    private Vector3 startPosition;
    private Vector3 currentTarget;

    private void Awake() 
    {
        startPosition = transform.position;
        currentTarget = target.position;
    }

    private void FixedUpdate()
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

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag(ConstManager.PLAYER_LAYER))
        {
            other.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.PLAYER_LAYER))
        {
            other.transform.SetParent(null);
        }
    }
}

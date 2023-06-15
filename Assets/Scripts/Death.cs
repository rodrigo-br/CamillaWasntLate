using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    int checkPointIndex = 0;
    Vector3 checkPoint;

    void Awake()
    {
        checkPoint = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown("."))
        {
            Die();
        }
    }

    public void Die() => transform.position = checkPoint;

    public void SetCheckPoint(Checkpoint newCheckPoint)
    {
        if (newCheckPoint.Index > checkPointIndex)
        {
            checkPointIndex = newCheckPoint.Index;
            checkPoint = newCheckPoint.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.CHECKPOINT_LAYER))
        {
            SetCheckPoint(other.GetComponent<Checkpoint>());
        }
    }
}

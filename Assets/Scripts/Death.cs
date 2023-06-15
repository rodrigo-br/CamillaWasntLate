using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] Vector3[] checkPoint;
    int checkPointIndex = 0;

    public void Die() => transform.position = checkPoint[checkPointIndex];

    public void SetCheckPoint(int index)
    {
        if (index > checkPointIndex)
        {
            checkPointIndex = index;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.CHECKPOINT_LAYER))
        {
            SetCheckPoint(other.GetComponent<Checkpoint>().Index);
        }
    }
}

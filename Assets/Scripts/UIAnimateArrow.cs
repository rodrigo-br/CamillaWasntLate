using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimateArrow : MonoBehaviour
{
    [SerializeField] ActivePlayerManager activePlayerManager;
    [SerializeField] RectTransform[] targets;
    RectTransform myTransform;
    IEnumerator coroutine;
    float moveSpeed = 4f;
    int offset;
    int target;

    void Awake()
    {
        myTransform = GetComponent<RectTransform>();
    }

    private void Start() 
    {
        offset = 3 - activePlayerManager.GetNumberOfPlayersInScene();
        SetTarget(1);
    }

    private void OnEnable()
    {
        activePlayerManager.OnSelectedPlayer += SetTarget;
    }

    private void OnDisable()
    {
        activePlayerManager.OnSelectedPlayer -= SetTarget;
    }

    private void SetTarget(int value)
    {
        target = (value - 1) + offset;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = AdjustArrowPositionRoutine();
        StartCoroutine(coroutine);
    }

    IEnumerator AdjustArrowPositionRoutine()
    {
        while (Mathf.Abs(myTransform.position.x - targets[target].position.x) > Mathf.Epsilon)
        {
            myTransform.position = Vector2.MoveTowards(
            myTransform.position,
            new Vector2 (targets[target].position.x, myTransform.position.y),
            moveSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}

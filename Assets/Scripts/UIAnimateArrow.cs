using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimateArrow : MonoBehaviour
{
    [SerializeField] ActivePlayerManager activePlayerManager;
    RectTransform myTransform;
    float moveSpeed = 4f;
    float step;
    int offset;
    Vector2 target;

    void Awake()
    {
        myTransform = GetComponent<RectTransform>();
        step = myTransform.parent.GetComponent<RectTransform>().rect.width / 3;
    }

    private void Start() 
    {
        offset = 3 - activePlayerManager.GetNumberOfPlayersInScene();
        target = new Vector2(step * offset, myTransform.anchoredPosition.y);
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
        target = new Vector2((step * (value - 1)) + (step * offset), myTransform.anchoredPosition.y);
    }

    private void FixedUpdate()
    {
        myTransform.anchoredPosition = Vector2.MoveTowards(myTransform.anchoredPosition, target, moveSpeed);
    }
}

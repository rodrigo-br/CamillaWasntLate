using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] Material teleportMaterial;
    Material defaultMaterial;
    PlayerMovements playerMovements;
    GameObject currentTeleport;
    SpriteRenderer mySpriteRenderer;
    Vector3 targetPosition;
    float teleportCooldown = 1f;
    bool isOnCooldown = false;

    void Awake()
    {
        playerMovements = GetComponentInChildren<PlayerMovements>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = mySpriteRenderer.material;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.TELEPORTER))
        {
            currentTeleport = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.TELEPORTER))
        {
            if (other.gameObject == currentTeleport)
            {
                currentTeleport = null;
            }
        }
    }

    void OnTeleport(InputValue value)
    {
        if (!playerMovements.IsSelected()) { return ; }
        if (value.isPressed && currentTeleport != null && !isOnCooldown)
        {
            targetPosition = currentTeleport.GetComponent<Teleports>().Destination.position;
            isOnCooldown = true;
            mySpriteRenderer.material = new Material(teleportMaterial);
            StartCoroutine(ChangeMaterialValue(mySpriteRenderer.material.GetFloat("_Progress"), 0));
        }
    }

    IEnumerator ChangeMaterialValue(float currentValue, float targetValue)
    {
        float progress = mySpriteRenderer.material.GetFloat("_Progress");
        while (!Mathf.Approximately(progress, targetValue))
        {
            progress = Mathf.MoveTowards(progress, targetValue, Time.deltaTime);
            mySpriteRenderer.material.SetFloat("_Progress", progress);
            yield return null;
        }
        transform.position = targetPosition;
        targetValue = 3;
        while (!Mathf.Approximately(progress, targetValue))
        {
            progress = Mathf.MoveTowards(progress, targetValue, Time.deltaTime * 4);
            mySpriteRenderer.material.SetFloat("_Progress", progress);
            yield return null;
        }
        mySpriteRenderer.material = defaultMaterial;
        yield return new WaitForSeconds(teleportCooldown);
        isOnCooldown = false;
    }
}

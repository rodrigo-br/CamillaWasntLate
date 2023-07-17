using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private Material teleportMaterial;
    private Material defaultMaterial;
    private PlayerMovements playerMovements;
    private GameObject currentTeleport;
    private SpriteRenderer mySpriteRenderer;
    private Vector3 targetPosition;
    private float teleportCooldown = 1f;
    private bool isOnCooldown = false;

    private void Awake()
    {
        playerMovements = GetComponent<PlayerMovements>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultMaterial = mySpriteRenderer.material;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.TELEPORTER))
        {
            currentTeleport = other.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.TELEPORTER))
        {
            currentTeleport = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(ConstManager.TELEPORTER))
        {
            currentTeleport = null;
        }
    }

    private void OnTeleport(InputValue value)
    {
        if (!playerMovements.IsSelected()) { return ; }
        if (value.isPressed && currentTeleport != null && !isOnCooldown)
        {
            Transform destination = currentTeleport.GetComponent<Teleports>().Destination;
            if (destination.gameObject.GetComponent<BoxCollider2D>().IsTouchingLayers(
                LayerMask.GetMask(ConstManager.PLAYER_LAYER)))
            {
                return ;
            }
            targetPosition = destination.position;
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

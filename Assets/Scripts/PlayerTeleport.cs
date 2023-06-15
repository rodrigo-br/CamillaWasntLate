using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleport : MonoBehaviour
{
    GameObject currentTeleport;
    float teleportCooldown = 1f;
    bool isOnCooldown = false;

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
        if (value.isPressed && currentTeleport != null && !isOnCooldown)
        {
            isOnCooldown = true;
            transform.position = currentTeleport.GetComponent<Teleports>().Destination.position;
            StartCoroutine(TeleportCooldownRoutine());
        }
    }

    IEnumerator TeleportCooldownRoutine()
    {
        yield return new WaitForSeconds(teleportCooldown);
        isOnCooldown = false;
    }
}

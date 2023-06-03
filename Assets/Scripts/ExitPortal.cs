using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    [SerializeField][Range(1, 3)] int id;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerKinematic player = other.transform.parent.GetComponentInChildren<PlayerKinematic>();

        if (player != null && player.Id == id)
        {
            myAnimator.SetBool("IsTouchingOwner", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerKinematic player = other.transform.parent.GetComponentInChildren<PlayerKinematic>();

        if (player != null && player.Id == id)
        {
            myAnimator.SetBool("IsTouchingOwner", false);
        }
    }
}

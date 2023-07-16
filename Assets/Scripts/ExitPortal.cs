using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    public delegate void PortalChange(int value);
    public PortalChange OnPortalChange; 
    [SerializeField][Range(1, 3)] int id;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovements player = other.gameObject.GetComponent<PlayerMovements>();

        if (player != null && player.Id == id)
        {
            myAnimator.SetBool("IsTouchingOwner", true);
            OnPortalChange?.Invoke(1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerMovements player = other.gameObject.GetComponent<PlayerMovements>();

        if (player != null && player.Id == id)
        {
            myAnimator.SetBool("IsTouchingOwner", false);
            OnPortalChange?.Invoke(-1);
        }
    }
}

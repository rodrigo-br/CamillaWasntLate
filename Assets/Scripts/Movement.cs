using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpMagnitude = 5f;
    [SerializeField] private BoxCollider2D footCollider;
    [SerializeField][Range(1, 3)] private int id = 1;
    private int selectedPlayer = 1;
    private Rigidbody2D myRigidBody;
    private Vector2 moveInput;
    private bool isJumpBufferCooldown = false;
    private float jumpBufferCooldown = 0.1f;

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (CanMove())
        {
            Move();
        }
    }

    private void Move()
    {
        myRigidBody.velocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (isJumpBufferCooldown || !CanMove()) { return ; }
        if (footCollider.IsTouchingLayers(LayerMask.GetMask(LayersManager.PLAYER_LAYER, LayersManager.FLOOR_LAYER)))
        {
            isJumpBufferCooldown = true;
            myRigidBody.velocity += (Vector2.up * jumpMagnitude);
            StartCoroutine(JumpBufferTimeRoutine());
        }
    }

    private IEnumerator JumpBufferTimeRoutine()
    {
        yield return new WaitForSeconds(jumpBufferCooldown);
        isJumpBufferCooldown = false;
    }

    private void OnSelectPlayer(InputValue value)
    {
        int pressed = (int)value.Get<float>();
        if (pressed != 0)
        {
            selectedPlayer = pressed;
        }
    }

    private bool CanMove() => selectedPlayer == id;
}

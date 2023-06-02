using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicMover : MonoBehaviour
{
    [SerializeField] Rigidbody2D myRigidBody;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpPower = 6.5f;
    private Vector2 velocity;
    public Vector2 CurrentVelocity => velocity;

    private void Start()
    {
        myRigidBody.isKinematic = true;
    }

    public void PerformMovement() => myRigidBody.MovePosition(myRigidBody.position + velocity * Time.fixedDeltaTime);

    public void ApplyGravity() => velocity += Physics2D.gravity * Time.fixedDeltaTime;

    public void ForceMovement(Vector2 offset) => myRigidBody.position = myRigidBody.position + offset;

    public void Jump() => velocity = new Vector2(velocity.x, jumpPower);

    public void MoveHorizontally(float xInput) => velocity = new Vector2(xInput * moveSpeed, velocity.y);

    internal void StopMovementBothAxis() => velocity = Vector2.zero;

    internal void StopMovementX() => velocity = new Vector2(0, velocity.y);

    internal void StopMovementY() => velocity = new Vector2(velocity.x, 0);
}

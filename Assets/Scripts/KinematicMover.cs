using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicMover : MonoBehaviour
{
    [SerializeField] Rigidbody2D myRigidBody;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 6.5f;
    [SerializeField] float maxAcceleration = 35f;
    [SerializeField] float maxAirAcceleration = 20f;
    [Tooltip("How fast it falls after a jump")][SerializeField] float downwardMovementMultiplier = 3f;
    [Tooltip("How fast it reaches it peaks after begin a jump")][SerializeField] float upwardMovementMultiplier = 1.7f;
    private Vector2 desireVelocity;
    public Vector2 CurrentVelocity => desireVelocity;
    private Vector2 velocity;
    private float acceleration;
    private float moveSpeedChange;
    float jumpSpeed;
    float gravityScale;
    float defaultGravityScale = 1f;

    private void Start()
    {
        myRigidBody.isKinematic = true;
    }

    public void PerformMovement(bool grounded)
    {
        velocity = myRigidBody.velocity;

        if (desireVelocity.x == 0)
        {
            velocity.x = 0;
        }
        else
        {
            acceleration = grounded ? maxAcceleration : maxAirAcceleration;
            moveSpeedChange = acceleration * Time.fixedDeltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desireVelocity.x, moveSpeedChange);
        }
        velocity.y = desireVelocity.y;
        if (myRigidBody.velocity.y > 0)
        {
            gravityScale = upwardMovementMultiplier;
        }
        else if (myRigidBody.velocity.y < 0)
        {
            gravityScale = downwardMovementMultiplier;
        }
        else
        {
            gravityScale = defaultGravityScale; // default
        }
        myRigidBody.velocity = velocity;
    }

    public void ApplyGravity() => desireVelocity += (Physics2D.gravity * gravityScale)  * Time.fixedDeltaTime;

    public void ForceMovement(Vector2 offset) => myRigidBody.position = myRigidBody.position + offset;

    public void Jump()
    {
        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        if (velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        desireVelocity = new Vector2(desireVelocity.x, jumpSpeed);
    }

    public void MoveHorizontally(float xInput) => desireVelocity = new Vector2(xInput * moveSpeed, desireVelocity.y);

    internal void StopMovementBothAxis() => desireVelocity = Vector2.zero;

    internal void StopMovementX() => desireVelocity = new Vector2(0, desireVelocity.y);

    internal void StopMovementY() => desireVelocity = new Vector2(desireVelocity.x, 0);
}

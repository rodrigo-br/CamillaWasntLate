using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerKinematic : MonoBehaviour
{
    [SerializeField] private KinematicMover agentMover;
    [SerializeField] private CollisionDetector collisionDetector;
    [SerializeField] private bool justJumped = false;
    [SerializeField] private State currentState = State.Idle;
    [SerializeField] private bool grounded = false;
    [SerializeField] private float safetyDistance = 0.01f;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    [SerializeField][Range(1, 3)] private int id = 1;
    [SerializeField] private GameObject selectedSprite;
    private int selectedPlayer = 1;
    private Vector2 movementInput;
    private float halfPlayerHeight, halfPlayerWidth;
    private InputPlayer inputPlayer;
    public int Id => id;

    private void Awake()
    {
        inputPlayer = new InputPlayer();
    }

    private void OnEnable() 
    {
        inputPlayer.Enable();
    }

    private void OnDisable() 
    {
        inputPlayer.Disable();
    }

    void Start()
    {
        Debug.Log("mySpriteRenderer.bounds.size.x: " + mySpriteRenderer.bounds.size.x);
        Debug.Log("mySpriteRenderer.bounds.size.y: " + mySpriteRenderer.bounds.size.y);
        halfPlayerHeight = mySpriteRenderer.bounds.size.y / 2;
        halfPlayerWidth = mySpriteRenderer.bounds.size.x / 2;
        selectedSprite.SetActive(id == 1);
        inputPlayer.Player.Jump.started += _ => Jump();
    }

    void Update()
    {
        if (!IsSelected()) { return; }

        PlayerInput();
    }

    private void PlayerInput()
    {
        movementInput = inputPlayer.Player.Move.ReadValue<Vector2>();
    }

    private void Jump()
    {
        if (IsSelected() && grounded)
        {
            agentMover.Jump();
            justJumped = true;
            grounded = false;
        }
    }

    void FixedUpdate()
    {
        if (justJumped)
        {
            currentState = State.Jumping;
        }
        if (currentState == State.Idle || currentState == State.Moving)
        {
            (grounded, _) = CheckIfGrounded(true);
        }

        HandleIdleState();
        HandleMovementState();
        HandleJumpState();
        HandleFallState();

        agentMover.PerformMovement();
    }

    private void HandleIdleState()
    {
        if (currentState != State.Idle) { return; }

        agentMover.StopMovementX();
        if (grounded == false)
        {
            currentState = State.Falling;
        }
        else
        {
            if (Mathf.Abs(movementInput.x) > 0.01)
            {
                currentState = State.Moving;
            }
        }
    }

    private void HandleMovementState()
    {
        if (currentState != State.Moving) { return; }

        if (grounded == false)
        {
            currentState = State.Falling;
        }
        else
        {
            if (Mathf.Abs(movementInput.x) < 0.01)
            {
                currentState = State.Idle;
                agentMover.StopMovementBothAxis();
            }
            else
            {
                agentMover.MoveHorizontally(movementInput.x);
                HandleCollisionMovement();
            }
        }
    }

    private void HandleJumpState()
    {
        if (currentState != State.Jumping) { return; }

        if (justJumped)
        {
            justJumped = false;
            agentMover.Jump();
        }
        else
        {
            if (agentMover.CurrentVelocity.y <= 0)
            {
                currentState = State.Falling;
            }
            else
            {
                agentMover.ApplyGravity();
                agentMover.MoveHorizontally(movementInput.x);
            }
        }
        HandleCollisionMovement();
        HandleCollisionMovementUp();
    }

    private void HandleCollisionMovementUp()
    {
        (bool upCollision, RaycastHit2D hitResult) = CheckCollisionUp();

        if (upCollision && !hitResult.collider.isTrigger)
        {
            currentState = State.Falling;
            agentMover.StopMovementY();
        }
        else
        {
            // O que colocar neste else?
        }
    }

    private (bool upCollision, RaycastHit2D hitResult) CheckCollisionUp()
    {

        bool horizontalMovementCollision;
        RaycastHit2D hit;

        (horizontalMovementCollision, hit) = CheckCollisionForMovement();

        if (horizontalMovementCollision && !hit.collider.isTrigger)
        {
            agentMover.StopMovementX();
        }
        return (horizontalMovementCollision, collisionDetector.collisionResults[0]);
    }

    private void HandleFallState()
    {
        if (currentState == State.Falling)
        {
            agentMover.MoveHorizontally(movementInput.x);
            HandleCollisionMovement();
            agentMover.ApplyGravity();
            HandleCollisionMovementDown();
        }
    }

    private void HandleCollisionMovement()
    {
        bool horizontalMovementCollision;
        RaycastHit2D hit;

        (horizontalMovementCollision, hit) = CheckCollisionForMovement();

        if (horizontalMovementCollision && !hit.collider.isTrigger)
        {
            agentMover.StopMovementX();
        }
    }

    private (bool horizontalMovementCollision, RaycastHit2D _) CheckCollisionForMovement()
    {
        return (collisionDetector.CheckCollisionIn(
            agentMover.CurrentVelocity.normalized,
            agentMover.CurrentVelocity.magnitude * Time.fixedDeltaTime + safetyDistance),
            collisionDetector.collisionResults[0]);
    }

    void HandleCollisionMovementDown()
    {
        RaycastHit2D hitResult;
        (grounded, hitResult) = CheckIfGrounded();
        if (grounded && !hitResult.collider.isTrigger)
        {
            currentState = State.Idle;
            agentMover.StopMovementBothAxis();
            float distance = hitResult.distance - safetyDistance;
            agentMover.ForceMovement(new Vector2(0, -distance));
        }
    }

    (bool, RaycastHit2D) CheckIfGrounded(bool addExtraDistance = false)
    {
        float extraDistance = addExtraDistance ? safetyDistance * 2 : safetyDistance;

        return (collisionDetector.CheckCollisionIn(
            Vector2.down,
            Mathf.Abs(agentMover.CurrentVelocity.y) * Time.fixedDeltaTime + extraDistance),
            collisionDetector.collisionResults[0]);
    }

    public void SetSelectedPlayer(int selected)
    {
        bool on;
        selectedPlayer = selected;
        on = IsSelected();
        selectedSprite.SetActive(on);
        if (!on)
        {
            movementInput = movementInput = Vector2.zero;
            agentMover.StopMovementBothAxis();
            agentMover.PerformMovement();
        }
    }

    private bool IsSelected() => selectedPlayer == id;
}

public enum State
{
    Idle,
    Moving,
    Jumping,
    Falling
}

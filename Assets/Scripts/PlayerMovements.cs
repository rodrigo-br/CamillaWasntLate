using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField][Range(1, 3)] private int id = 1;
    [SerializeField] private GameObject selectedSprite;
    [SerializeField] private BoxCollider2D myFeetCollider;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float maxAcceleration = 35f;
    [SerializeField] private float maxAirAcceleration = 20f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float downwardMovementMultiplier = 3f;
    [SerializeField] private float upwardMovementMultiplier = 1.7f;
    private Rigidbody2D myRigidBody;
    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private float maxSpeedChange;
    private float acceleration;
    private Animator myAnimator;
    private int selectedPlayer = 1;
    private InputPlayer inputPlayer;
    public int Id => id;
    private bool grounded = false;
    private float defaultGravityScale;
    private bool desiredJump;

    private void Awake()
    {
        inputPlayer = new InputPlayer();
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        defaultGravityScale = 1f;
    }

    private void OnEnable() 
    {
        inputPlayer.Enable();
    }

    private void OnDisable() 
    {
        inputPlayer.Disable();
    }

    private void Start()
    {
        selectedSprite.SetActive(id == 1);
        inputPlayer.Player.Jump.started += _ => TryJump();
    }

    private void Update()
    {
        if (!IsSelected()) { return; }

        PlayerInput();
    }

    private void PlayerInput()
    {
        direction = inputPlayer.Player.Move.ReadValue<Vector2>();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed, 0f);
    }

    private void TryJump()
    {
        if (IsSelected() && IsOnGround() && !desiredJump)
        {
            desiredJump = true;
        }
    }

    private void Jump()
    {
        desiredJump = false;
        float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
        if (velocity.y > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        AudioManager.Instance.PlayJumpingClip();
        velocity.y += jumpSpeed;
    }

    private void FixedUpdate()
    {
        if (!IsSelected()) { return; }

        grounded = IsOnGround();
        velocity = myRigidBody.velocity;

        acceleration = grounded ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        if (desiredJump)
        {
            Jump();
        }
        AdjustGravityScale();
        myRigidBody.velocity = velocity;

    }

    private void AdjustGravityScale()
    {
        if (myRigidBody.velocity.y > 0)
        {
            myRigidBody.gravityScale = upwardMovementMultiplier;
        }
        else if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.gravityScale = downwardMovementMultiplier;
        }
        else
        {
            myRigidBody.gravityScale = defaultGravityScale;
        }
    }

    public void SetSelectedPlayer(int selected)
    {
        bool on;
        selectedPlayer = selected;
        on = IsSelected();
        selectedSprite.SetActive(on);
        if (!on)
        {
            direction = Vector2.zero;
            myRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            myRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public bool IsSelected() => selectedPlayer == id;

    private bool IsFeetTouching(params string[] layers) => myFeetCollider.IsTouchingLayers(LayerMask.GetMask(layers));

    private bool IsOnGround() => IsFeetTouching(ConstManager.FLOOR_LAYER, ConstManager.PLAYER_LAYER);
}

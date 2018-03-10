using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour {

    #region Settings
    public float walkingSpeed = 6f;
    public float runningSpeed = 10f;
    public float sprintingSpeed = 13f;

    public float crouchSpeedDivider = 2f;

    public float speedInfluenceOnJump = 0.05f;

    public float midairSpeed = 1f;
    public float jumpingStrengh = 5f;
    public float gravityDivider = 3.5f;
    public float maxFallSpeed = 10f;
    #endregion

    #region Public methods
    public MovementState MovementState
    {
        get; set;
    }

    public Vector3 Direction
    {
        get; set;
    }

    public bool Crouching
    {
        get; set;
    }

    public bool Grounded
    {
        get
        {
            return controller.isGrounded;
        }
    }

    private MovingGround platform;
    public MovingGround Platform {
        get
        {
            return platform;
        }

        internal set
        {
            platform = value;
            transform.SetParent(value ? value.transform : null);
        }
    }

    public bool Jump()
    {
        if (!Grounded)
            return false;

        Platform = null;
        return jumpFlag = true;
    }
    #endregion

    #region Protected fields
    protected CharacterController controller;
    protected bool jumpFlag = false;
    protected Vector3 lastMovement;
    protected Vector3 lastPosition;
    #endregion

    void Start () {
        controller = GetComponent<CharacterController>();
	}

	void FixedUpdate () {
        // Compute next movement
        float movementSpeed = 0f;
        switch (MovementState)
        {
            case MovementState.Walking:
                movementSpeed = walkingSpeed;
                break;
            case MovementState.Running:
                movementSpeed = runningSpeed;
                break;
            case MovementState.Sprinting:
                movementSpeed = sprintingSpeed;
                break;
            case MovementState.Stopped:
                movementSpeed = 0f;
                break;
        }

        if (Crouching)
            movementSpeed /= crouchSpeedDivider;

        Vector3 movement = Vector3.zero;
        if(Grounded)
        {
            movement = Direction.normalized;
            movement.Scale(new Vector3(1f, 0f, 1f)); // Make sure no movement in the Y axis is set in the direction

            movement *= movementSpeed;

            if (jumpFlag) // Jump if needed
            {
                movement /= 1.5f;
                movement += new Vector3(0, jumpingStrengh, 0) * (1 + speedInfluenceOnJump * movementSpeed);
                jumpFlag = false;
            }

            movement *= Time.fixedDeltaTime;
        }
        else // Midair
        {
            if (transform.parent != null)
                transform.SetParent(null);

            var directionShift = Direction.normalized;
            directionShift.Scale(new Vector3(1f, 0f, 1f)); // Make sure no movement in the Y axis is set in the direction
            movement = (lastMovement + (directionShift * midairSpeed)) * Time.fixedDeltaTime;
        }
        movement += Physics.gravity/gravityDivider * Time.fixedDeltaTime;
        movement.y = Mathf.Max(movement.y, -(Mathf.Abs(maxFallSpeed)));

        controller.Move(movement);
        lastMovement = movement/Time.fixedDeltaTime; // TODO May need to be improved to account for collisions
    }
}

    public enum MovementState
    {
        Stopped,
        Walking,
        Running,
        Sprinting
    }
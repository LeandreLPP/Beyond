using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(PlayerCameraController))]
public class PlayerInputController : MonoBehaviour {

    #region Settings
    public float mouseSensibility = 50f;
    public float mouseZoomSensibility = 50f;
    #endregion

    #region Protected fields
    protected MovementController moveController;
    protected PlayerCameraController cameraController;
    protected MovementState orderState;
    #endregion
    
    void Start () {
        moveController = GetComponent<MovementController>();
        cameraController = GetComponent<PlayerCameraController>();

        orderState = MovementState.Running;
        moveController.Direction = Vector3.zero;
        moveController.Crouching = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
    
    void Update()
    {
        ListenMovementInputs();
        ListenCameraMovements();
    }

    private void ListenMovementInputs()
    {
        // Deal with sprinting
        if (Input.GetButtonDown("Sprint"))
            orderState = MovementState.Sprinting;
        else if (Input.GetButtonUp("Sprint"))
            if(Input.GetButton("Walk"))
                orderState = MovementState.Walking;
            else
                orderState = MovementState.Running;

        // Deal with walking
        if (Input.GetButtonDown("Walk"))
            orderState = MovementState.Walking;
        else if (Input.GetButtonUp("Walk"))
            if(Input.GetButton("Sprint"))
                orderState = MovementState.Sprinting;
            else
                orderState = MovementState.Running;

        // Toggle crouch
        if (Input.GetButtonDown("Crouch"))
            moveController.Crouching = !moveController.Crouching;

        // Jump
        if (Input.GetButtonDown("Jump"))
            moveController.Jump();


        // Compute direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Quaternion angle = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up);
        var direction = angle * new Vector3(horizontal, 0, vertical);
        // Give order to movement controller
        moveController.Direction = direction;

        // If no direction, stop moving
        if (direction.magnitude != 0)
        {
            var shooterComponent = GetComponent<ICarrier>();
            // Can't move faster than walking if aiming
            if (shooterComponent != null && 
                shooterComponent.Weapon != null && 
                shooterComponent is RangedWeapon &&
                (shooterComponent as RangedWeapon).IsAiming)
                moveController.MovementState = MovementState.Walking;
            else
                moveController.MovementState = orderState;
        }
        else
            moveController.MovementState = MovementState.Stopped;
        
    }

    private void ListenCameraMovements()
    {
        // Orientation
        float x = Input.GetAxis("Mouse X") * mouseSensibility * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensibility * Time.deltaTime;

        transform.Rotate(0, x, 0);
        cameraController.CurrentAngle += y;

        // Distance
        float diff = Input.GetAxis("Mouse ScrollWheel") * mouseZoomSensibility * Time.deltaTime;
        cameraController.CurrentDistance -= diff;

        // Change cam place
        if (Input.GetButtonDown("ToggleCam"))
            cameraController.cameraOnRight = !cameraController.cameraOnRight;
    }
}

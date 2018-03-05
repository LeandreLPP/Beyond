using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController), typeof(PlayerCameraController))]
public class PlayerInputController : MonoBehaviour, IShooter {

    #region Settings
    public float mouseSensibility = 50f;
    public float mouseZoomSensibility = 50f;

    public RangedWeapon rangedWeapon;
    public float recoilRecover = 2f;
    public float maxRecoil = 10f;
    #endregion

    #region Protected fields
    protected MovementController moveController;
    protected PlayerCameraController cameraController;
    protected MovementState orderState;

    protected Vector3 recoilAccumulated = new Vector3();
    #endregion
    
    void Start () {
        moveController = GetComponent<MovementController>();
        cameraController = GetComponent<PlayerCameraController>();

        orderState = MovementState.Running;
        moveController.Direction = Vector3.zero;
        moveController.Crouching = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Weapon = rangedWeapon;
	}
    
    void Update()
    {

        ListenMovementInputs();
        ListenCameraMovements();
        ListenShootingInputs();
        SetWeaponDirection();

        RecoverRecoil();
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
            // Can't move faster than walking if aiming
            if (rangedWeapon && rangedWeapon.IsAiming)
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

    private void ListenShootingInputs()
    {
        if (rangedWeapon == null)
            return;

        if (Input.GetButtonDown("Fire"))
            rangedWeapon.PullTrigger();
        else if (Input.GetButtonUp("Fire"))
            rangedWeapon.ReleaseTrigger();

        if (Input.GetButtonDown("Aim"))
        {
            rangedWeapon.StartAiming();
            cameraController.FieldOfView /= 1.5f;
        }
        else if (Input.GetButtonUp("Aim"))
        {
            rangedWeapon.StopAiming();
            cameraController.FieldOfView *= 1.5f;
        }

        if (Input.GetButtonDown("Firemode"))
            if (rangedWeapon is Firearm)
                (rangedWeapon as Firearm).CycleMode();

        if (Input.GetButtonDown("Reload"))
            if (rangedWeapon is Firearm)
                (rangedWeapon as Firearm).Reload();
    }

    private void SetWeaponDirection()
    {
        var cam = cameraController.camera;
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        Ray ray = cam.ScreenPointToRay(new Vector3(x, y));
        RaycastHit info;
        var mask = ~(1 << gameObject.layer); // Mask "IgnoreRaycast"
        Physics.Raycast(ray, out info, mask);

        var trueOrigin = (Weapon.transform.position + Weapon.shotOrigin);
        var dir = info.point - trueOrigin;

        Weapon.ShootDirection = dir;
    }

    #region Inheritances
    #region Shooter
    public RangedWeapon Weapon
    {
        get
        {
            return rangedWeapon;
        }

        set
        {
            if (rangedWeapon != null)
                rangedWeapon.Shooter = null;
            rangedWeapon = value;
            rangedWeapon.Shooter = this;
        }
    }

    public void ApplyRecoil(Vector3 recoil)
    {
        transform.Rotate(0, recoil.y, 0);
        cameraController.CurrentAngle += recoil.x;
        recoilAccumulated += recoil;
    }

    private void RecoverRecoil()
    {
        var recovering = -recoilAccumulated.normalized * recoilRecover * Time.deltaTime * (recoilAccumulated.magnitude);
        ApplyRecoil(recovering);
    }
    #endregion
    #endregion
}

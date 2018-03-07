using UnityEngine;
using System.Collections;

public class PlayerCarrier : BasicShooter
{

    protected PlayerCameraController cameraController;


    // Use this for initialization
    void Start()
    {
        RangedWeapon = rangedWeapon;
        cameraController = GetComponent<PlayerCameraController>();
        recoilAccumulated = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        ListenShootingInputs();
        SetWeaponDirection();

        RecoverRecoil();
    }

    private void ListenShootingInputs()
    {
        if (!RangedWeapon)
            return;

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
        if (!RangedWeapon)
            return;
        var cam = cameraController.camera;
        int x = Screen.width / 2;
        int y = Screen.height / 2;

        var trueOrigin = (RangedWeapon.transform.position + RangedWeapon.shotOrigin);
        Vector3 dir;

        Ray ray = cam.ScreenPointToRay(new Vector3(x, y));
        RaycastHit info;
        int mask = ~(1 << gameObject.layer); // Mask "IgnoreRaycast"
        if (Physics.Raycast(ray, out info, 500f, mask))
            dir = info.point - trueOrigin;
        else
            dir = ray.direction;

        RangedWeapon.ShootDirection = dir;
    }

    public override void ApplyRecoil(Vector3 recoil)
    {
        transform.Rotate(0, recoil.y, 0);
        cameraController.CurrentAngle += recoil.x;
        recoilAccumulated += recoil;
    }
}

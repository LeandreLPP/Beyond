using System;
using UnityEngine;

public class PlayerCarrier : BasicCarrier
{

    protected PlayerCameraController cameraController;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        cameraController = GetComponent<PlayerCameraController>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Weapon == null)
            return;

        if (Weapon is RangedWeapon)
            ListenShootingInputs();
        else if (Weapon is MeleeWeapon)
            ListenMeleeInputs();

        SetWeaponDirection();
    }

    private void ListenMeleeInputs()
    {
        var meleeWeapon = Weapon as MeleeWeapon;
        if (Input.GetButtonDown("Fire"))
            meleeWeapon.QuickStrike();

        if (Input.GetButtonDown("Aim"))
            meleeWeapon.Parry();

        if (Input.GetButtonDown("Firemode"))
            meleeWeapon.StrongStrike();

        if (Input.GetButtonDown("Reload"))
            meleeWeapon.SprecialStrike();
    }

    private void ListenShootingInputs()
    {
        var rangedWeapon = Weapon as RangedWeapon;
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
        if (Weapon == null || !(Weapon is RangedWeapon))
            return;
        var cam = cameraController.camera;
        int x = Screen.width / 2;
        int y = Screen.height / 2;
        
        Vector3 dir;

        Ray ray = cam.ScreenPointToRay(new Vector3(x, y));
        RaycastHit info;
        int mask = ~(1 << gameObject.layer); // Mask "IgnoreRaycast"
        if (Physics.Raycast(ray, out info, 500f, mask))
            dir = info.point;
        else
            dir = ray.direction * 500f;

        (Weapon as RangedWeapon).ShootTarget = dir;
    }

    public override void ApplyRecoil(Vector3 recoil)
    {
        base.ApplyRecoil(recoil);
        transform.Rotate(0, recoil.y, 0);
        cameraController.CurrentAngle += recoil.x;
    }
}

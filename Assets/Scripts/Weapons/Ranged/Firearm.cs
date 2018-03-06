using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firearm : RangedWeapon
{
    public GameObject impact;

    #region Settings
    public int magazine = 8;
    public float reloadTime = 2.0f;
    public float damage = 10f;
    public float range = 5f;

    public Vector3 minRecoil = new Vector3(0f, -0.5f);
    public Vector3 maxRecoil = new Vector3(5f, 0.5f);

    public bool semiAutoEnabled = true;
    public float shotDelay = 0.3f;

    public bool burstEnabled = false;
    public int burstSize = 3;
    public float burstDelay = 0.5f;

    public bool autoEnabled = false;
    public float autoFireDelay = 0.1f;
    #endregion

    #region Public Variables
    public FireMode Firemode { get; protected set; }

    public int Ammo { get; protected set; }
    #endregion

    #region Private variables
    protected FireMode[] enabledFiremodes;
    protected int fireModeIndex = 0;

    protected float lastShot;

    protected int burstShotLeft = 0;
    protected float reloadStartTime = 0f;
    protected bool triggerDown = false;
    protected bool reloading = false;
    #endregion

    public virtual float ReloadRemaining
    {
        get
        {
            return Mathf.Max(0f, reloadTime - (Time.time - reloadStartTime));
        }
    }

    public virtual float ReloadPercentageRemaining
    {
        get
        {
            return ReloadRemaining / reloadTime;
        }
    }

    public virtual bool Reloading
    {
        get
        {
            return reloading;
        }
    }

    void Start()
    {
        lastShot = 0f;
        List<FireMode> list = new List<FireMode>();

        if (semiAutoEnabled || (!semiAutoEnabled && !autoEnabled && !burstEnabled))
            list.Add(FireMode.SemiAuto);
        if (burstEnabled)
            list.Add(FireMode.Burst);
        if (autoEnabled)
            list.Add(FireMode.Automatic);

        enabledFiremodes = list.ToArray();
        Firemode = enabledFiremodes[0];

        Ammo = magazine;
    }

    void FixedUpdate()
    {
        if (reloading)
            return;

        var timeSinceLastShot = Time.fixedTime - lastShot;
        if (magazine > 0 && Ammo <= 0)
            Reload();
        else
            while ((timeSinceLastShot -= autoFireDelay) > 0) // Should account for multiple shot between frames
            {
                if (Firemode == FireMode.Burst && burstShotLeft > 0)
                {
                    burstShotLeft--;
                    Shoot();
                }
                else if(Firemode == FireMode.Automatic && triggerDown)
                    Shoot();
            }
        
    }

    protected override void Shoot()
    {
        if(magazine > 0 && Ammo > 0 && !reloading)
        {
            Ammo--;
            base.Shoot();
            var recoilX = UnityEngine.Random.Range(minRecoil.x, maxRecoil.x);
            var recoilY = UnityEngine.Random.Range(minRecoil.y, maxRecoil.y);
            var recoil = new Vector3(recoilX, recoilY);
            Shooter.ApplyRecoil(recoil);
            lastShot = Time.fixedTime;
        }
    }

    public override void PullTrigger()
    {
        var timeSinceLastShot = Time.fixedTime - lastShot;
        triggerDown = true;
        switch (Firemode)
        {
            case FireMode.SemiAuto:
                if (timeSinceLastShot > shotDelay)
                    Shoot();
                break;
            case FireMode.Burst:
                if (burstShotLeft == 0 && timeSinceLastShot > burstDelay && !reloading)
                {
                    Shoot();
                    burstShotLeft = burstSize - 1;
                }
                break;
            case FireMode.Automatic:
                if(timeSinceLastShot > shotDelay)
                {
                    if(!reloading)
                        Shoot();
                }
                break;
        }
    }

    public override void ReleaseTrigger()
    {
        triggerDown = false;
    }

    public virtual void Reload()
    {
        if (Reloading || Ammo >= magazine)
            return;

        reloading = true;
        burstShotLeft = 0;
        reloadStartTime = Time.time;
        StartCoroutine(FinishReloading());
    }

    public virtual void CycleMode()
    {
        fireModeIndex = (fireModeIndex + 1) % enabledFiremodes.Length;
        Firemode = enabledFiremodes[fireModeIndex];

        burstShotLeft = 0;
    }

    protected override void LaunchProjectile(Vector3 origin, Vector3 direction)
    {
        RaycastHit raycastHit;
        if (!Physics.Raycast(origin, direction, out raycastHit, range))
            return;

        Instantiate(impact, raycastHit.point, new Quaternion());

        if(raycastHit.collider)
        {
            Damageable target = raycastHit.collider.gameObject.GetComponent<Damageable>();
            if (target != null)
                target.TakeDamages(damage);
        }
    }

    protected virtual IEnumerator FinishReloading()
    {
        yield return new WaitForSeconds(reloadTime);
        Ammo = magazine;
        lastShot = Time.time;
        reloading = false;
    }

    public enum FireMode
    {
        SemiAuto,
        Burst,
        Automatic
    }
}

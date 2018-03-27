using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : AWeapon  {

    #region Settings
    public Vector3 shotOrigin = Vector3.zero;
    public float dispertionAngle = 5f;
    public float aimedDispertion = 1f;
    #endregion

    #region Protected variables
    protected bool aiming = false;
    #endregion

    #region IWeapon inheritance
    public override ACarrier Carrier
    {
        get; set;
    }
    #endregion

    #region Public methods
    public Vector3 ShootTarget
    {
        get; set;
    }

    public bool IsAiming { get { return aiming; } }

    public abstract void PullTrigger();
    public abstract void ReleaseTrigger();

    public virtual void StartAiming()
    {
        aiming = true;
    }

    public virtual void StopAiming()
    {
        aiming = false;
    }
    #endregion

    #region Protected methods
    protected virtual void Shoot()
    {
        var shootDirection = ShootTarget - (transform.position + shotOrigin);
        var actualDispertion = Random.Range(0f, aiming ? aimedDispertion : dispertionAngle);
        var axis = new Vector3(shootDirection.z, 0, -shootDirection.x);
        var direction = Quaternion.AngleAxis(actualDispertion, axis) * shootDirection;

        var rand = Random.Range(0f, 360f);
        direction = Quaternion.AngleAxis(rand, shootDirection) * direction;

        LaunchProjectile(transform.position + shotOrigin, direction);
    }

    protected abstract void LaunchProjectile(Vector3 origin, Vector3 direction);
    #endregion
}

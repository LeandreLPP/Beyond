using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BasicShooter : IShooter
{
    public RangedWeapon rangedWeapon;
    public float recoilRecover = 2f;
    public float maxRecoil = 10f;

    protected Vector3 recoilAccumulated = new Vector3();

    void Start()
    {
        RangedWeapon = rangedWeapon;
    }

    #region Shooter
    public override RangedWeapon RangedWeapon
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
            if(rangedWeapon != null)
                rangedWeapon.Shooter = this;
        }
    }

    public override IWeapon Weapon
    {
        get
        {
            return RangedWeapon;
        }

        protected set
        {
            RangedWeapon = value as RangedWeapon;
        }
    }

    public override void ApplyRecoil(Vector3 recoil)
    {
        recoilAccumulated += recoil;
    }

    protected virtual void RecoverRecoil()
    {
        var recovering = -recoilAccumulated.normalized * recoilRecover * Time.deltaTime * (recoilAccumulated.magnitude);
        ApplyRecoil(recovering);
    }
    #endregion
}
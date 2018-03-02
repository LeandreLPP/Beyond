using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BasicShooter : MonoBehaviour, IShooter
{
    public RangedWeapon rangedWeapon;
    public float recoilRecover = 2f;
    public float maxRecoil = 10f;

    protected Vector3 recoilAccumulated = new Vector3();

    void Start()
    {
        Weapon = rangedWeapon;
    }

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

    public virtual void ApplyRecoil(Vector3 recoil)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BasicCarrier : ACarrier
{
    public AWeapon weapon;

    public float recoilRecover = 2f;
    public float maxRecoil = 10f;

    protected Vector3 recoilAccumulated = new Vector3();

    protected virtual void Start()
    {
        Equip(weapon);
    }

    protected virtual void Update()
    {
        RecoverRecoil();
    }
    #region Shooter

    public override void ApplyRecoil(Vector3 recoil)
    {
        recoilAccumulated += recoil;
    }

    protected virtual void RecoverRecoil()
    {
        var recovering = -recoilAccumulated.normalized * recoilRecover * Time.deltaTime * (recoilAccumulated.magnitude);
        ApplyRecoil(recovering);
    }

    public override bool CanSwap(AWeapon newWeapon)
    {
        return true;
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractShooter : AbstractCarrier, IShooter
{
    public abstract RangedWeapon RangedWeapon { get; protected set; }

    public override IWeapon Weapon {
        get
        {
            return RangedWeapon;
        }
    }

    public abstract void ApplyRecoil(Vector3 recoil);

    public override bool AcceptWeapon(IWeapon weapon)
    {
        return (weapon is RangedWeapon) || weapon == null;
    }

    public override bool TryEquip(IWeapon weapon)
    {
        if (!AcceptWeapon(weapon))
            return false;

        RangedWeapon = weapon as RangedWeapon;
        return base.TryEquip(weapon);
    }
}

public interface IShooter : ICarrier
{
    RangedWeapon RangedWeapon { get; }

    void ApplyRecoil(Vector3 recoil);
}

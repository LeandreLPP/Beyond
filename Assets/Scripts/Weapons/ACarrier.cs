using System.Collections.Generic;
using UnityEngine;

public abstract class ACarrier : MonoBehaviour, ICarrier
{
    public GameObject hand;
    protected virtual GameObject Hand
    {
        get
        {
            return hand;
        }
    }

    public virtual AWeapon Weapon { get; protected set; }
    
    public virtual bool CanEquip(AWeapon otherWeapon)
    {
        return Weapon == null;
    }

    public virtual bool Equip(AWeapon otherWeapon)
    {
        if (!CanEquip(otherWeapon) || otherWeapon == null)
            return false;
        
        otherWeapon.transform.SetParent(Hand.transform);
        otherWeapon.transform.localPosition = Vector3.zero;
        otherWeapon.transform.localEulerAngles = Vector3.zero;

        otherWeapon.Carrier = this;

        Weapon = otherWeapon;

        return true;
    }

    public virtual AWeapon UnequipWeapon()
    {
        AWeapon ret = Weapon;

        Weapon.Carrier = null;
        Weapon = null;

        return ret;
    }

    public abstract void ApplyRecoil(Vector3 recoil);

    public abstract bool CanSwap(AWeapon newWeapon);
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RackDisplay : ACarrier {
    

    private void OnTriggerEnter(Collider other)
    {
        var carrier = other.gameObject.GetComponent<ICarrier>();
        if (carrier != null)
        {
            if (carrier.CanEquip(Weapon))
            {
                var weapExt = carrier.Weapon;
                carrier.Equip(Weapon);
                Equip(weapExt);
            }
            else if (Weapon == null)
            {
                var weapExt = carrier.UnequipWeapon();
                Equip(weapExt);
            }
        }
    }

    public override bool CanEquip(AWeapon weapon)
    {
        return true;
    }

    public override bool CanSwap(AWeapon newWeapon)
    {
        return true;
    }

    public override void ApplyRecoil(Vector3 recoil) { }
}

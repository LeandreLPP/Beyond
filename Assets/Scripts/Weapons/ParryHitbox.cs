using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryHitbox : Damageable
{
    public AWeapon weapon;
    public float strengh;

    public override void TakeDamages(float damageAmount, AWeapon source)
    {
        if (weapon == source || !(source is MeleeWeapon))
            return;

        if (damageAmount > strengh)
            weapon.Carrier.Parried();
        else
            source.Carrier.Parried();
    }
}

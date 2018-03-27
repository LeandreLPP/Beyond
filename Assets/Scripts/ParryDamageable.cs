using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryDamageable : DebugDamageable {

    public bool isParrying;
    public float angle = 90;
    public float strengh;
    public MeleeWeapon weapon;

    public override void TakeDamages(float damageAmount, AWeapon source)
    {
        if(isParrying && source is MeleeWeapon && AngleBetween(transform, source.Carrier.transform) <= angle)
        {
            if (damageAmount > strengh)
                weapon.Carrier.Parried();
            else
                source.Carrier.Parried();
        }
        else
            base.TakeDamages(damageAmount, source);
    }

    private float AngleBetween(Transform t1, Transform t2)
    {
        var dir = t2.position - t1.position;
        return Vector3.Angle(t1.forward, dir);
    }
}

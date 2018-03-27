using System;
using UnityEngine;

[RequireComponent(typeof(ACarrier), typeof(MovementController))]
public class AnimatedCarrier : MonoBehaviour
{
    public MeleeWeapon Weapon
    {
        get
        {
            if (GetComponent<ACarrier>().Weapon == null || !(GetComponent<ACarrier>().Weapon is MeleeWeapon))
                return null;

            return GetComponent<ACarrier>().Weapon as MeleeWeapon;
        }
    }

    public float baseDamages = 0;
    public bool hitboxActivated = false;
    public bool parryActivated = false;

    public bool animationDriven = false;

    public bool animationLocked = false;

    private void FixedUpdate()
    {
        GetComponent<MovementController>().AnimationDriven = animationDriven;
        GetComponent<Animator>().SetBool("AnimationLocked", animationLocked);

        if (Weapon == null) return;

        Weapon.BaseDamages = baseDamages;
        Weapon.HitboxActivated = hitboxActivated;

        GetComponent<ParryDamageable>().weapon = Weapon;
        GetComponent<ParryDamageable>().isParrying = parryActivated;

        if (Weapon != null)
            GetComponent<ParryDamageable>().strengh = Weapon.parryStrengh;
        else
            GetComponent<ParryDamageable>().isParrying = false;
    }
}

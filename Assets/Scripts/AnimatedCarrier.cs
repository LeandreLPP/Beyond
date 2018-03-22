using System;
using UnityEngine;

[RequireComponent(typeof(ICarrier), typeof(MovementController))]
public class AnimatedCarrier : MonoBehaviour
{
    public MeleeWeapon Weapon
    {
        get
        {
            if (GetComponent<ICarrier>().Weapon == null || !(GetComponent<ICarrier>().Weapon is MeleeWeapon))
                return null;

            return GetComponent<ICarrier>().Weapon as MeleeWeapon;
        }
    }

    public float baseDamages = 0;
    public bool hitboxActivated = false;
    public bool parryActivated = false;

    public bool animationDriven = false;

    public bool animationLocked = false;

    private void FixedUpdate()
    {
        if (Weapon == null) return;

        Weapon.BaseDamages = baseDamages;
        Weapon.HitboxActivated = hitboxActivated;
        Weapon.ParryActivated = parryActivated;

        GetComponent<MovementController>().AnimationDriven = animationDriven;

        GetComponent<Animator>().SetBool("AnimationLocked", animationLocked);
    }
}

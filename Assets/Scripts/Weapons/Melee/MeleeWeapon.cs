using UnityEngine;

public class MeleeWeapon : AWeapon
{
    public string meleeTypeName;
    public GameObject parryObject;

    protected Animator animator;
    public override ICarrier Carrier {
        get
        {
            return base.Carrier;
        }
        set
        {
            if (Carrier != null)
                animator.SetBool(meleeTypeName, false);
            base.Carrier = value;
            if (value != null)
            {
                animator = value.Animator;
                animator.SetBool(meleeTypeName, true);
            }
        }
    }

    public virtual void QuickStrike()
    {
        if (animator == null) return;

        animator.SetTrigger("Quick");
    }

    public virtual void StrongStrike()
    {
        if (animator == null) return;

        animator.SetTrigger("Strong");
    }

    public virtual void SprecialStrike()
    {
        if (animator == null) return;

        animator.SetTrigger("Special");
    }

    public virtual void Parry(bool on)
    {
        if (animator == null) return;

        parryObject.SetActive(on);
        animator.SetBool("Parry", on);
    }
}
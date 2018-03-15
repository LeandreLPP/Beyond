using UnityEngine;

public class MeleeWeapon : AWeapon
{
    public string meleeTypeName;
    public GameObject parryObject;
    public float weaponStrengh = 1;

    protected Animator animator;
    public override ICarrier Carrier {
        get
        {
            return base.Carrier;
        }
        set
        {
            if (Carrier != null)
            {
                animator.SetBool(meleeTypeName, false);
            }
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
    public virtual void Parry()
    {
        if (animator == null) return;

        animator.SetTrigger("Parry");
    }

    public virtual bool ParryActivated
    {
        get
        {
            return parryObject.activeInHierarchy;
        }
        set
        {
            parryObject.SetActive(value);
        }
    }

    public virtual bool HitboxActivated { get; set; }
    public virtual float BaseDamages { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!HitboxActivated)
            return;

        if (other.tag == "ParryHitbox" && other.gameObject != parryObject)
            Carrier.Parried();

        Damageable target = other.GetComponent<Damageable>();
        if (target != null)
            target.TakeDamages(BaseDamages * weaponStrengh);
    }
}
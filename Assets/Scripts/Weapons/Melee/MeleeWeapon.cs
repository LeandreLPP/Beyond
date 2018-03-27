using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : AWeapon
{
    public string meleeTypeName;
    public float weaponStrengh = 1;
    public float weaponSpeed = 1;
    public float parryStrengh = 30;

    public float resetTime = 0.5f;

    private Dictionary<string, float> map;

    private void Start()
    {
        map = new Dictionary<string, float>();
    }

    protected Animator animator;
    public override ACarrier Carrier {
        get
        {
            return base.Carrier;
        }
        set
        {
            if (Carrier != null)
            {
                animator.SetBool(meleeTypeName, false);
                animator.SetFloat("meleeSpeed", 1f);
            }
            HitboxActivated = false;
            BaseDamages = 0f;
            base.Carrier = value;
            if (value != null)
            {
                animator = value.Animator;
                animator.SetBool(meleeTypeName, true);
                animator.SetFloat("meleeSpeed", weaponSpeed);
            }
        }
    }

    public virtual void QuickStrike()
    {
        if (animator == null) return;

        string k = "Quick";
        animator.SetTrigger(k);
        map.Remove(k);
        map.Add(k, Time.time);
    }
    public virtual void StrongStrike()
    {
        if (animator == null) return;
        
        string k = "Strong";
        animator.SetTrigger(k);
        map.Remove(k);
        map.Add(k, Time.time);
    }
    public virtual void SprecialStrike()
    {
        if (animator == null) return;
        
        string k = "Special";
        animator.SetTrigger(k);
        map.Remove(k);
        map.Add(k, Time.time);
    }
    public virtual void Parry()
    {
        if (animator == null) return;
        
        string k = "Parry";
        animator.SetTrigger(k);
        map.Remove(k);
        map.Add(k, Time.time);
    }

    private void Update()
    { // Reset orders that are pending for too long
        var t = Time.time;
        List<string> toRemove = new List<string>();
        foreach(var d in map)
            if(t - d.Value > resetTime)
            {
                animator.ResetTrigger(d.Key);
                toRemove.Add(d.Key);
            }

        foreach (var k in toRemove)
            map.Remove(k);
    }

    public virtual bool HitboxActivated { get; set; }
    public virtual float BaseDamages { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!HitboxActivated)
            return;

        ACarrier c = other.GetComponent<ACarrier>();
        if (c == Carrier)
            return;

        Damageable target = other.GetComponent<Damageable>();
        if (target != null) 
            target.TakeDamages(BaseDamages * weaponStrengh, this);
    }
}
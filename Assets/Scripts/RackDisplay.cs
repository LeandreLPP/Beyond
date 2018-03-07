using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RackDisplay : ICarrier {

    public GameObject rack;

    private void OnTriggerEnter(Collider other)
    {
        var carrier = other.gameObject.GetComponent<ICarrier>();
        if(carrier && carrier.AcceptWeapon(Weapon))
        {
            var weapExt = carrier.Weapon;
            carrier.TryEquip(Weapon);
            TryEquip(weapExt);
        }
    }

    private IWeapon weapon;
    public override IWeapon Weapon
    {
        get
        {
            return weapon;
        }
    }

    public override bool AcceptWeapon(IWeapon weapon)
    {
        return true;
    }

    public override bool TryEquip(IWeapon weapon)
    {
        this.weapon = weapon;
        if(weapon != null)
        {
            weapon.transform.SetParent(rack.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
        }
        return true;
    }
}

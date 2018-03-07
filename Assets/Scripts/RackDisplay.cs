using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RackDisplay : ICarrier {

    public override IWeapon Weapon
    {
        get;

        protected set;
    }

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

    public override bool AcceptWeapon(IWeapon weapon)
    {
        return true;
    }
}

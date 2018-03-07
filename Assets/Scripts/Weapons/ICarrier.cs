using UnityEngine;

public abstract class ICarrier : MonoBehaviour
{
    public GameObject hand;

    public abstract IWeapon Weapon { get; protected set; }

    public abstract bool AcceptWeapon(IWeapon weapon);

    public virtual bool TryEquip(IWeapon weapon)
    {
        if(weapon != null)
        {
            weapon.transform.SetParent(hand.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
        }
        Weapon = weapon;
        return true;
    }
}
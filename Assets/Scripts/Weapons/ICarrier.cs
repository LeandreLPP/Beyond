using UnityEngine;

public abstract class ICarrier : MonoBehaviour
{
    public abstract IWeapon Weapon { get; }

    public abstract bool AcceptWeapon(IWeapon weapon);

    public virtual bool TryEquip(IWeapon weapon)
    {
        if(weapon != null)
        {
            weapon.transform.SetParent(transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localEulerAngles = Vector3.zero;
        }
        return true;
    }
}
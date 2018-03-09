using UnityEngine;

public abstract class AbstractCarrier : MonoBehaviour, ICarrier
{
    public GameObject hand;

    public virtual IWeapon Weapon { get; protected set; }

    public abstract bool AcceptWeapon(IWeapon weapon);

    public virtual bool TryEquip(IWeapon weapon)
    {
        var weaponGO = weapon as MonoBehaviour;
        if(weaponGO != null)
        {
            weaponGO.transform.SetParent(hand.transform);
            weaponGO.transform.localPosition = Vector3.zero;
            weaponGO.transform.localEulerAngles = Vector3.zero;
        }
        Weapon = weapon;
        return true;
    }
}

public interface ICarrier
{
    IWeapon Weapon { get; }

    bool AcceptWeapon(IWeapon weapon);

    bool TryEquip(IWeapon weapon);
}
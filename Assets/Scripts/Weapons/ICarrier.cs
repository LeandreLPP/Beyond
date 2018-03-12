using UnityEngine;

public interface ICarrier
{
    AWeapon Weapon { get; }

    bool CanEquip(AWeapon weapon);
    bool CanSwap(AWeapon newWeapon);
    bool Equip(AWeapon weapon);
    AWeapon UnequipWeapon();

    #region Shooter
    void ApplyRecoil(Vector3 recoil);
    #endregion

    #region Melee
    #endregion
}
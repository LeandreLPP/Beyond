using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    RangedWeapon Weapon { get; set; }

    void ApplyRecoil(Vector3 recoil);
}

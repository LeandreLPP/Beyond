using UnityEngine;

public abstract class IWeapon : MonoBehaviour
{
    public abstract ICarrier Carrier { get; set; }
}
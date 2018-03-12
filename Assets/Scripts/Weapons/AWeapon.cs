using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    public virtual ICarrier Carrier { get; set; }
}
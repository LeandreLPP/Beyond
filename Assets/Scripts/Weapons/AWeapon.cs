using UnityEngine;

public abstract class AWeapon : MonoBehaviour
{
    public virtual ACarrier Carrier { get; set; }
}
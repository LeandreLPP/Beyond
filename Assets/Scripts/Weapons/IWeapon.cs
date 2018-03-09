using UnityEngine;

public abstract class AbstractWeapon 
{
    public abstract ICarrier Carrier { get; set; }
}

public interface IWeapon
{
    ICarrier Carrier { get; set; }
}
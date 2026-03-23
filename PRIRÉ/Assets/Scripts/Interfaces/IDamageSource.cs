using UnityEngine;

public interface IDamageSource
{
    DamageType Type { get; }
    float Amount {  get; }
    IAgent? Owner { get; } 
    IWeapon Weapon { get; }
}

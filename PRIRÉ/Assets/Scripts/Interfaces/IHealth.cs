using System;
using UnityEngine;

public interface IHealth
{
    event Action<Vector3, float> OnDamage;
    event Action<Vector3, DamageType> OnDeath;
    

    float MaxHealth { get; }
    float CurrentHealth { get; }
    void TakeDamage(IDamageSource source, Vector3 direction);
    
}

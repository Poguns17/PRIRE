using UnityEngine;

public interface IWeapon
{
    string Name { get; }
    float Damage { get; }
    float FireRate { get; }
    int AmmoCapacity { get; }

    int AmmoCount { get; }

    float ReloadTime { get; }
    float MobilityMultiplier { get; }
    void Shoot();
    void Reload();

}

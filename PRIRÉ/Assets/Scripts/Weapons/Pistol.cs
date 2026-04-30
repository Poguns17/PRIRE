using UnityEngine;

// Semi-auto. High damage per shot, medium ammo.
public class Pistol : BaseWeapon
{
    public override string Name => "Pistol";

    protected override void Start()
    {
        damage = 100f;
        fireRate = 2f;
        ammoCapacity = 17;
        reloadTime = 3.2f;
        mobilityMultiplier = 100f;
        range = 50f;
        base.Start();
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        ammoCount--;
        lastFireTime = Time.time;
        PlaySound(shootSound);
        muzzleFlash?.Play();
        FireRaycast(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.Log($"[Pistol] Fired. Ammo: {ammoCount}/{ammoCapacity}");
    }
}

using UnityEngine;

// Full auto. Fast fire rate, medium damage, large ammo capacity.
public class Rifle : BaseWeapon
{
    public override string Name => "Rifle";

    protected override void Start()
    {
        damage = 25f;
        fireRate = 10f;
        ammoCapacity = 30;
        reloadTime = 2.0f;
        mobilityMultiplier = 85f;
        range = 60f;
        base.Start();
    }

    // Override Update to use GetMouseButton (hold to fire)
    protected override void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Input.GetMouseButton(0)) Shoot();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        ammoCount--;
        lastFireTime = Time.time;
        //PlaySound(shootSound);
        //muzzleFlash?.Play();
        
        FireRaycast(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.Log($"[Rifle] Fired. Ammo: {ammoCount}/{ammoCapacity}");
    }
}

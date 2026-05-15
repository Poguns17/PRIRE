using UnityEngine;

// Fires multiple pellets in a spread. Effective at close range.
public class Shotgun : BaseWeapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private int pelletsPerShot = 8;
    [SerializeField] private float spreadAngle = 10f;

    public override string Name => "Shotgun";

    protected override void Start()
    {
        damage = 20f;
        fireRate = 0.8f;
        ammoCapacity = 6;
        reloadTime = 2.5f;
        mobilityMultiplier = 75f;
        range = 20f;
        base.Start();
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        ammoCount--;
        lastFireTime = Time.time;
        //PlaySound(shootSound);
        //muzzleFlash?.Play();

        // Fire each pellet with a random spread
        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector3 spread = new Vector3(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0f
            );
            FireRaycast(Camera.main.transform.position, Quaternion.Euler(spread) * Camera.main.transform.forward);
        }
        Debug.Log($"[Shotgun] Fired. Ammo: {ammoCount}/{ammoCapacity}");
    }
}

using UnityEngine;

// High damage, long range. Pierces through multiple enemies in a line.
public class SniperRifle : BaseWeapon
{
    public override string Name => "Sniper Rifle";

    protected override void Start()
    {
        damage = 200f;
        fireRate = 0.4f;
        ammoCapacity = 5;
        reloadTime = 3.5f;
        mobilityMultiplier = 60f;
        range = 200f;
        base.Start();
    }

    public override void Shoot()
    {
        if (!CanShoot()) return;
        ammoCount--;
        lastFireTime = Time.time;
        PlaySound(shootSound);
        muzzleFlash?.Play();

        // RaycastAll so the shot pierces through multiple enemies
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, range);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit hit in hits)
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(this, Camera.main.transform.forward);
                Debug.Log($"[Sniper] Hit {hit.transform.name} for {damage} damage.");
            }
        }
        Debug.Log($"[Sniper] Fired. Ammo: {ammoCount}/{ammoCapacity}");
    }
}

using System.Collections;
using UnityEngine;

public class Pistol: MonoBehaviour, IWeapon, IDamageSource
{
    private string name = "Pistol";
    private float damage = 100f;
    private float fireRate = 2f;
    private int ammoCapacity = 17;
    private float reloadTime = 3.2f;
    private float mobilityMultiplier = 100f;
    private bool isReloading = false;
    private float range = 50f;

    private int ammoCount;
    private float lastFireTime;
    void Start()
    {
        ammoCount = ammoCapacity;
    }

    // IWeapon
    public string Name => name;
    public float Damage => damage;
    public float FireRate => fireRate;
    public int AmmoCapacity => ammoCapacity;
    public int AmmoCount => ammoCount;
    public float ReloadTime => reloadTime;
    public float MobilityMultiplier => mobilityMultiplier;


    // IDamageSource
    public DamageType Type => DamageType.Ballistic;
    public float Amount => damage;
    public IAgent? Owner => GetComponentInParent<IAgent>();
    public IWeapon Weapon => this;


    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
        ammoCount = ammoCapacity;
    }

    public void Shoot()
    {
        float timeBetweenShots = 1f/fireRate;
        if (isReloading == true) { return; }
        if (Time.time < lastFireTime + timeBetweenShots){return;}
        if(ammoCount <= 0) { return; }
        
        ammoCount -= 1;
        lastFireTime = Time.time;
       
        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, range))
        {

            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {

                health.TakeDamage(this, direction);
            }
        }
        
    }

    public void Reload()
    {
        if (isReloading) { return; }
        if (ammoCount == ammoCapacity) { return; }
        StartCoroutine(ReloadRoutine());
    }
}

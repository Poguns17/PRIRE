using System.Collections;
using UnityEngine;

// Shared logic for all weapons. Each weapon inherits this and overrides Shoot().


public abstract class BaseWeapon : MonoBehaviour, IWeapon, IDamageSource
{
    [Header("Weapon Stats")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float fireRate = 2f;
    [SerializeField] protected int ammoCapacity = 10;
    [SerializeField] protected float reloadTime = 2f;
    [SerializeField] protected float mobilityMultiplier = 100f;
    [SerializeField] protected float range = 50f;

    [Header("Feedback (optional)")]
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip shootSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected ParticleSystem muzzleFlash;

    protected int ammoCount;
    protected float lastFireTime;
    protected bool isReloading = false;

    // IWeapon
    public abstract string Name { get; }
    public float Damage => damage;
    public float FireRate => fireRate;
    public int AmmoCapacity => ammoCapacity;
    public int AmmoCount => ammoCount;
    public float ReloadTime => reloadTime;
    public float MobilityMultiplier => mobilityMultiplier;

    // IDamageSource
    public virtual DamageType Type => DamageType.Ballistic;
    public float Amount => damage;
    public IAgent? Owner => GetComponentInParent<IAgent>();
    public IWeapon Weapon => this;

    protected virtual void Start()
    {
        ammoCount = ammoCapacity;
    }

    protected virtual void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Input.GetMouseButtonDown(0)) Shoot();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public abstract void Shoot();

    public void Reload()
    {
        if (isReloading) return;
        if (ammoCount == ammoCapacity) return;
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        PlaySound(reloadSound);

        Debug.Log($"{Name} reloading");
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammoCapacity;
        isReloading = false;
        
        Debug.Log($"[{Name}] Reloaded. Ammo: {ammoCount}/{ammoCapacity}");
    }

    // Returns false if weapon cannot fire, triggers reload if empty
    protected bool CanShoot()
    {
        if (isReloading) return false;
        if (Time.time < lastFireTime + 1f / fireRate) return false;
        if (ammoCount <= 0) { Reload(); return false; }
        return true;
    }

    // Fires a single raycast and applies damage if enemy is hit
    protected void FireRaycast(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(this, direction);
                Debug.Log($"[{Name}] Hit {hit.transform.name} for {damage} damage.");
            }
        }
    }

    protected void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}

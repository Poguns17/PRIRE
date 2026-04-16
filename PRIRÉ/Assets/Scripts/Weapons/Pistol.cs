using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon, IDamageSource
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 100f;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private int ammoCapacity = 17;
    [SerializeField] private float reloadTime = 3.2f;
    [SerializeField] private float mobilityMultiplier = 100f;
    [SerializeField] private float range = 50f;

    [Header("Feedback (optional)")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private ParticleSystem muzzleFlash;

    private int ammoCount;
    private float lastFireTime;
    private bool isReloading = false;

    public string Name => "Pistol";
    public float Damage => damage;
    public float FireRate => fireRate;
    public int AmmoCapacity => ammoCapacity;
    public int AmmoCount => ammoCount;
    public float ReloadTime => reloadTime;
    public float MobilityMultiplier => mobilityMultiplier;

    public DamageType Type => DamageType.Ballistic;
    public float Amount => damage;
    public IAgent? Owner => GetComponentInParent<IAgent>();
    public IWeapon Weapon => this;

    private void Start()
    {
        ammoCount = ammoCapacity;
    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (Input.GetMouseButtonDown(0)) Shoot();
        if (Input.GetKeyDown(KeyCode.R)) Reload();
    }

    public void Shoot()
    {
        float timeBetweenShots = 1f / fireRate;
        if (isReloading) return;
        if (Time.time < lastFireTime + timeBetweenShots) return;
        if (ammoCount <= 0) { Reload(); return; }

        ammoCount--;
        lastFireTime = Time.time;
        PlaySound(shootSound);
        muzzleFlash?.Play();

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, range))
        {
            Health health = hit.transform.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(this, direction);
                Debug.Log($"[Pistol] Hit {hit.transform.name} for {damage} damage.");
            }
        }
        Debug.Log($"[Pistol] Fired. Ammo: {ammoCount}/{ammoCapacity}");
    }

    public void Reload()
    {
        if (isReloading) return;
        if (ammoCount == ammoCapacity) return;
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("[Pistol] Reloading...");
        PlaySound(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        ammoCount = ammoCapacity;
        isReloading = false;
        Debug.Log($"[Pistol] Reloaded. Ammo: {ammoCount}/{ammoCapacity}");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}

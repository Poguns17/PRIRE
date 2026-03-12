using UnityEngine;

public class PlayerShoot : MonoBehaviour

{
    [Header("Bullet variables")]
    public float bulletSpeed;
    public float fireRate, bulletDamage;
    public bool isAuto;

    [Header("Initial Setup")]
    public Transform bulletSpawnTransrom;
    public GameObject bulletPrefab;

    private float timer;



	public void Update()
    {
        if(timer > 0)

            {
            timer -= Time.deltaTime / fireRate;
		}


		if (isAuto)
        {
            if (Input.GetButton("Fire")) 

            {
                Shoot(); 
			}

        }
        else
        {
            if (Input.GetButtonDown("Fire1")    && timer <= 0)
			{
                    Shoot();

			}



        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnTransrom.position, Quaternion.identity, GameObject.FindGameObjectsWithTag("WorldObjectHolder")[0].transform);
        bullet.GetComponent<Rigidbody>().AddForce (bulletSpawnTransrom.forward * bulletSpeed, ForceMode.Impulse);
        bullet.GetComponent<Bullet>().damage = bulletDamage;


        timer = 1;  
	}
	}

using UnityEngine;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    [Header("HUD Text")]
    public TMP_Text waveText;
    public TMP_Text xpText;
    public TMP_Text ammoText;
    public TMP_Text reloadText;
    public TMP_Text healthText;
    public TMP_Text hitMarkerText;
    public TMP_Text killMarkerText;

    [Header("Game Values")]
    public int wave = 1;
    public int enemies = 10;
    public int xp = 0;

    public int currentAmmo = 30;

    public int magazineSize = 30;
    public int reserveAmmo = 120;

    public int health = 100;

     void Start()

    {

        reloadText.gameObject.SetActive(false);

        hitMarkerText.gameObject.SetActive(false);

        killMarkerText.gameObject.SetActive(false);

        UpdateHUD();

    }

    void Update()
    {
        // TEST XP
        if (Input.GetKeyDown(KeyCode.X))
        {
            xp += 50;
            UpdateHUD();
        }

        // TEST ENEMY KILL
        if (Input.GetKeyDown(KeyCode.K))
        {
            enemies--;

            xp += 100;

            if (enemies <= 0)
            {
                wave++;
                enemies = 10;
            }

            UpdateHUD();
        }

        // SHOOT
        if (Input.GetMouseButtonDown(0))
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                UpdateHUD();
                StartCoroutine(ShowHitMarker());
            }
        }

        // RELOAD
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        // DAMAGE TEST
        if (Input.GetKeyDown(KeyCode.H))
        {
            health -= 10;

            if (health < 0)
                health = 0;

            UpdateHUD();

            StopCoroutine("RegenerateHealth");
            StartCoroutine("RegenerateHealth");
        }
    }

    void UpdateHUD()
    {
        // Wave + Enemy Count
        waveText.text = "W" + wave + " - " + enemies;

        // XP
        xpText.text = "XP: " + xp;

        // Ammo
        ammoText.text = currentAmmo + " / " + reserveAmmo;

        //  Health Bar
        string healthBar = "";

        int bars = health / 15;

        for (int i = 0; i < bars; i++)
        {
            healthBar += "█";
        }

        healthText.text = "HP " + healthBar;
    }

  IEnumerator Reload()
{
    reloadText.gameObject.SetActive(true);

    yield return new WaitForSeconds(2f);

    currentAmmo = 30;

    reloadText.gameObject.SetActive(false);

    UpdateHUD();
}

    IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(5f);

        while (health < 100)
        {
            health += 5;

            if (health > 100)
                health = 100;

            UpdateHUD();

            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator ShowHitMarker()
{
    hitMarkerText.gameObject.SetActive(true);

    yield return new WaitForSeconds(0.1f);

    hitMarkerText.gameObject.SetActive(false);

}
  IEnumerator ShowKillMarker()

    {

        killMarkerText.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        killMarkerText.gameObject.SetActive(false);

    }

}
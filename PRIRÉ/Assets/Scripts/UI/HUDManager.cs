using UnityEngine;
using TMPro;
using System;
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

    [Header("Wave Settings")]
    public int wave = 1;
    public int enemies = 10;

    // Current weapon
    private BaseWeapon currentWeapon;

    // Player health system
    private PlayerHealth playerHealth;

    void Start()
    {
        // Find player health automatically
        playerHealth = FindObjectOfType<PlayerHealth>();
        
        // Hide markers at start
        reloadText.gameObject.SetActive(false);
        hitMarkerText.gameObject.SetActive(false);
        killMarkerText.gameObject.SetActive(false);

        UpdateHUD();
    }

    void Update()
    {
        // Automatically find active weapon
        BaseWeapon[] weapons = FindObjectsOfType<BaseWeapon>();

        foreach (BaseWeapon weapon in weapons)
        {
            if (weapon.gameObject.activeInHierarchy)
            {
                currentWeapon = weapon;
                break;
            }
        }

        // Reload display
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(ShowReload());
        }

        UpdateHUD();
    }

    void UpdateHUD()
    {
        // Wave + enemies
        waveText.text = "W" + wave + " - " + enemies;

        // XP
        if (XPSystem.Instance != null)
        {
            xpText.text = "XP: " + XPSystem.Instance.TotalXP;
        }
        else
        {
            xpText.text = "XP: 0";
        }

        // Ammo
        if (currentWeapon != null)
        {
            ammoText.text =
                currentWeapon.AmmoCount +
                " / " +
                currentWeapon.AmmoCapacity;
        }
        else
        {
            ammoText.text = "-- / --";
        }

        // Health
        if (playerHealth != null)
        {
            int bars = Mathf.RoundToInt(playerHealth.CurrentHealth / 20f);

            string healthBar = "";

            for (int i = 0; i < bars; i++)
            {
                healthBar += "█";
            }

            healthText.text = "HP " + healthBar;
        }
    }

    IEnumerator ShowReload()
    {
        reloadText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        reloadText.gameObject.SetActive(false);
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

    // Called when enemy is hit
    public void ShowHit()
    {
        StartCoroutine(ShowHitMarker());
    }

    // Called when enemy dies
    public void ShowKill()
    {
        StartCoroutine(ShowKillMarker());
    }

    // Called when enemy dies
    public void EnemyKilled()
    {
        enemies--;

        if (enemies <= 0)
        {
            wave++;
            enemies = 10;
        }

        UpdateHUD();
    }
}
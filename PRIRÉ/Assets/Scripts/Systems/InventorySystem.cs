using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Starting Weapons")]
    [SerializeField] private List<GameObject> Weapons = new List<GameObject>();

    public event Action<string, int, int> OnWeaponChanged;

    private List<GameObject> ownedWeapons = new List<GameObject>();
    private int equippedIndex = 0;

   



    public GameObject CurrentWeapon => ownedWeapons.Count > 0 ? ownedWeapons[equippedIndex] : null;

    private void Start()
    {
        foreach (var weapon in Weapons)
        {
            if (weapon != null) AddWeapon(weapon, equip: false);
            Debug.Log($"{weapon.name} Equipped ");
            if (ownedWeapons.Count > 0) EquipWeapon(0);
        }
    }

    private void Update() { 
    
            float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            EquipWeapon((equippedIndex + 1) % ownedWeapons.Count);
            //Debug.Log($"{weapon.name} equipped");
        }
        else if (scroll < 0f)
        {
            EquipWeapon((equippedIndex - 1 + ownedWeapons.Count) % ownedWeapons.Count);
        }
        
    }

    public void AddWeapon(GameObject weapon, bool equip = true)
    {
        if (weapon == null || ownedWeapons.Contains(weapon)) return;
        ownedWeapons.Add(weapon);
        weapon.SetActive(false);
        Debug.Log($"[Inventory] Added: {weapon.name}");
        if (equip) EquipWeapon(ownedWeapons.Count - 1);
    }

    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= ownedWeapons.Count) return;
        if (ownedWeapons[equippedIndex] != null) ownedWeapons[equippedIndex].SetActive(false);
        equippedIndex = index;
        ownedWeapons[equippedIndex].SetActive(true);
        IWeapon weapon = ownedWeapons[equippedIndex].GetComponent<IWeapon>();
        string weaponName = weapon?.Name ?? ownedWeapons[equippedIndex].name;
        int ammoCount = weapon?.AmmoCount ?? 0;
        int ammoCapacity = weapon?.AmmoCapacity ?? 0;
        OnWeaponChanged?.Invoke(weaponName, ammoCount, ammoCapacity);
        Debug.Log($"[Inventory] Equipped: {weaponName} ({ammoCount}/{ammoCapacity})");
    }
}

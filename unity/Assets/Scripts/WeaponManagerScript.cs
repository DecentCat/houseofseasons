using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManagerScript : MonoBehaviour
{
    // public bool primaryUnlocked = true;
    public bool heavyUnlocked;
    public bool assaultUnlocked;
    public bool shotgunUnlocked;
    public bool laserUnlocked;

    [SerializeField] GameObject weaponInfoUI;
    private WeaponInfoUIScript weaponInfoUIScript;

    private WeaponScript primaryWeapon;
    private WeaponScript heavyWeapon;
    private WeaponScript assaultWeapon;
    private WeaponScript shotgun;
    private LaserScript laser;

    private List<MonoBehaviour> weapons;
    private int selectedWeaponIndex = (int)WeaponType.Primary;

    private Dictionary<WeaponType, MonoBehaviour> weaponDict;


    private void Awake()
    {
        weaponInfoUIScript = weaponInfoUI.GetComponent<WeaponInfoUIScript>();

        // init weapons and laser
        primaryWeapon = gameObject.transform.Find("BasicWeapon").GetComponent<WeaponScript>();
        heavyWeapon = gameObject.transform.Find("HeavyWeapon").GetComponent<WeaponScript>();
        assaultWeapon = gameObject.transform.Find("AssaultWeapon").GetComponent<WeaponScript>();
        shotgun = gameObject.transform.Find("ShotgunWeapon").GetComponent<WeaponScript>();
        laser = gameObject.transform.Find("Laser").GetComponent<LaserScript>();

        // init weapon enum association
        weaponDict = new Dictionary<WeaponType, MonoBehaviour>()
        {
            {WeaponType.Primary, primaryWeapon},
            {WeaponType.Heavy, heavyWeapon},
            {WeaponType.Assault, assaultWeapon},
            {WeaponType.Shotgun, shotgun},
            {WeaponType.Laser, laser},
        };

        // init active weapons and unlock
        weapons = new List<MonoBehaviour>()
        {
            primaryWeapon,
            //heavyWeapon,
            //assaultWeapon,
            //shotgun,
            //laser
        };

        heavyUnlocked |= CrossSceneInformation.PlayerHeavyUnlocked;
        assaultUnlocked |= CrossSceneInformation.PlayerAssaultUnlocked;
        shotgunUnlocked |= CrossSceneInformation.PlayerShotgunUnlocked;
        laserUnlocked |= CrossSceneInformation.PlayerLaserUnlocked;

        // unlock weapons
        if (heavyUnlocked) { weapons.Add(heavyWeapon); }
        if (assaultUnlocked) { weapons.Add(assaultWeapon); }
        if (shotgunUnlocked) { weapons.Add(shotgun); }
        if (laserUnlocked) { weapons.Add(laser); }

        // weaponInfoUIScript.SetWeaponActive(WeaponType.Primary, primaryWeapon.GetBullets);

        // load weapon ammo
        if (CrossSceneInformation.AmmoCount != null)
        {
            LoadAmmoCount(CrossSceneInformation.AmmoCount);
        }
    }

    // get WeaponType by index in Weapon Dictionary
    private WeaponType GetTypeByIndex(int index)
    {
        var weapon = weapons[index];
        WeaponType type = WeaponType.Primary;
        foreach (var pair in weaponDict)
        {
            if (pair.Value == weapon)
            {
                type = pair.Key;
                break;
            }
        }
        return type;
    }

    private void UpdateUI()
    {
        WeaponType type = GetTypeByIndex(selectedWeaponIndex);
        UpdateUI(type);
    }

    private void UpdateUI(WeaponType type)
    {
        object weapon = weapons[selectedWeaponIndex];
        int ammoCount = 0;
        if (weapon is LaserScript)
        {
            ammoCount = (int)((LaserScript)weapon).GetCharge;
        }
        else if (weapon is WeaponScript)
        {
            ammoCount = ((WeaponScript)weapon).GetBullets;
        }
        weaponInfoUIScript.SetWeaponActive(type, ammoCount);
    }

    public void ShootEquippedWeapon(Vector2 dir)
    {
        object weapon = weapons[selectedWeaponIndex];
        if (weapon is LaserScript)
        {
            ((LaserScript)weapon).Shoot();
            weaponInfoUIScript.UpdateAmmoCount((int)((LaserScript)weapon).GetCharge);
        }
        else if (weapon is WeaponScript)
        {
            ((WeaponScript)weapon).Shoot(dir);
            weaponInfoUIScript.UpdateAmmoCount(((WeaponScript)weapon).GetBullets);
        }
    }

    public void SwitchWeaponNext()
    {
        selectedWeaponIndex++; 
        selectedWeaponIndex = selectedWeaponIndex < weapons.Count ? selectedWeaponIndex : 0;

        UpdateUI();
    }

    public void SwitchWeaponPrevious()
    {
        selectedWeaponIndex--;
        selectedWeaponIndex = selectedWeaponIndex >= 0 ? selectedWeaponIndex : weapons.Count - 1;

        UpdateUI();
    }

    public void EquipWeapon(WeaponType weapon)
    {
        //selectedWeaponIndex = (int)weapon;

        for (int idx = 0; idx < weapons.Count; idx++)
        {
            if (weaponDict[weapon] == weapons[idx])
            {
                selectedWeaponIndex = idx;
            }
        }

        UpdateUI(weapon);
    }

    public void UnlockWeapon(WeaponType weapon)
    {
        if (!weapons.Contains(weaponDict[weapon]))
        {
            weapons.Add(weaponDict[weapon]);

            switch (weapon)
            {
                case WeaponType.Heavy:
                    heavyUnlocked = true;
                    break;

                case WeaponType.Assault:
                    assaultUnlocked = true;
                    break;

                case WeaponType.Shotgun:
                    shotgunUnlocked = true;
                    break;

                case WeaponType.Laser:
                    laserUnlocked = true;
                    break;

                default: break;
            }
        }
    }

    public void AddAmmo(WeaponType weaponType, int amount)
    {
        object weapon = weaponDict[weaponType];
        if (weapon is LaserScript)
        {
            ((LaserScript)weapon).Charge(amount);
        }
        else if (weapon is WeaponScript)
        {
            ((WeaponScript)weapon).AddBullets(amount);
        }

        UpdateUI();
    }

    public int[] ExportAmmoCount()
    {
        int[] array = new int[5];
        for (int idx = 0; idx < array.Length; idx++)
        {
            WeaponType type = (WeaponType)idx;
            object weapon = weaponDict[type];
            if (weapon is LaserScript)
            {
                array[idx] = (int)((LaserScript)weapon).charge;
            }
            else if (weapon is WeaponScript)
            {
               array[idx] = ((WeaponScript)weapon).bullets;
            }
        }
        return array;
    }

    public void LoadAmmoCount(int[] array)
    {
        for (int idx = 0; idx < array.Length; idx++)
        {
            WeaponType type = (WeaponType)idx;
            object weapon = weaponDict[type];
            if (weapon is LaserScript)
            {
                ((LaserScript)weapon).charge = array[idx];
            }
            else if (weapon is WeaponScript)
            {
                ((WeaponScript)weapon).bullets = array[idx];
            }
        }
    }
}

public enum WeaponType
{
    Primary = 0,
    Heavy = 1,
    Assault = 2,
    Shotgun = 3,
    Laser = 4
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManagerScript : MonoBehaviour
{
    public enum Weapon
    {
        Primary = 0,
        Heavy   = 1,
        Assault = 2,
        Shotgun = 3,
        Laser   = 4
    }

    // public bool primaryUnlocked = true;
    public bool heavyUnlocked;
    public bool assaultUnlocked;
    public bool shotgunUnlocked;
    public bool laserUnlocked;

    private WeaponScript primaryWeapon;
    private WeaponScript heavyWeapon;
    private WeaponScript assaultWeapon;
    private WeaponScript shotgun;
    private LaserScript laser;

    private List<MonoBehaviour> weapons;
    private int selectedWeaponIndex = (int)Weapon.Primary;

    private Dictionary<Weapon, MonoBehaviour> weaponDict;


    private void Awake()
    {
        // init weapons and laser
        primaryWeapon = gameObject.transform.Find("BasicWeapon").GetComponent<WeaponScript>();
        heavyWeapon = gameObject.transform.Find("HeavyWeapon").GetComponent<WeaponScript>();
        assaultWeapon = gameObject.transform.Find("AssaultWeapon").GetComponent<WeaponScript>();
        shotgun = gameObject.transform.Find("ShotgunWeapon").GetComponent<WeaponScript>();
        laser = gameObject.transform.Find("Laser").GetComponent<LaserScript>();

        // init weapon enum association
        weaponDict = new Dictionary<Weapon, MonoBehaviour>()
        {
            {Weapon.Primary, primaryWeapon},
            {Weapon.Heavy, heavyWeapon},
            {Weapon.Assault, assaultWeapon},
            {Weapon.Shotgun, shotgun},
            {Weapon.Laser, laser},
        };

        // init active weapons
        weapons = new List<MonoBehaviour>()
        {
            primaryWeapon,
            //heavyWeapon,
            //assaultWeapon,
            //shotgun,
            //laser
        };

        if (heavyUnlocked) { weapons.Add(heavyWeapon); }
        if (assaultUnlocked) { weapons.Add(assaultWeapon); }
        if (shotgunUnlocked) { weapons.Add(shotgun); }
        if (laserUnlocked) { weapons.Add(laser); }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootEquippedWeapon(Vector2 dir)
    {
        object weapon = weapons[selectedWeaponIndex];
        if (weapon is LaserScript)
        {
            ((LaserScript)weapon).Shoot();
        }
        else if (weapon is WeaponScript)
        {
            ((WeaponScript)weapon).Shoot(dir);
        }
    }

    public void SwitchWeaponNext()
    {
        selectedWeaponIndex++; 
        selectedWeaponIndex = selectedWeaponIndex < weapons.Count ? selectedWeaponIndex : 0;
    }

    public void SwitchWeaponPrevious()
    {
        selectedWeaponIndex--;
        selectedWeaponIndex = selectedWeaponIndex >= 0 ? selectedWeaponIndex : weapons.Count - 1;
    }

    public void EquipWeapon(Weapon weapon)
    {
        //selectedWeaponIndex = (int)weapon;

        for (int idx = 0; idx < weapons.Count; idx++)
        {
            if (weaponDict[weapon] == weapons[idx])
            {
                selectedWeaponIndex = idx;
            }
        }
    }

    public void UnlockWeapon(Weapon weapon)
    {
        if (!weapons.Contains(weaponDict[weapon]))
        {
            weapons.Add(weaponDict[weapon]);

            switch (weapon)
            {
                case Weapon.Heavy:
                    heavyUnlocked = true;
                    break;

                case Weapon.Assault:
                    assaultUnlocked = true;
                    break;

                case Weapon.Shotgun:
                    shotgunUnlocked = true;
                    break;

                case Weapon.Laser:
                    laserUnlocked = true;
                    break;

                default: break;
            }
        }
    }

    public void AddAmmo(Weapon weaponType, int amount)
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    public PickupType type;
    public WeaponManagerScript.Weapon weaponType;
    public int amount;

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
            WeaponManagerScript weaponManager = collision.gameObject.GetComponent<WeaponManagerScript>();
            switch (type)
            {
                case PickupType.Healing:
                    player.HealDamage(amount);
                    break;

                case PickupType.Ammo:
                    weaponManager.AddAmmo(weaponType, amount);
                    break;

                case PickupType.WeaponUnlock:
                    weaponManager.UnlockWeapon(weaponType);
                    break;

                default: break;
            }
        }
    }
}

public enum PickupType
{
    Healing,
    Ammo,
    WeaponUnlock
}

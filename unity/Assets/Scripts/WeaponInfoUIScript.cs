using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUIScript : MonoBehaviour
{
    public Sprite primarySprite;
    public Sprite heavySprite;
    public Sprite assaultSprite;
    public Sprite shotgunSprite;
    public Sprite laserSprite;

    private Image activeIcon;
    private TextMeshProUGUI activeText;

    private WeaponType activeWeapon = WeaponType.Primary;

    private void Awake()
    {
        activeIcon = gameObject.transform.Find("WeaponIcon").GetComponent<UnityEngine.UI.Image>();
        activeText = gameObject.transform.Find("AmmoCountText").GetComponent<TextMeshProUGUI>();
    }

    public void SetWeaponActive( WeaponType type, int ammoCount )
    {
        activeWeapon = type;
        switch (activeWeapon)
        {
            case WeaponType.Primary:
                activeIcon.sprite = primarySprite;
                break;

            case WeaponType.Heavy:
                activeIcon.sprite = heavySprite;
                break;

            case WeaponType.Assault:
                activeIcon.sprite = assaultSprite;
                break;

            case WeaponType.Shotgun:
                activeIcon.sprite = shotgunSprite;
                break;

            case WeaponType.Laser:
                activeIcon.sprite = laserSprite;
                break;

            default: break;
        }
        UpdateAmmoCount(ammoCount);
    }

    public void UpdateAmmoCount(int count)
    {
        if ( count < 0)
        {
            activeText.text = "+99";
        }
        else if (activeWeapon == WeaponType.Laser)
        {
            activeText.text = count.ToString() + "%";
        }
        else
        {
            activeText.text = count.ToString();
        }
    }
}

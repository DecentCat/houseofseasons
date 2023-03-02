using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;

    public void SetUiHealth(int health)
    {
        slider.value = health;
    }

    public void SetUiMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Slider slider;
    // Start is called before the first frame update
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;

    }
    public void SetHealth(float health)
    {
        slider.value = health;
    }
    public void SetValue(float maxHealth, float health)
    {
        slider.maxValue = maxHealth;
        slider.value = health;
    }
}

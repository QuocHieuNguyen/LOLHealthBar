using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public float recordedHealth;
    public List<Shielding> shields = new List<Shielding>();
    public HealthBarUI healthBarUI;

    public Action onShieldGain;
    public Action onAllShieldLost;
    public Action onShieldPossesion;
    public Action onHealthValueChanged;
    public Action onHealthValueAssign;
    public Action onNewMaxHealthAssign;
    public Action TestAction; //Test Only
    public float CurrentShield()
    {
        float value = 0;
        for (int i = 0; i < shields.Count; i++)
        {
            value += shields[i].shieldAmount;
        }
        
        return value;
    }
    public float TotalLife()
    {
        return currentHealth + CurrentShield();
    }
    public bool IsDeath()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            return true;
        }else
        {
            return false;
        }
    }
    public void AssignStartedValue(float _maxHealth)
    {
        maxHealth = _maxHealth;
        currentHealth = maxHealth;
        AssignRecordedHealth();
        healthBarUI.SetValue(_maxHealth, _maxHealth);
        onHealthValueAssign?.Invoke();
    }
    public void AssignNewMaxHealth(float health)
    {
        maxHealth = health;
        healthBarUI.SetMaxHealth(health);
        onNewMaxHealthAssign?.Invoke();

    }
    public void AssignNewValue(float _currentHealth, float _maxHealth)
    {
        currentHealth = _currentHealth;
        maxHealth = _maxHealth;
        AssignRecordedHealth();
        healthBarUI.SetValue(_maxHealth, _currentHealth);
        onNewMaxHealthAssign?.Invoke();
    }
    public void TakeDamage(float damage)
    {
        if(CurrentShield() >= damage)
        {
            //CurrentShield() -= damage;
            List<Shielding> newShieldsList = new List<Shielding>(shields);
            for (int i = newShieldsList.Count - 1; i >= 0; i--)
            {
                if(damage >= newShieldsList[i].shieldAmount)
                {
                    
                    damage -= newShieldsList[i].shieldAmount;
                    newShieldsList[i].shieldAmount = 0;
                    shields.Remove(newShieldsList[i]);
                }else
                {
                    shields[i].shieldAmount -= damage;
                    break;
                }
            }           
        }
        if(damage > CurrentShield() && CurrentShield() > 0)
        {
            float value = damage - CurrentShield();
            currentHealth -= value;
            damage = 0;
            shields.Clear();
        }
        if(CurrentShield() <= 0 && currentHealth > 0 && damage > 0) 
        {
            currentHealth -= damage;
        }
        healthBarUI.SetHealth(currentHealth);
        IsDeath();
    }
    public void GainHealth(float health)
    {
        if(currentHealth < maxHealth)
        {
            if(currentHealth + health >= maxHealth)
            {
                currentHealth = maxHealth;
            }else
            {
                currentHealth += health;
            }
        }
        //TestAction?.Invoke();
        healthBarUI.SetHealth(currentHealth);
        //onHealthValueChanged?.Invoke();
    }
    public void AddShieldBehavior(Shielding shielding)
    {
        shields.Add(shielding);
        if(onShieldGain != null)
        onShieldGain();
    }

    void UpdateThroughListOfShielding()
    {
        for (int i = 0; i < shields.Count; i++)
        {
            shields[i].Update();
        }
        if(shields.Count > 0 && onShieldPossesion != null)
        {
            onShieldPossesion();

        }
    }
    void EliminateOutOfAmountShield()
    {
        List<Shielding> newList = new List<Shielding>(shields);
        for (int i = 0; i < newList.Count; i++)
        {
            if(newList[i].shieldAmount <= 0)
            {
                shields.Remove(newList[i]);
            }
        }
        if(shields.Count == 0 && onAllShieldLost != null)
        {
            onAllShieldLost();
        }
    }
    void AssignRecordedHealth()
    {
        recordedHealth = Mathf.Floor(TotalLife());
    }
    private void Update() {
        UpdateThroughListOfShielding();
        EliminateOutOfAmountShield();
        //Debug.Log("Update");
        //Debug.Log("Current Shield " + CurrentShield());
        if(Mathf.Abs(recordedHealth - TotalLife()) > 1f)
        {
            onHealthValueChanged?.Invoke();
            AssignRecordedHealth();
        }
    }

    
}

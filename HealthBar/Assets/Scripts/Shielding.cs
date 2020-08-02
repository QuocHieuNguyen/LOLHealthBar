using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Shielding 
{
    public HealthSystem _healthSystem;
    public float shieldAmount;
    public Shielding()
    {
        //shieldAmount = givenShield;
    }
    public Shielding(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        AddShieldBehavior(healthSystem);
        //shieldAmount = givenShield;
    }
    public virtual void Update()
    {

    }
    public void AddShieldBehavior(HealthSystem healthSystem)
    {
        healthSystem.AddShieldBehavior((Shielding)this);
        
    }

}

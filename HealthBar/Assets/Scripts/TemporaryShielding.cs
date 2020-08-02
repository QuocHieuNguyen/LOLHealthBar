using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TemporaryShielding : Shielding 
{
    public float time;
    public Action finishingGainShield;
    // Start is called before the first frame update
    public TemporaryShielding()
    {

    }
    public TemporaryShielding(float shieldAmount, float timeAmount, HealthSystem healthSystem) :base(healthSystem)
    {
        GainShieldForPeriod(shieldAmount, timeAmount);
    }
    public void GainShieldForPeriod(float shield, float amount)
    {
        shieldAmount += shield;
        time = amount;
        //MonoBehaviour.StartCoroutine(ShieldingPeriodically(shield, amount));
    }
    public override void Update()
    {
        time -= Time.deltaTime;
        if(time < 0)
        {
            if(shieldAmount > 0)
            {
                shieldAmount = 0;
                //_healthSystem.TestAction?.Invoke();
            }
        }
    }
}

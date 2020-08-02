using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingShielding : Shielding
{
    public float time;
    public FadingShielding()
    {

    }
    public FadingShielding(float shieldAmount, float timeAmount, HealthSystem healthSystem) :base(healthSystem)
    {
        GainShieldForPeriod(shieldAmount, timeAmount);
    }
    public void GainShieldForPeriod(float shield, float amountOfTime)
    {
        shieldAmount += shield;
        time = amountOfTime;
        
        //MonoBehaviour.StartCoroutine(ShieldingPeriodically(shield, amount));
    }
    public override void Update()
    {  
        if(shieldAmount > 0.1f)
        {
            shieldAmount -= Mathf.Clamp(Mathf.Clamp(shieldAmount/(time / Time.deltaTime), 0.1f, float.MaxValue),0, float.MaxValue);
            //_healthSystem.TestAction?.Invoke();
            //time += 0.5f * Time.deltaTime;
        }else
        {
            shieldAmount = 0;
            //_healthSystem.TestAction?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentShielding : Shielding 
{
    public PermanentShielding(float amount, HealthSystem healthSystem) :base(healthSystem)
    {
        shieldAmount += amount;
    }
    public PermanentShielding()
    {

    }
    public void GainShieldPermanently(float amount)
    {
        shieldAmount += amount;
    }
}

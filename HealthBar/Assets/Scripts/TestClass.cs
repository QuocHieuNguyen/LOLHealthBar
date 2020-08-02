using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public HealthSystem healthSystem1, healthSystem2, healthSystem3;
    public float damage;
    public float shieldGain;
    public Transform canvas;
    public GameObject healthBarPrefab;
    // Start is called before the first frame update
    void Start()
    {
        healthSystem1.AssignStartedValue(240);
        healthSystem2.AssignStartedValue(240);
        healthSystem3.AssignStartedValue(240);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            healthSystem1.TakeDamage(damage);
            healthSystem2.TakeDamage(damage);
            healthSystem3.TakeDamage(damage);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            healthSystem1.GainHealth(damage);
            healthSystem2.GainHealth(damage);
            healthSystem3.GainHealth(damage);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            healthSystem1.AssignNewMaxHealth(healthSystem1.maxHealth + 10);
            healthSystem2.AssignNewMaxHealth(healthSystem2.maxHealth + 10);
            healthSystem3.AssignNewMaxHealth(healthSystem3.maxHealth + 10);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //healthSystem1.GainShieldPermanently(20);
            Shielding shield = new PermanentShielding(shieldGain, healthSystem1);
            Shielding shield2 = new PermanentShielding(shieldGain, healthSystem2);
            Shielding shield3 = new PermanentShielding(shieldGain, healthSystem3);
            //shield.AddShieldBehavior(healthSystem1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Shielding shield = new TemporaryShielding(shieldGain, 2f, healthSystem1);
            Shielding shield2 = new TemporaryShielding(shieldGain, 2f, healthSystem2);
            Shielding shield3 = new TemporaryShielding(shieldGain, 2f, healthSystem3);
            //shield.AddShieldBehavior(healthSystem1);
            //healthSystem1.GainShieldForPeriod(30, 2f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {

            Shielding shield = new FadingShielding(shieldGain, 2f, healthSystem1);
            Shielding shield2 = new FadingShielding(shieldGain, 2f, healthSystem2);
            Shielding shield3 = new FadingShielding(shieldGain, 2f, healthSystem3);
            //shield.AddShieldBehavior(healthSystem);
            //healthSystem.GainFadingShield(35, 10f);
        }
    }
    public void TakeDamage()
    {
        healthSystem1.TakeDamage(damage);
        healthSystem2.TakeDamage(damage);
        healthSystem3.TakeDamage(damage);
    }
    public void GainHealth()
    {
        healthSystem1.GainHealth(damage);
        healthSystem2.GainHealth(damage);
        healthSystem3.GainHealth(damage);
    }
    public void PShield()
    {
        Shielding shield = new PermanentShielding(shieldGain, healthSystem1);
        Shielding shield2 = new PermanentShielding(shieldGain, healthSystem2);
        Shielding shield3 = new PermanentShielding(shieldGain, healthSystem3);
    }
    public void TShield()
    {
        Shielding shield = new TemporaryShielding(shieldGain, 2f, healthSystem1);
        Shielding shield2 = new TemporaryShielding(shieldGain, 2f, healthSystem2);
        Shielding shield3 = new TemporaryShielding(shieldGain, 2f, healthSystem3);
    }
    public void FShield()
    {
        Shielding shield = new FadingShielding(shieldGain, 2f, healthSystem1);
        Shielding shield2 = new FadingShielding(shieldGain, 2f, healthSystem2);
        Shielding shield3 = new FadingShielding(shieldGain, 2f, healthSystem3);
    }
    void CreateHealthbar()
    {
        GameObject hs = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, canvas) as GameObject;
        HealthSystem h = hs.GetComponent<HealthSystem>();
        h.AssignStartedValue(100f);
        Shielding shield = new PermanentShielding(20f, h);
    }
}

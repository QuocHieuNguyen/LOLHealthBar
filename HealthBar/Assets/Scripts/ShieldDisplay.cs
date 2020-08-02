using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldDisplay : MonoBehaviour
{
    public HealthSystem healthSystem;
    public Image shieldDisplayUI;
    public Transform front, rear;
    Vector3 shieldDisplayOriginalPositon;

    // Start is called before the first frame update
    void Start()
    {
        //healthSystem.onShieldGain += DisplayShield;
        healthSystem.onShieldPossesion += DisplayShield;
        healthSystem.onAllShieldLost += UnableShieldUI;
        shieldDisplayOriginalPositon = shieldDisplayUI.gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayShield()
    {
        float totalLife = healthSystem.TotalLife();
        float maxHealth = healthSystem.maxHealth;
        float spriteWidth =Mathf.Abs(rear.position.x - front.position.x);
        Vector3 shieldDisplayScale = shieldDisplayUI.GetComponent<RectTransform>().localScale;
        Vector3 shieldDisplayPosition = shieldDisplayUI.transform.position;
        float shieldFraction = 0f;
        if(IsFullHealth())
        {
            shieldDisplayUI.gameObject.SetActive(false);
            shieldFraction = healthSystem.CurrentShield()/healthSystem.TotalLife();           
            shieldDisplayUI.transform.position = new Vector3(rear.position.x -(spriteWidth)*shieldFraction, shieldDisplayPosition.y, shieldDisplayPosition.z);
            shieldDisplayUI.GetComponent<RectTransform>().localScale =
                new Vector2(shieldFraction,shieldDisplayScale.y);
            shieldDisplayUI.gameObject.SetActive(true);
        }else
        {
            if(totalLife == maxHealth)
            {
                Debug.Log("===");
                shieldDisplayUI.gameObject.SetActive(true);
                shieldDisplayUI.GetComponent<RectTransform>().localScale =
                    new Vector2(healthSystem.CurrentShield()/healthSystem.maxHealth,
                                shieldDisplayScale.y);
                float intersectionPointX = healthSystem.healthBarUI.gameObject.GetComponent<Slider>().handleRect.position.x;
                shieldDisplayUI.transform.position = new Vector3(intersectionPointX, shieldDisplayPosition.y, shieldDisplayPosition.z);               
            }
            else if (totalLife > maxHealth)
            {;
                shieldDisplayUI.gameObject.SetActive(false);            
                shieldFraction = healthSystem.CurrentShield()/healthSystem.TotalLife();
                shieldDisplayUI.transform.position = 
                new Vector3(rear.position.x -(spriteWidth)*shieldFraction, shieldDisplayPosition.y, shieldDisplayPosition.z);
                shieldDisplayUI.GetComponent<RectTransform>().localScale =
                    new Vector2(shieldFraction,
                                shieldDisplayScale.y);
                shieldDisplayUI.gameObject.SetActive(true);                
                    //Create a new max health
                    //change pivot point
            }else
            {
                shieldDisplayUI.gameObject.SetActive(true);
                float intersectionPointX = healthSystem.healthBarUI.gameObject.GetComponent<Slider>().handleRect.position.x;
                shieldDisplayUI.transform.position = new Vector3(intersectionPointX, shieldDisplayPosition.y, shieldDisplayPosition.z);
                shieldDisplayUI.GetComponent<RectTransform>().localScale =
                    new Vector2(healthSystem.CurrentShield()/healthSystem.maxHealth,
                                shieldDisplayUI.GetComponent<RectTransform>().localScale.y);
            }
        }
    }
    public void UnableShieldUI()
    {        
        shieldDisplayUI.gameObject.SetActive(false);
        shieldDisplayUI.transform.position = shieldDisplayOriginalPositon;
    }

    public bool IsFullHealth()
    {
        if(healthSystem.currentHealth == healthSystem.maxHealth)
        {
            return true;
        }else return false;
    }
}

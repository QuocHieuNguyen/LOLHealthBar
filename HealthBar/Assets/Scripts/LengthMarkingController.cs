using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LengthMarkingController : MonoBehaviour
{
    float standardLengthMarkingUnit = 100f;
    public HealthSystem healthSystem;
    public Transform healthBarFront, healthBarRear;
    public GameObject lengthMarkingPrefab;
    public Transform lengthMarkingPosition, lengthMarkingInstanceParent;
    Vector3 handle;

    public List<LengthMarking> lengthMarkings = new List<LengthMarking>();
    LengthMarking lastLengthMarking;
    float TotalLife()
    {
        return healthSystem.TotalLife();
    }
    float MaxHealth()
    {
        return healthSystem.maxHealth;
    }
    float recordedLife;
    // Start is called before the first frame update
    void Start()
    {
        handle = healthSystem.healthBarUI.gameObject.GetComponent<Slider>().handleRect.position;

        //healthSystem.onHealthValueChanged += UpdateLengthMarking;
        healthSystem.onHealthValueChanged += UpdateLengthMarking;
        healthSystem.onHealthValueAssign += LengthMarkingInitialization;
        healthSystem.onNewMaxHealthAssign += LengthMarkingReinitialization;
        InvokeRepeating("AssignRecordedHealth", 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void AssignRecordedHealth()
    {
        recordedLife = TotalLife();
    }
    void CreateLengthMarkingObject(float currentValue, float maxValue)
    {
        int wholeLengthMarking = Mathf.FloorToInt(currentValue / standardLengthMarkingUnit);
        float extraPart = Mathf.FloorToInt(currentValue) % standardLengthMarkingUnit;
        float spriteWidth = Mathf.Abs(healthBarRear.position.x - healthBarFront.position.x);
        Debug.Log("w " + wholeLengthMarking + " " + "s " + extraPart );
        if (extraPart > 0)
        {
            wholeLengthMarking += 1;
        }
        for (int i = 0; i < wholeLengthMarking; i++)
        {
            GameObject o = (GameObject)Instantiate(lengthMarkingPrefab, lengthMarkingPosition.position, Quaternion.identity, lengthMarkingInstanceParent);
            o.name = "Length Marking " + i;
            LengthMarking lengthMarking = new LengthMarking(o.transform.GetChild(0).gameObject,
                                                            o.transform.GetChild(1).gameObject,
                                                            o);
            lengthMarking.image.GetComponent<RectTransform>().localScale =
                 new Vector2(standardLengthMarkingUnit/maxValue,
                            lengthMarking.image.GetComponent<RectTransform>().localScale.y);
            if (lengthMarkings.Count > 0)
            {
                o.transform.position = new Vector3(lengthMarkings[i - 1].rear.transform.position.x, o.transform.position.y, o.transform.position.z);
            }
            lengthMarkings.Add(lengthMarking);

            if (i == wholeLengthMarking - 1)
            {
                lastLengthMarking = lengthMarking;
                lastLengthMarking.image.fillAmount =
                    (currentValue - standardLengthMarkingUnit * (wholeLengthMarking - 1)) / standardLengthMarkingUnit;
            }
        }
        AssignRecordedHealth();
    }
    void LengthMarkingInitialization()
    {
        CreateLengthMarkingObject(TotalLife(), TotalLife());

    }
    void LengthMarkingReinitialization()
    {
        if(TotalLife() < MaxHealth())
        {
            for (int i = 0; i < lengthMarkings.Count; i++)
            {
                Destroy(lengthMarkings[i].lengthMarkingObject);
            }
            lengthMarkings.Clear();
            CreateLengthMarkingObject(TotalLife(), MaxHealth());
        }
        AssignRecordedHealth();

    }
    void UpdateLengthMarking()
    {
        float difference = recordedLife - TotalLife();
        float distance = Mathf.Abs(difference);
        float lastLengthMarkingRatio = lastLengthMarking.image.fillAmount;
        float quotient = distance / standardLengthMarkingUnit;

        if (recordedLife > TotalLife())
        {
            if (lastLengthMarking.image.fillAmount * standardLengthMarkingUnit <= distance)
            {
                distance -= lastLengthMarking.image.fillAmount * standardLengthMarkingUnit;
                PopLengthMarkingsPeek();
                List<LengthMarking> copyList = new List<LengthMarking>(lengthMarkings);
                for (int i = copyList.Count - 1; i >= 0; i--)
                {
                    if (distance < standardLengthMarkingUnit)
                    {
                        lengthMarkings[i].image.fillAmount = 1 - distance / standardLengthMarkingUnit;
                        lastLengthMarking = lengthMarkings[i];
                        break;
                    }
                    else
                    {
                        distance -= standardLengthMarkingUnit;
                        Destroy(lengthMarkings[i].lengthMarkingObject);
                        quotient -= 1;
                        lengthMarkings.RemoveAt(i);
                    }
                }
                RescaleLengthMarking(MaxHealth());

            }
            else
            {
                lastLengthMarking.image.fillAmount = (float)Math.Round(lastLengthMarking.image.fillAmount - 
                   distance / standardLengthMarkingUnit, 2); //some case almost 0

                if (lastLengthMarking.image.fillAmount < 0.05)
                {
                    PopLengthMarkingsPeek();
                }
            }
            if (TotalLife() >= MaxHealth()) RescaleLengthMarking(TotalLife());
        }
        else if (recordedLife < TotalLife())
        {
            if ((1 - lastLengthMarking.image.fillAmount) * standardLengthMarkingUnit > distance)
            {
                lastLengthMarking.image.fillAmount = (float)Math.Round
                    (lastLengthMarking.image.fillAmount + distance / standardLengthMarkingUnit, 2);
            }
            else
            {
                distance -= (1 - lastLengthMarking.image.fillAmount) * standardLengthMarkingUnit;
                lastLengthMarking.image.fillAmount = 1;
                quotient = Mathf.CeilToInt(distance / standardLengthMarkingUnit);
                int amount = lengthMarkings.Count;
                for (int i = 0; i < quotient; i++)
                {
                    GameObject o = (GameObject)Instantiate
                                (lengthMarkingPrefab, lengthMarkingPosition.position, Quaternion.identity, lengthMarkingInstanceParent);
                    o.name = "Length Marking " + (i + amount);
                    LengthMarking lengthMarking = new LengthMarking(o.transform.GetChild(0).gameObject,
                                                                    o.transform.GetChild(1).gameObject,
                                                                    o);
                    lengthMarking.image.GetComponent<RectTransform>().localScale =
                        new Vector2(standardLengthMarkingUnit / MaxHealth(),
                                    lengthMarking.image.GetComponent<RectTransform>().localScale.y);
                    lengthMarkings.Add(lengthMarking);
                    if (i == 0)
                        o.transform.position =
                            new Vector3(lastLengthMarking.rear.transform.position.x, o.transform.position.y, o.transform.position.z);
                    else o.transform.position =
                        new Vector3(lengthMarkings[amount + i - 1].rear.transform.position.x, o.transform.position.y, o.transform.position.z);
                    if (i == quotient - 1)
                    {
                        lastLengthMarking = lengthMarking;
                        lastLengthMarking.image.fillAmount = distance / standardLengthMarkingUnit;
                    }
                    else
                        distance -= standardLengthMarkingUnit;

                }
            }
            if (TotalLife() >= MaxHealth()) RescaleLengthMarking(TotalLife());
            
        }
        AssignRecordedHealth();
    }
    void RescaleLengthMarking(float length)
    {
        for (int i = 0; i < lengthMarkings.Count; i++)
        {
            lengthMarkings[i].image.GetComponent<RectTransform>().localScale =
                new Vector2(standardLengthMarkingUnit / length,
                            lengthMarkings[i].image.GetComponent<RectTransform>().localScale.y);
            if (i == 0)
            {
                lengthMarkings[i].lengthMarkingObject.transform.position = lengthMarkingPosition.position;
            }
            else
            {
                Vector3 temp = lengthMarkings[i].lengthMarkingObject.transform.position;
                lengthMarkings[i].lengthMarkingObject.transform.position =
                    new Vector3(lengthMarkings[i - 1].rear.transform.position.x, temp.y, temp.z);
            }
        }
    }
    void PopLengthMarkingsPeek()
    {
        Destroy(lastLengthMarking.lengthMarkingObject);
        lengthMarkings.Remove(lastLengthMarking);
        if (lengthMarkings.Count >= 1)
            lastLengthMarking = lengthMarkings[lengthMarkings.Count - 1];
    }

    
    [System.Serializable]
    public class LengthMarking
    {
        public GameObject front;
        public GameObject rear;
        public GameObject lengthMarkingObject;
        public Image image;

        public LengthMarking(GameObject _front, GameObject _rear, GameObject _object)
        {
            front = _front;
            rear = _rear;
            lengthMarkingObject = _object;
            if (_object.GetComponent<Image>()) image = _object.GetComponent<Image>();
        }
    }
}

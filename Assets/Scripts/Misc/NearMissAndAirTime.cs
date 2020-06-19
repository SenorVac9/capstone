using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.CustomVehiclePhysics;


public class NearMissAndAirTime : MonoBehaviour
{
    public GameObject nearMissText , airTimeText;
    public Text airTimeCounter;
    public bool isCollided = false;
    public bool isNearMissActive = false;
    public float airTimeGainRate = 12.5f;
    public float nitrogain = 0;
    public float nearMissGain = 20;
    
    public VehicleBehavior vehicleBehavior;
    // Start is called before the first frame update
    void Start()
    {
     
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
        nearMissText.SetActive(false);
        airTimeText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (airTimeCounter == null) return;
        if (vehicleBehavior == null) return;
        //NearMissCall(vehicleBehavior.max_nitros_meter_float);
        if (vehicleBehavior.nitros_meter_float > 100)
        {
            vehicleBehavior.nitros_meter_float = 100;
        }

        vehicleBehavior.nitros_meter_float = AirTimeNitroBoost(vehicleBehavior.nitros_meter_float, nitrogain, airTimeGainRate);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Obstacle"))
        {
            isCollided = true;
            Debug.Log("collided");
            StartCoroutine("WaitForSec");
        }
    }

    void OnTriggerExit(Collider c)
    {

        if (c.gameObject.CompareTag("Obstacle") && !isCollided)
        {
            Debug.Log("exited trigger");
            Debug.Log("isCollided" + isCollided);
            if(isCollided)
            {
                StartCoroutine("WaitForSec");
            }
            else if(!isCollided)
            {
                //isNearMissActive = true;
                NearMissCall();
            }
        }
    }

    void NearMissCall()
    {

        /*
        if (vehicleBehavior.nitros_meter_float < 100)
        {
            vehicleBehavior.nitros_meter_float += 10;

        }*/

        vehicleBehavior.nitros_meter_float = vehicleBehavior.nitros_meter_float + nearMissGain;

        nearMissText.SetActive(true);
        isCollided = false;
        //isNearMissActive = false;
        StartCoroutine("WaitForSec");
        
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(2);
        nearMissText.SetActive(false);
        isCollided = false;
    }
    
    float AirTimeNitroBoost(float nitro, float gain, float rate)
    {
        if (!vehicleBehavior.is_grounded)
        {
            float countr = Time.deltaTime;
            if (countr >= 0.5)
            {
                airTimeText.SetActive(true);
                gain = Time.deltaTime * rate;
                nitro += gain;
            }
            airTimeCounter.text = (countr).ToString();
            nitro += gain;
            
        }
        else
        {
            airTimeText.SetActive(false) ;
        }
        return nitro;
    }
    
}

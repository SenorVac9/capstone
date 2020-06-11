using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ModuloKart.CustomVehiclePhysics;

public class MilkSpill : MonoBehaviour
{
    private void Update()
    {
        if(transform.localScale.x < 30.5f)
        transform.localScale += new Vector3(0.1f, 0.0f, 0.1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            //Call spin out function
            Debug.Log("Hit Milk");
            //other.GetComponent<VehicleBehavior>().StartSpinOut();
            Destroy(gameObject);
        }
    }

   
}

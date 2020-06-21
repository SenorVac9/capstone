using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;


public class Nitro_effect : MonoBehaviour
{
   
    VehicleBehavior vehicleBehavior;
    public ParticleSystem nitro;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (vehicleBehavior.is_nitrosboost==true)
        {
            nitro.Stop();
            if (!nitro.isEmitting)
            {
                nitro.Play();
                Debug.Log("nitro effect playing N");
            }
           


        }
        else if (vehicleBehavior.is_nitrosboost==false)
                {
            nitro.Stop();

        }
    }
}

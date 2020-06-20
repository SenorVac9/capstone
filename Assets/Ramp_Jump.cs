using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;


public class Ramp_Jump : MonoBehaviour
{
    VehicleBehavior vehiclebehavior;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    int cnt = 0;
    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag=="GameController")
        {
            vehiclebehavior = c.gameObject.GetComponent<VehicleBehavior>();
          
                vehiclebehavior.Jump();
          
        }
    }
}

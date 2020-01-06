using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
{
    public Transform spawnpoint;// setPoint;
    public GameObject prefab1,ramp;
    public Camera cam_p1;
    VehicleBehavior vehicleBehavior;
    public float speed;
    public GameObject hood_destroy;
    public int count = 0;

        
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();

    }

    // Update is called once per frame
    void Update()
    {

        speed = 3000 + (vehicleBehavior.accel_magnitude_float   );

    }
  
   /* public void OnCollisionEnter(Collision c)
{
        if(c.gameObject.tag=="Track")
            Instantiate(ramp, transform.position, Quaternion.Euler(50f, 0f, -90f));
    }*/
    public void Throw_Hood()
    {
      
            
               
                    prefab1.SetActive(true);
                    GameObject Hood = Instantiate(prefab1) as GameObject; count++;
                    Hood.transform.position = spawnpoint.transform.position;
                    Rigidbody rb = Hood.GetComponent<Rigidbody>();
                    rb.velocity = cam_p1.transform.forward * speed;
                    //Destroy(prefab1);
                   // prefab1.SetActive(false);
                    hood_destroy.SetActive(false);
           
                // flag = false;
                //rb.transform.position = setPoint.transform.position * speed ;             //cam_p1.transform.forward *((vehicleBehavior.accel_magnitude_float)+ 1000 )
            

        
    }

}

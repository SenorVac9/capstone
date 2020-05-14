using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
{
    public Transform spawnpoint, MilkSpawnPoint;// setPoint;
    public GameObject prefab1, ramp, MilkSpill;
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

    public void Throw_Milk()
    {
        Debug.Log("milk really thrown");
        GameObject Milk = Instantiate(MilkSpill, MilkSpawnPoint.transform);
        Milk.transform.parent = null;
        Milk.transform.position = MilkSpawnPoint.position;

    }
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

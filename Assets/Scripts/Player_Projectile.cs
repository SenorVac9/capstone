using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Projectile : MonoBehaviour
{
    public Transform spawnpoint,  MilkSpawnPoint;// setPoint;
    //private Transform target;
    public GameObject prefab1, ramp, MilkSpill;
    public Camera cam_p1;
    VehicleBehavior vehicleBehavior;
    public float speed;
    public GameObject hood_destroy;
    public float force = 100f;
    public int count = 0;

        
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
        //target.transform.position = spawnpoint.transform.position + new Vector3(0, 0, 100);

    }

    // Update is called once per frame
    void Update()
    {

        speed = 4f + (vehicleBehavior.accel_magnitude_float/100);

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
        GameObject Hood = Instantiate(prefab1, spawnpoint.position, spawnpoint.rotation) as GameObject;
        //  Hood.transform.position = Vector3.MoveTowards(hood_destroy.transform.position, target.transform.position, speed*Time.deltaTime);

        Hood.GetComponent<Rigidbody>().AddForce(spawnpoint.forward * force * speed, ForceMode.Impulse);
        //Rigidbody rb = Hood.GetComponent<Rigidbody>();
        //  rb.velocity = cam_p1.transform.forward * speed;
        //Destroy(prefab1);
        //prefab1.SetActive(false);
        hood_destroy.SetActive(false);

        // flag = false;
        //rb.transform.position = setPoint.transform.position * speed ;             //cam_p1.transform.forward *((vehicleBehavior.accel_magnitude_float)+ 1000 )




    }

}

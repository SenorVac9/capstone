using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Door_Detach : MonoBehaviour
{
    public Transform Spawnpoint_door1, Spawnpoint_door2;
    public GameObject Prefab1;
    public Camera cam_p1;
    VehicleBehavior vehicleBehaviour;
    public float speed = 0.5f;
    //public GameObject Door_destory1, Door_destroy2;
   // public int[] reserveParts;
    //public List<int> reservePartsList = new List<int>();
    //public int partsUsed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Detach_Door(int doornum)
    {
        //1 left door
        //2 right door
        if(doornum==1)
        {
            GameObject Door = Instantiate(Prefab1) as GameObject;
            Door.transform.position = Spawnpoint_door1.transform.position;
            Rigidbody rb = Door.GetComponent<Rigidbody>();
            rb.velocity = cam_p1.transform.forward * -speed;
           // partsUsed++;
            //reservePartsList.Add(1);
            //Door_destroy1.SetACtive(false);
            //Debug.log(reservePartsList);

         }

        else if(doornum==2)
        {
            GameObject Door = Instantiate(Prefab1) as GameObject;
            Door.transform.position = Spawnpoint_door2.transform.position;
            Rigidbody rb = Door.GetComponent<Rigidbody>();
            rb.velocity = cam_p1.transform.forward * speed;
            //partsUsed++;
            //reservePartsList.Add(2);
            //Door_destroy1.SetACtive(false);
            //Debug.log(reservePartsList);


        }
    }
}

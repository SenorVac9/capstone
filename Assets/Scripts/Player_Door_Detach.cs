using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Door_Detach : MonoBehaviour
{

    public Transform Spawnpoint_door1, Spawnpoint_door2;
    public GameObject Prefab1, door_left, door_right;
    public Camera cam_p1;
    VehicleBehavior vehicleBehaviour;
    float speed = 50f;
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
       
        if (doornum == 1)
        {
         
            GameObject Door = Instantiate(Prefab1) as GameObject;
            Door.transform.position = Spawnpoint_door1.transform.position;
            Rigidbody rb = Door.GetComponent<Rigidbody>();
            rb.velocity = Spawnpoint_door1.transform.forward * speed;
            // partsUsed++;
            //reservePartsList.Add(1);
            door_left.SetActive(false);
            //Debug.log(reservePartsList);

        }

        else if (doornum == 2)
        {
           
            GameObject Door = Instantiate(Prefab1) as GameObject;
            Door.transform.position = Spawnpoint_door2.transform.position;
            Rigidbody rb = Door.GetComponent<Rigidbody>();
            rb.velocity = Spawnpoint_door2.transform.forward * speed;
            //partsUsed++;
            //reservePartsList.Add(2);
            door_right.SetActive(false);
            //Debug.log(reservePartsList);


        }
    }

}

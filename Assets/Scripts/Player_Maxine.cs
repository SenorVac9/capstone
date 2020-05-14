using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Maxine : MonoBehaviour
{
    public Transform Maxine_part_spawnpoint;
    public GameObject Prefab1;
    public Camera cam_p1;
    VehicleBehavior vehicleBehaviour;
    //public GameObject ui_part_extra;
    public float speed = 2f;
    //public int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehaviour = GameObject.FindObjectOfType<VehicleBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        //Maxine_Extrapart();
    }
    public void Maxine_Extrapart()
    {
        
       
            GameObject Extrapart = Instantiate(Prefab1) as GameObject;
            Extrapart.transform.position = Maxine_part_spawnpoint.transform.position;
            Rigidbody rb = Extrapart.GetComponent<Rigidbody>();
            rb.velocity = cam_p1.transform.forward * -speed;
            //ui_part_extra.SetActive(false);
        
    }
}

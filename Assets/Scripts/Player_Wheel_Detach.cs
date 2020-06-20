using ModuloKart.CustomVehiclePhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Wheel_Detach : MonoBehaviour
{
    public Transform spawnpoint1, spawnpoint2, spawnpoint3, spawnpoint4;// setPoints;
    public GameObject prefab1;
    public Camera cam_p1;
    VehicleBehavior vehicleBehavior;
   int speed = 50;
    public GameObject wheel_destroy1, wheel_destroy2, wheel_destroy3, wheel_destroy4;
    public int[] reserveParts;
    public List<int> reservePartsList = new List<int>();
    public int partsUsed;
    Vehicle_Collisions _vehicleCollisions;


    // Start is called before the first frame update

    void Start()
    {
        _vehicleCollisions = GameObject.FindObjectOfType<Vehicle_Collisions>();
    }

    // Update is called once per frame
    void Update()
    {


    }


    public void Throw_Wheel(int tirenum)
    {

        //AUDIO STUFF
        //Calling the audio based on the character selected
        //It checks which character has been selected and play accordingly
        //IF Maxime
        //if (vehicleBehavior.playerHUD.simpleCharacterSeleciton.whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Maxine)
        //{
        //    AudioManager.instance.Play("Maxime_Throw_Tire");
        //}

        ////IF Felix
        //if (vehicleBehavior.playerHUD.simpleCharacterSeleciton.whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Felix)
        //{
        //    AudioManager.instance.Play("Felix_Throw_Tire");
        //}

        ////IF Paul
        //if (vehicleBehavior.playerHUD.simpleCharacterSeleciton.whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Paul)
        //{
        //     AudioManager.instance.Play("Paul_Throw_Tire");
        //}

        ////IF Toby
        //if (vehicleBehavior.playerHUD.simpleCharacterSeleciton.whichCharacterDidISelectDuringTheGameScene == AVerySimpleEnumOfCharacters.Toby)
        //{
        //    AudioManager.instance.Play("Toby_Throw_Tire");
        //}

        /* 
         1 back left
         2 back right 
         3 front left
         4 front right
         */
        if (tirenum == 2)
        {
            _vehicleCollisions.CollisionDisabler();
            GameObject Wheel = Instantiate<GameObject>(prefab1, spawnpoint1.transform.position, spawnpoint1.rotation);
          
            Rigidbody rb = Wheel.GetComponent<Rigidbody>();
            rb.velocity = spawnpoint1.transform.forward * speed;
            partsUsed++;
            
            reservePartsList.Add(2);
            wheel_destroy1.SetActive(false);
            Debug.Log(reservePartsList);
        }
        else if (tirenum == 1)
        {
            _vehicleCollisions.CollisionDisabler();
            GameObject Wheel = Instantiate<GameObject>(prefab1, spawnpoint2.transform.position, spawnpoint2.rotation);
            Rigidbody rb = Wheel.GetComponent<Rigidbody>();
            rb.velocity = spawnpoint2.transform.forward * speed;
            reservePartsList.Add(1);
            Debug.Log(reservePartsList);
            wheel_destroy2.SetActive(false);
            partsUsed++;
        }
        else if (tirenum == 4)
        {
            _vehicleCollisions.CollisionDisabler();
            GameObject Wheel = Instantiate<GameObject>(prefab1, spawnpoint3.transform.position, spawnpoint3.rotation);
            Rigidbody rb = Wheel.GetComponent<Rigidbody>();
            rb.velocity = spawnpoint3.transform.forward * speed;
            reservePartsList.Add(4);
            Debug.Log(reservePartsList);
            wheel_destroy3.SetActive(false);
            partsUsed++;
        }
        else if (tirenum == 3)
        {
            _vehicleCollisions.CollisionDisabler();
            GameObject Wheel = Instantiate<GameObject>(prefab1, spawnpoint4.transform.position, spawnpoint4.rotation);
            Rigidbody rb = Wheel.GetComponent<Rigidbody>();
            rb.velocity = spawnpoint4.transform.forward * speed;
            reservePartsList.Add(3);
            Debug.Log(reservePartsList);
            wheel_destroy4.SetActive(false);
            partsUsed++;
        }
    }

}

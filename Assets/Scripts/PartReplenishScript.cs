using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.Controls;
using ModuloKart.CustomVehiclePhysics;

public class PartReplenishScript : MonoBehaviour
{

    //variables needed
    bool retry;
    public GameObject player;
   
    // public GameObject hud;
    int partBack;
    public VehicleBehavior car;
    public Player_Wheel_Detach wheels;
    public ui_controller headsUp;
    Collider ThisCollider;
    PickUpSpawner spawner;
    //I had to make the bools in "ui_controller.cs" public


    // Start is called before the first frame update
    void Start()
    {
       
        Debug.Log("Randomizer Active");
        spawner = gameObject.GetComponentInParent<PickUpSpawner>();
  
    }


    
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "GameController")
        {
            ThisCollider = c;
            player = c.gameObject;
            car = player.GetComponentInChildren<VehicleBehavior>();
            wheels = player.GetComponentInChildren<Player_Wheel_Detach>();
            headsUp = c.gameObject.GetComponentInChildren<VehicleBehavior>().playerHUD.GetComponentInChildren<ui_controller>();
            if (headsUp)
                Debug.Log("HEADS UP: " + headsUp);
            else
                Debug.Log("NO HEADS UP");
            Debug.Log("Cody Test");

        }


        //get the scripts for the players
        //VehicleBehavior car = player.GetComponentInChildren<VehicleBehavior>();
        //Player_Wheel_Detach wheels = player.GetComponentInChildren<Player_Wheel_Detach>();
        //ui_controller headsUp = hud.GetComponentInChildren<ui_controller>();
        retry = true;
        // while (retry)
        //  {
        if (headsUp)
        {
            if (headsUp.has_tire_1 == true && headsUp.has_tire_2 == true && headsUp.has_tire_3 == true && headsUp.has_tire_4 == true && headsUp.has_hood == true)
            {
                spawner.Timer = Time.time + 5.0f;
                gameObject.SetActive(false);
                //if this is true, then it nullifies the script
                return;
            }
            if (c.gameObject.CompareTag("GameController"))
            {


                partBack = Random.Range(0, wheels.reservePartsList.Count);
                if (wheels.reservePartsList[partBack] == 2)
                {
                    car.wheel4.SetActive(true);
                    wheels.wheel_destroy1.SetActive(true);
                    //    headsUp.has_tire_2 = true;
                    wheels.reservePartsList.Remove(2);
                    //  headsUp.ui_item[0].gameObject.SetActive(true);
                    headsUp.RegainPart(5);

                    Debug.Log("FR wheel replenished");
                }
                else if (wheels.reservePartsList[partBack] == 1)
                {
                    car.wheel3.SetActive(true);
                    wheels.wheel_destroy2.SetActive(true);
                    //  headsUp.has_tire_1 = true;
                    wheels.reservePartsList.Remove(1);
                    Debug.Log("FL wheel replenished");
                    // headsUp.ui_item[2].gameObject.SetActive(true);
                    headsUp.RegainPart(7);
                }
                else if (wheels.reservePartsList[partBack] == 4)
                {

                    car.wheel2.SetActive(true);
                    wheels.wheel_destroy3.SetActive(true);
                    // headsUp.has_tire_4 = true;
                    wheels.reservePartsList.Remove(4);
                    Debug.Log("RL wheel replenished");
                    // headsUp.ui_item[5].gameObject.SetActive(true);
                    headsUp.RegainPart(0);
                }
                else if (wheels.reservePartsList[partBack] == 3)
                {
                    car.wheel1.SetActive(true);
                    wheels.wheel_destroy4.SetActive(true);
                    //   headsUp.has_tire_3 = true;
                    wheels.reservePartsList.Remove(3);
                    Debug.Log("RR wheel replenished");
                    //    headsUp.ui_item[7].gameObject.SetActive(true);
                    headsUp.RegainPart(2);
                }
                else
                {
                    Debug.Log("Not supposed to show");
                    //  break;
                }
                retry = false;

            }
            else
            {
                //it needs to hit a specific part of the car, otherwise, this activates
                Debug.Log("Collider Not Hit");
                return;
            }
        }
        else
        {
            Debug.Log("We have no 'Heads up' Object");
            return;
        }
        spawner.Timer = Time.time + 5.0f;
        gameObject.SetActive(false);

    }

   
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.Controls;
using ModuloKart.CustomVehiclePhysics;

public class PartReplenishScript : MonoBehaviour
{

    //variables needed
   
    public GameObject player;
   
    // public GameObject hud;
    int partBack;
    public VehicleBehavior car;
    public Player_Wheel_Detach wheels;
    public ui_controller headsUp;
    public Material CharMaterial;
    public Material TireMaterial;
    public Material NitroMaterial;
    float nitroPickUp =25;
    Collider ThisCollider;
    PickUpSpawner spawner;
    PickUpType upType = PickUpType.Tires;
    //I had to make the bools in "ui_controller.cs" public


     //I'm setting up different types of pickups
     public enum PickUpType
    {
        Tires ,
        Character,
         Nitro
    }

    // Start is called before the first frame update
    void Start()
    {
       
        Debug.Log("Randomizer Active");
        spawner = gameObject.GetComponentInParent<PickUpSpawner>();
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                setPickUpType(PickUpType.Tires);
                break;
            case 1:
                setPickUpType(PickUpType.Nitro);
                break;
            case 2:
                setPickUpType(PickUpType.Character);
                break;
        }
  
    }
    public void setPickUpType(PickUpType type)
    {
        upType = type;
        switch (upType)
        {
            case PickUpType.Character:
                gameObject.GetComponent<MeshRenderer>().material = CharMaterial;
                break;
            case PickUpType.Nitro:
                gameObject.GetComponent<MeshRenderer>().material = NitroMaterial;
                break;
            case PickUpType.Tires:
                gameObject.GetComponent<MeshRenderer>().material = TireMaterial;
                break;
        }
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
      
        // while (retry)
        //  {
        if (headsUp)
        {
            AVerySimpleEnumOfCharacters character = headsUp.GetCharacter();
            if (upType == PickUpType.Tires)
            {
            if (headsUp.has_tire_1 == true && headsUp.has_tire_2 == true && headsUp.has_tire_3 == true && headsUp.has_tire_4 == true )
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

                    spawner.Timer = Time.time + 5.0f;
                    gameObject.SetActive(false);
                }
            else
            {
                //it needs to hit a specific part of the car, otherwise, this activates
                Debug.Log("Collider Not Hit");
                return;
            }
            }
           else if(upType == PickUpType.Nitro)
            {
                if (c.gameObject.CompareTag("GameController"))
                {
                    
                    if(car.nitros_meter_float + nitroPickUp > car.max_nitros_meter_float)
                    {
                        if(character == AVerySimpleEnumOfCharacters.Felix || character == AVerySimpleEnumOfCharacters.Toby)
                        {
                            float dif = car.max_nitros_meter_float - car.nitros_meter_float;
                            car.nitros_meter_float = car.max_nitros_meter_float;
                            car.extra_nitros_meter_float += (nitroPickUp - dif);
                            if(car.extra_nitros_meter_float > 100 && character == AVerySimpleEnumOfCharacters.Felix)
                            {                        
                                    car.extra_nitros_meter_float = 100;
                               
                                if (car.extra_nitros_meter_float > 50 && character == AVerySimpleEnumOfCharacters.Toby)
                                    car.extra_nitros_meter_float = 50;
                            }
                        }
                        else
                        car.nitros_meter_float = car.max_nitros_meter_float;
                    }
                    else
                    {
                        car.nitros_meter_float += nitroPickUp; 
                    }
                    spawner.Timer = Time.time + 5.0f;
                    gameObject.SetActive(false);
                }
               
            }
            else if(upType == PickUpType.Character)
            {
               
                switch (character){
                    case AVerySimpleEnumOfCharacters.Felix:
                        if (headsUp.has_door_1 == true && headsUp.has_Shield == true  && headsUp.has_door_2 == true)
                        {
                            spawner.Timer = Time.time + 5.0f;
                            gameObject.SetActive(false);
                            //if this is true, then it nullifies the script
                            return;
                        }
                        break;
                    case AVerySimpleEnumOfCharacters.Maxine:
                        if (headsUp.has_door_1 == true  && headsUp.has_extra1 == true && headsUp.has_extra2 == true && headsUp.has_door_2 == true)
                        {
                            spawner.Timer = Time.time + 5.0f;
                            gameObject.SetActive(false);
                            //if this is true, then it nullifies the script
                            return;
                        }
                        break;
                    case AVerySimpleEnumOfCharacters.Paul:
                        if (headsUp.has_door_1 == true && headsUp.has_Shield == true && headsUp.has_door_2 == true)
                        {
                            spawner.Timer = Time.time + 5.0f;
                            gameObject.SetActive(false);
                            //if this is true, then it nullifies the script
                            return;
                        }
                        break;
                    case AVerySimpleEnumOfCharacters.Toby:
                        if (headsUp.has_door_1 == true  && headsUp.has_extra1 == true  && headsUp.has_door_2 == true)
                        {
                            spawner.Timer = Time.time + 5.0f;
                            gameObject.SetActive(false);
                            //if this is true, then it nullifies the script
                            return;
                        }
                        break;
                }
              
                if (c.gameObject.CompareTag("GameController"))
                {
                    bool retry = true;
                   
                    partBack = Random.Range(0, 5);
                    while (retry == true)
                    {
                        
                        switch (partBack)
                        {
                            case 0:
                                if (headsUp.has_door_1)
                                {
                                    partBack++;
                                }
                                else
                                {
                                    headsUp.RegainPart(3);
                                    retry = false;
                                }
                                break;
                            case 1:
                                if (headsUp.has_door_2)
                                {
                                    partBack++;
                                }
                                else
                                {
                                    headsUp.RegainPart(4);
                                    retry = false;
                                }

                                break;
                            case 2:
                                if (headsUp.has_Shield||character != AVerySimpleEnumOfCharacters.Paul)
                                {
                                    partBack++;
                                }
                                else
                                {
                                    headsUp.has_Shield = true;
                                    retry = false;
                                }

                                break;
                            case 3:
                                if (headsUp.has_extra1|| character == AVerySimpleEnumOfCharacters.Felix || character == AVerySimpleEnumOfCharacters.Paul||character == AVerySimpleEnumOfCharacters.NotInGame)
                                {
                                    partBack++;
                                }
                                else
                                {
                                    headsUp.RegainPart(8);
                                    retry = false;
                                }
                                break;
                            case 4:
                                if (headsUp.has_extra2||character != AVerySimpleEnumOfCharacters.Maxine)
                                {
                                    partBack = 0;
                                 //   retry = false;
                                }
                                else
                                {
                                    headsUp.RegainPart(9);
                                    retry = false;
                                }

                                break;
                        }
                    }
                    spawner.Timer = Time.time + 5.0f;
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("We have no 'Heads up' Object");
            return;
        }
        

    }

   
}


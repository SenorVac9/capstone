using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.Controls;
using ModuloKart.CustomVehiclePhysics;

public class ui_controller : MonoBehaviour
{
   public GameObject[] ui_item;
   public int item_selected;
     VehicleBehavior vehicleBehaviour;

    public bool has_door_1 = true; //front right
     public bool has_door_2 = true;
    public bool has_tire_1 = true;
    public bool has_tire_2 = true;
    public bool has_tire_3 = true;
    public bool has_tire_4 = true;
    public bool has_hood = true;
    public bool has_Milk = true;
    public bool has_Maxine_extra = true;
    public int playerNum;
    public GameObject vechicle;
    public GameObject wheeldetacher;
    
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehaviour = GameObject.FindObjectOfType<VehicleBehavior>();
        if (gameObject.tag == "Player1")
            playerNum = 1;
        else if (gameObject.tag == "Player2")
            playerNum = 2;
        else if (gameObject.tag == "Player3")
            playerNum = 3;
        else if (gameObject.tag == "Player4")
            playerNum = 4;

        item_selected = 0;
        ui_item = new GameObject[9];
        int i = 0;
        
        while (i < 9)
        {
            ui_item[i] = gameObject.transform.GetChild(i).gameObject;
            if (ui_item[i] == null)
                throw new System.Exception("ui_item " + i + " not set");
            i++;
        }







    }

 public   bool allItemsGone()
    {
        if (!has_door_1 && !has_door_2 && !has_hood && !has_Maxine_extra && !has_Milk && !has_tire_1 && !has_tire_2 && !has_tire_3 && !has_tire_4)
        {
            return true;
        }

        return false;
    }
    public void RegainPart(int partRecovered)
    {
        ui_item[partRecovered].gameObject.SetActive(true);
        switch ((partRecovered))
        {
            case 0:
                has_tire_1 = true;
                break;
            case 2:
                has_tire_2 = true;
                break;
            case 5:
                has_tire_3 = true;
                break;
            case 7:
                has_tire_4 = true;
                break;
            case 6:
                has_Milk = true;
                break;
            case 3:
                has_door_1 = true;
                break;
            case 4:
                has_door_2 = true;
                break;
            case 8:
                has_Maxine_extra = true;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if (!vechicle.GetComponent<VehicleBehavior>().isControllerInitialized) return;
        if (!vechicle.GetComponent<VehicleBehavior>().playerHUD.simpleCharacterSeleciton.isCharacterSelected) return;

        if (Input.GetButtonDown(vechicle.GetComponent<VehicleBehavior>().input_ItemNext))
        {
            Debug.Log("TEST NEXT INPUT");
            Debug.Log("before" + item_selected);
            ui_item[item_selected].transform.localScale -= new Vector3(0.5f, 0.5f);


            item_selected -= 1;
            if (item_selected < 0)

            {
                item_selected = 8;
            }

            while (ui_item[item_selected].gameObject.activeSelf == false && !allItemsGone())
            {
                item_selected -= 1;
                if (item_selected < 0)
                {
                    item_selected = 8;
                }
            }



            ui_item[item_selected].transform.localScale += new Vector3(0.5f, 0.5f);

            Debug.Log("after" + item_selected);

        }
        else if (Input.GetButtonDown(vechicle.GetComponent<VehicleBehavior>().input_ItemPrev))
        {
            Debug.Log("TEST PREV INPUT");
            ui_item[item_selected].transform.localScale -= new Vector3(0.5f, 0.5f);


            item_selected += 1;


            if (item_selected >= 9)
                item_selected = 0;
            while (ui_item[item_selected].gameObject.activeSelf == false && !allItemsGone())
            {
                item_selected += 1;
                if (item_selected >= 9)
                {
                    item_selected = 0;
                }
            }



            ui_item[item_selected].transform.localScale += new Vector3(0.5f, 0.5f);


        }

        if (Input.GetButtonDown(vechicle.GetComponent<VehicleBehavior>().input_projectile))
        {
            Debug.Log("TEST SHOOT INPUT");


            if (ui_item[item_selected].gameObject.tag == "Tire")
            {
                if (item_selected == 0)
                {
                    if (has_tire_1)
                    {
                        vechicle.GetComponentInChildren<Player_Wheel_Detach>().Throw_Wheel(4);
                        Debug.Log("used front right tire");
                        has_tire_1 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 2)
                {
                    if (has_tire_2)
                    {
                        vechicle.GetComponentInChildren<Player_Wheel_Detach>().Throw_Wheel(3);
                        Debug.Log("used front left tire");
                        has_tire_2 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 5)
                {
                    if (has_tire_3)
                    {
                        vechicle.GetComponentInChildren<Player_Wheel_Detach>().Throw_Wheel(2);
                        Debug.Log("used back right tire");
                        has_tire_3 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 7)
                {
                    if (has_tire_4)
                    {
                        vechicle.GetComponentInChildren<Player_Wheel_Detach>().Throw_Wheel(1);
                        Debug.Log("used back left tire");
                        has_tire_4 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }


            }
            else if (ui_item[item_selected].gameObject.tag == "Door")
            {

                if (item_selected == 3)
                {
                    if (has_door_1)
                    {
                        vechicle.GetComponentInChildren<Player_Door_Detach>().Detach_Door(1);

                        Debug.Log("used left door");
                        has_door_1 = false;
                        ui_item[item_selected].gameObject.SetActive(false);
                    }
                    else
                        Debug.Log("door already used");
                }

                if (item_selected == 4)
                {
                    if (has_door_2)
                    {
                        vechicle.GetComponentInChildren<Player_Door_Detach>().Detach_Door(2);
                        Debug.Log("used right door");
                        has_door_2 = false;
                        ui_item[item_selected].gameObject.SetActive(false);
                    }
                }
                    else
                        Debug.Log("door already used");
                
            }
            else if (ui_item[item_selected].gameObject.tag == "Hood")
            {
                if (has_hood)
                {
                    vechicle.GetComponentInChildren<Player_Projectile>().Throw_Hood();
                    Debug.Log("used hood");
                    has_hood = false;
                    //call hood item function use here
                }
                else if (!has_hood)
                {
                    Debug.Log("No hood left fool");
                    //play sound? or other notification
                }
            }
            else if (ui_item[item_selected].gameObject.tag == "Milk")
            {
                if (has_Milk)
                {

                    vechicle.GetComponentInChildren<Player_Projectile>().Throw_Milk();
                    has_Milk = false;
                    Debug.Log("Used Milk");
                    ui_item[item_selected].gameObject.SetActive(false);
                }


            }   
            else if (ui_item[item_selected].gameObject.tag== "Maxine_Extra_Parts")
            {
                if(has_Maxine_extra)
                {
                    vehicleBehaviour.GetComponentInChildren<Player_Maxine>().Maxine_Extrapart();
                    Debug.Log("Used Maxine spl");
                    has_Maxine_extra = false;
                    ui_item[item_selected].gameObject.SetActive(false);
                }
            }

            ui_item[item_selected].SetActive(false);
            ui_item[item_selected].transform.localScale -= new Vector3(0.5f, 0.5f);
            item_selected -= 1;
            if (item_selected < 0)
            {
                item_selected = 8;
            }
            while (ui_item[item_selected].gameObject.activeSelf == false && !allItemsGone())
            {
                item_selected -= 1;
                if (item_selected < 0)
                {
                    item_selected = 8;
                }
            }


            ui_item[item_selected].transform.localScale += new Vector3(0.5f, 0.5f);

        }


    }

}

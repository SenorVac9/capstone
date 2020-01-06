using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_controller : MonoBehaviour
{
    GameObject[] ui_item;
    int item_selected;
   
    bool has_door_1 = true;
    bool has_door_2 = true;
    bool has_tire_1 = true;
    bool has_tire_2 = true;
    bool has_tire_3 = true;
    bool has_tire_4 = true;
    bool has_hood = true;


    // Start is called before the first frame update
    void Start()
    {
        item_selected = 0;
        ui_item = new GameObject[7];
        int i = 0;

        while (i < 7)
        {
            ui_item[i] = gameObject.transform.GetChild(i).gameObject;
            if (ui_item[i] == null)
                throw new System.Exception("ui_item " + i + " not set");
            i++;
        }

       


        


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
           // Debug.Log("before" + item_selected);
             ui_item[item_selected].transform.localScale -= new Vector3(0.2f, 0.2f);
            

            item_selected += 1;
            if (item_selected >= 7)
                item_selected = 0;

            ui_item[item_selected].transform.localScale += new Vector3(0.2f, 0.2f);
          

            //Debug.Log("after" + item_selected);

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (ui_item[item_selected].gameObject.tag == "tire")
            {
                if (item_selected == 0)
                {
                    if (has_tire_1)
                    {
                        Debug.Log("used front left tire");
                        has_tire_1 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 2)
                {
                    if (has_tire_2)
                    {
                        Debug.Log("used front right tire");
                        has_tire_2 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 5)
                {
                    if (has_tire_3)
                    {
                        Debug.Log("used back left tire");
                        has_tire_3 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }

                if (item_selected == 6)
                {
                    if (has_tire_4)
                    {
                        Debug.Log("used back right tire");
                        has_tire_4 = false;
                    }
                    else
                        Debug.Log("tire already used");
                }


            }
            else if (ui_item[item_selected].gameObject.tag == "door")
            {

                if (item_selected == 3)
                {
                    if (has_door_1)
                    {
                        Debug.Log("used left door");
                        has_door_1 = false;
                    }
                    else
                        Debug.Log("door already used");
                }

                if (item_selected == 4)
                {
                    if (has_door_2)
                    {
                        Debug.Log("used right door");
                        has_door_2 = false;
                    }
                    else
                        Debug.Log("door already used");
                }
            }
            else if(ui_item[item_selected].gameObject.tag == "hood")
            {
                if (has_hood)
                {
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
        }




    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.Controls;
using ModuloKart.CustomVehiclePhysics;

public class milkPickUP : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            if (other.gameObject.GetComponentInChildren<VehicleBehavior>().playerHUD.GetComponentInChildren<ui_controller>().has_Milk == false)
            {
                other.gameObject.GetComponentInChildren<VehicleBehavior>().playerHUD.GetComponentInChildren<ui_controller>().RegainPart(6);
                Destroy(gameObject);
            }
        }
    }
}

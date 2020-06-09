using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public class DeathPosition : MonoBehaviour
{
    public Transform startPos;

    private GameObject vehicleObj;
    private Transform respawnTransform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            //other.gameObject.transform.position = GameManager.Instance.lastCheckpoint.position;
            vehicleObj = other.gameObject;
            respawnTransform = vehicleObj.GetComponent<VehicleLapData>().GetRespawnTransform();
            other.transform.position = respawnTransform.position;

            vehicleObj.GetComponent<VehicleBehavior>().accel_magnitude_float = 0;

            vehicleObj.transform.forward = respawnTransform.forward;
            other.GetComponent<VehicleBehavior>().vehicle_heading_transform.forward = respawnTransform.forward;
            //Debug.DrawRay(transform.position, transform.position + transform.right + vehicleObj.transform.forward, Color.red, 10);
            //Debug.DrawRay(transform.position, transform.position - transform.right + transform.forward, Color.blue, 10);
            other.GetComponent<VehicleBehavior>().vehicle_heading_transform.rotation = Quaternion.Euler(0, other.GetComponent<VehicleBehavior>().vehicle_heading_transform.rotation.y, 0);



            //other.gameObject.transform.position = GameManager.Instance.lastCheckpoint.position;
        }
    }
}

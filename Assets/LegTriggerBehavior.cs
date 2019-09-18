using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public enum LegId
{
    Zero = 0,
    One = 1,
    Two = 2,
    Three = 3,
}

public class LegTriggerBehavior : MonoBehaviour
{
    public bool isDebugMode;
    public LegId legID;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            if (Vector3.Dot(other.GetComponent<VehicleBehavior>().vehicle_heading_transform.forward, transform.forward) > 0)
            {
                if (isDebugMode) Debug.Log("Going in the correct direction");

                other.GetComponent<VehicleBehavior>().playerHUD.WrongDirectionWarning.SetActive(false);

                if (other.GetComponent<VehicleLapData>().currentLegID < legID)
                {
                    if (other.GetComponent<VehicleLapData>().currentLegID.Equals(LegId.Zero) && legID.Equals(LegId.Three))
                    {
                        //currentPlayerLeg 0 < (Final Leg = 3), so we make playerCurrentLeg = 3
                        other.GetComponent<VehicleLapData>().currentLegID = legID;
                    }
                    else
                    {
                        //Increment Leg Condition
                        if (other.GetComponent<VehicleLapData>().currentLegID + 1 == legID)
                        {
                            //The LapManager Component Dictates the Final Leg of the Lap, thus scoring a Lap. See Lap Manager for Lap incrementation.
                            other.GetComponent<VehicleLapData>().currentLegID = legID;
                            if (isDebugMode) Debug.Log("We are Now at LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                        }
                    }

                }

            }
            else
            {
                if (isDebugMode) Debug.Log("WRONG direction");
                //Check if Player Reaches Leg==3, and decides to backtrack to Legs 2, then 1, then 'Scores'
                //We cannot allow this to happen
                //So... This resets currentLeg Completed to back to legID of the LegTriggerData
                other.GetComponent<VehicleBehavior>().playerHUD.WrongDirectionWarning.SetActive(true);

                if (other.GetComponent<VehicleLapData>().currentLegID == legID)
                {
                    other.GetComponent<VehicleLapData>().currentLegID = legID - 1;
                    if (isDebugMode) Debug.Log("Going Back to LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                }
                return;
            }


        }
    }

}

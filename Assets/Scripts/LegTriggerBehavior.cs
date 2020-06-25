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
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9, 
    Ten = 10,
    Eleven = 11,
    Twelve = 12,
    Thirteen = 13,
}

public class LegTriggerBehavior : MonoBehaviour
{
    public bool isDebugMode;
    public LegId legID;

    private void Start()
    {
        vehicleTimingsPerLegPerLap = new float[4, 4];
        firstPlaceTiming = new float[LapManager.Instance.LapsToComplete];
        currentPlaceTiming = new float[LapManager.Instance.LapsToComplete];
        for (int i = 0; i < LapManager.Instance.LapsToComplete; i++)
        {
            firstPlaceTiming[i] = Mathf.Infinity;
            currentPlaceTiming[i] = Mathf.Infinity;
        }
        for (int lapID = 0; lapID < LapManager.Instance.LapsToComplete; lapID++)
        {
            for (int playerid = 0; playerid < 4; playerid++)
            {
                vehicleTimingsPerLegPerLap[lapID, playerid] = Mathf.Infinity;
            }
        }
    }

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
                    if (other.GetComponent<VehicleLapData>().currentLegID.Equals(LegId.Zero) && legID.Equals(LegId.Thirteen))
                    {
                        //currentPlayerLeg 0 < (Final Leg = 3), so we make playerCurrentLeg = 3
                        other.GetComponent<VehicleLapData>().currentLegID = legID;
                        LegTimingUpdate(other);
                    }
                    else
                    {
                        //Increment Leg Condition
                        //if (other.GetComponent<VehicleLapData>().currentLegID + 1 == legID)
                        
                            //The LapManager Component Dictates the Final Leg of the Lap, thus scoring a Lap. See Lap Manager for Lap incrementation.
                            other.GetComponent<VehicleLapData>().currentLegID = legID;
                            LegTimingUpdate(other);
                            if (isDebugMode) Debug.Log("We are Now at LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                        
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
                    LegTimingUpdate(other);

                    if (isDebugMode) Debug.Log("Going Back to LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                }
                return;
            }
            LegTimingUpdate(other);


        }
    }

    public float[,] vehicleTimingsPerLegPerLap;

    [SerializeField] private float[] firstPlaceTiming;
    [SerializeField] private float[] currentPlaceTiming;
    private void LegTimingUpdate(Collider other)
    {
        vehicleTimingsPerLegPerLap[other.GetComponent<VehicleLapData>().LapsCompleted, other.GetComponent<VehicleBehavior>().PlayerID - 1] = other.GetComponent<VehicleLapData>().playerRaceTime;
        GetBestTimeForLegAtLap(other.GetComponent<VehicleLapData>().LapsCompleted);
        //currentPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] = other.GetComponent<VehicleLapData>().playerRaceTime;

        //SetBestTimeForLegAtLap(other.GetComponent<VehicleLapData>().LapsCompleted);

        if (vehicleTimingsPerLegPerLap[other.GetComponent<VehicleLapData>().LapsCompleted, other.GetComponent<VehicleBehavior>().PlayerID - 1] < firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted])
        {
            //bestTimeToReachSegment = vehicleTimingsPerLegPerLap[other.GetComponent<VehicleLapData>().LapsCompleted, other.GetComponent<VehicleBehavior>().PlayerID - 1];
            Debug.Log("We have to update the new First Place time from: " + firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] + ", to: " + other.GetComponent<VehicleLapData>().playerRaceTime);
            firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] = other.GetComponent<VehicleLapData>().playerRaceTime;
        }
        else
        {
            TimeSplitsManager.Instance.PlayerTimeSplitObjs[other.GetComponent<VehicleBehavior>().PlayerID - 1].gameObject.SetActive(true);
            TimeSplitsManager.Instance.UpdatePlayerTimeSplit(
                other.GetComponent<VehicleBehavior>().PlayerID,
                //currentPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] - firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted],
                vehicleTimingsPerLegPerLap[other.GetComponent<VehicleLapData>().LapsCompleted, other.GetComponent<VehicleBehavior>().PlayerID - 1] - firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted],
                false
                );
            Debug.Log("First Place Time: " + firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] + ", My Current Timing: " + vehicleTimingsPerLegPerLap[other.GetComponent<VehicleLapData>().LapsCompleted, other.GetComponent<VehicleBehavior>().PlayerID - 1]);

            if (TimeSplitActivityCO == null)
            {
                TimeSplitActivityCO = TimeSplitActivity(other);
                StartCoroutine(TimeSplitActivityCO);
            }
        }

        //if (StatManager.Instance.GetPlayerStats(other.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank) == 1)
        //{
        //    Debug.Log("We have to update the new First Place time from: " + firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] + ", to: " + other.GetComponent<VehicleLapData>().playerRaceTime);
        //    firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] = other.GetComponent<VehicleLapData>().playerRaceTime;
        //}
        //else
        //{
        //    TimeSplitsManager.Instance.PlayerTimeSplitObjs[other.GetComponent<VehicleBehavior>().PlayerID - 1].gameObject.SetActive(true);
        //    TimeSplitsManager.Instance.UpdatePlayerTimeSplit(
        //        other.GetComponent<VehicleBehavior>().PlayerID,
        //        currentPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] - firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted],
        //        false
        //        );
        //    Debug.Log("First Place Time: " + firstPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted] + ", My Current Timing: " + currentPlaceTiming[other.GetComponent<VehicleLapData>().LapsCompleted]);
        //    StartCoroutine(TimeSplitActivity(other));
        //}
    }

    private float bestTimeToReachSegment = Mathf.Infinity;
    private void GetBestTimeForLegAtLap(int lap)
    {
        firstPlaceTiming[lap] = Mathf.Infinity;
        for (int i = 0; i < 4; i++)
        {
            if (firstPlaceTiming[lap] > vehicleTimingsPerLegPerLap[lap, i])
            {
                firstPlaceTiming[lap] = vehicleTimingsPerLegPerLap[lap, i];
            }
        }
    }

    private IEnumerator TimeSplitActivityCO;
    private IEnumerator TimeSplitActivity(Collider other)
    {
        yield return new WaitForSeconds(3);
        TimeSplitsManager.Instance.PlayerTimeSplitObjs[other.GetComponent<VehicleBehavior>().PlayerID - 1].gameObject.SetActive(false);
        TimeSplitActivityCO = null;
    }


}

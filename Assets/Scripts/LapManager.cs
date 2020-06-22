using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.Controls;

public class LapManager : MonoBehaviour
{
    public static LapManager Instance;
    public bool isDebugMode;
    public int LapsToComplete;
    private GameObject[] allPlayers;
    public List<VehicleLapData> playerLapDataList;
    public int RankVal;
    public int racersfinished;
   
    public int Lapnum;
    void Awake()
    {
        Instance = this;
        allPlayers = GameObject.FindGameObjectsWithTag("GameController");

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

    private void Start()
    {
        playerLapDataList = new List<VehicleLapData>();

        if (PlayerRankOrderHandle == null)
        {
            PlayerRankOrderHandle = PlayerRankOrderCoroutine();
            StartCoroutine(PlayerRankOrderHandle);
        }
    }

   

    int i;
    IEnumerator PlayerRankOrderHandle;
    IEnumerator PlayerRankOrderCoroutine()
    {
        while (true)
        {

            if (playerLapDataList.Count > 1)
            {
                playerLapDataList = playerLapDataList.OrderByDescending(x => x.playerTotalProgress).ToList();

                RankVal = 1;

                foreach (VehicleLapData playerLapData in playerLapDataList)
                {
                    switch (playerLapData.GetComponent<VehicleBehavior>().PlayerID)
                    {
                        case 1:
                            StatManager.Instance.ChangePlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank, RankVal++);
                            ControllerHandler.Instance.HUDPlayer1.TextRacePlacement.text = StatManager.Instance.GetPlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank).ToString();
                            break;
                        case 2:
                            StatManager.Instance.ChangePlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank, RankVal++);
                            ControllerHandler.Instance.HUDPlayer2.TextRacePlacement.text = StatManager.Instance.GetPlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank).ToString();
                            break;
                        case 3:
                            StatManager.Instance.ChangePlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank, RankVal++);
                            ControllerHandler.Instance.HUDPlayer3.TextRacePlacement.text = StatManager.Instance.GetPlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank).ToString();
                            break;
                        case 4:
                            StatManager.Instance.ChangePlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank, RankVal++);
                            ControllerHandler.Instance.HUDPlayer4.TextRacePlacement.text = StatManager.Instance.GetPlayerStats(playerLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank).ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            yield return new WaitForSeconds(1);
            PlayerRankOrderHandle = null;
        }
    }

    public void AddPlayerToScoreList(int playerID)
    {
        foreach (GameObject p in allPlayers)
        {
            if (p.GetComponent<VehicleBehavior>().PlayerID == playerID)
            {
                playerLapDataList.Add(p.GetComponent<VehicleLapData>());

                if (PlayerRankOrderHandle == null)
                {
                    PlayerRankOrderHandle = PlayerRankOrderCoroutine();
                    StartCoroutine(PlayerRankOrderHandle);
                }

            }
        }
    }


    //To successfully complete a lap, players must have a TriggerObject_LapCheck first before TriggerObject_Lap
    private void PlayerLapCompleted(GameObject obj)
    {
        StatManager.Instance.ChangePlayerStats(obj.GetComponent<VehicleBehavior>().PlayerID, StatType.LastLapTime, obj.GetComponent<VehicleLapData>().playerLapTime);

        obj.GetComponent<VehicleLapData>().IncrementLap();
        obj.GetComponent<VehicleLapData>().currentLegID = LegId.Zero;
        if (isDebugMode) Debug.Log("Player" + obj.GetComponent<VehicleBehavior>().PlayerID + ": completed a lap. Total Laps completed: " + obj.GetComponent<VehicleLapData>().LapsCompleted);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GameController"))
        {

            if (Vector3.Dot(other.GetComponent<VehicleBehavior>().vehicle_heading_transform.forward, transform.forward) > 0)
            {
                other.GetComponent<VehicleBehavior>().playerHUD.WrongDirectionWarning.SetActive(false);

                if (other.GetComponent<VehicleLapData>().currentLegID.Equals(LegId.Three))
                {
                    PlayerLapCompleted(other.gameObject);
                    LegTimingUpdate(other);
                }
            }
            else
            {
                if (isDebugMode) Debug.Log("WRONG direction");

                other.GetComponent<VehicleBehavior>().playerHUD.WrongDirectionWarning.SetActive(true);

                if (other.GetComponent<VehicleLapData>().currentLegID.Equals(LegId.Zero))
                {
                    other.GetComponent<VehicleLapData>().currentLegID = LegId.Three;
                    other.GetComponent<VehicleLapData>().LapsCompleted = other.GetComponent<VehicleLapData>().LapsCompleted >= 0 ? other.GetComponent<VehicleLapData>().LapsCompleted -= 1 : -1;
                    LegTimingUpdate(other);

                    if (isDebugMode) Debug.Log("LAP subtracted: " + other.GetComponent<VehicleLapData>().LapsCompleted + ", Going Back to LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                }
                return;
            }


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
        //}
        //StartCoroutine(TimeSplitActivity(other));
    }

    private float bestTimeToReachSegment = Mathf.Infinity;
    private void GetBestTimeForLegAtLap(int lap)
    {
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

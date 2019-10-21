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
    int RankVal;

    void Awake()
    {
        Instance = this;
        allPlayers = GameObject.FindGameObjectsWithTag("GameController");
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
                    if (isDebugMode) Debug.Log("LAP subtracted: " + other.GetComponent<VehicleLapData>().LapsCompleted + ", Going Back to LEG: " + other.GetComponent<VehicleLapData>().currentLegID.ToString());
                }
                return;
            }


        }
    }


}

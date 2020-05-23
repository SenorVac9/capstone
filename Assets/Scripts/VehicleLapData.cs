using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.Controls;

public class VehicleLapData : MonoBehaviour
{
    public bool isDebugMode;
    public int LapsCompleted;
    public int LapsTarget;
    public bool IsPlayerFinishedRace;
    public LegId currentLegID;

    public float percentLegComplete;
    public float playerLegProgress;
    public float currentLegProgress;
    public float playerTotalProgress;
    public float playerLapTime;
    public float playerRaceTime;
    float currentTempProgress;
    int LapTrackerValue;
    float timer;

    public SaveGameManager saveGameManager;
    LegTriggerBehavior[] legDatas;
    LapManager lapManager;

    VehicleBehavior vehicleBehavior;

    Transform leg1, leg2, leg3, leg4;

    SimpleUI playerHUD;

    GameObject[] allPlayers;
    int currentPlacement;


    void Awake()
    {
        saveGameManager = GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>();

        LapsCompleted = 0;
        currentLegID = LegId.Zero;
        legDatas = GameObject.FindObjectsOfType<LegTriggerBehavior>();
        lapManager = GameObject.FindObjectOfType<LapManager>();
        vehicleBehavior = GetComponent<VehicleBehavior>();
        playerHUD = GetComponent<VehicleBehavior>().playerHUD;

        foreach (LegTriggerBehavior l in legDatas)
        {
            switch (l.legID)
            {
                case LegId.One:
                    leg1 = l.transform;
                    continue;
                case LegId.Two:
                    leg2 = l.transform;
                    continue;
                case LegId.Three:
                    leg3 = l.transform;
                    continue;
                default:
                    break;
            }
        }
        leg4 = lapManager.transform;

        allPlayers = GameObject.FindGameObjectsWithTag("GameController");

    }

    private void Start()
    {
        //Home to first base (This should be tied to where the player initially spawns)
        GetLegDistance(leg4, leg1);
    }

    public void GhostFinished()
    {
        lapManager.racersfinished++;
        switch ((lapManager.racersfinished))
        {
            case 1:
                saveGameManager.GameState.FirstRaceTime = saveGameManager.Ghost.ghostTime;
                saveGameManager.GameState.FirstPlaceID = -1;
                break;
            case 2:
                saveGameManager.GameState.SecondRaceTime = saveGameManager.Ghost.ghostTime;
                saveGameManager.GameState.SecondPlaceID = -1;
                break;
            case 3:
                saveGameManager.GameState.ThirdRaceTime = saveGameManager.Ghost.ghostTime;
                saveGameManager.GameState.ThirdPlaceID = -1;
                break;
            case 4:
                saveGameManager.GameState.FourthRaceTime = saveGameManager.Ghost.ghostTime;
                saveGameManager.GameState.FourthPlaceID = -1;
                break;
        }
    }
    private void Update()
    {
        //if (!vehicleBehavior.isControllerInitialized) return;
        if (!vehicleBehavior.playerHUD.simpleCharacterSeleciton.isCharacterSelected) return;
        //if (!GameLogicManager.Instance.IsGameStarted) return;
        //if (GameLogicManager.Instance.IsGameFinished) return;

        if (LapsCompleted >= LapManager.Instance.LapsToComplete)
        {
            if (!IsPlayerFinishedRace)
            {
                IsPlayerFinishedRace = true;
                Debug.Log("PlayerContainer: " + vehicleBehavior.name);
                vehicleBehavior.playerHUD.GameOverBackgroundObject.SetActive(true);
                vehicleBehavior.playerHUD.placeshower.transform.GetChild(lapManager.racersfinished).gameObject.SetActive(true);
                lapManager.racersfinished++;
                vehicleBehavior.playerHUD.TextGameOver.text = "RACE COMPLETED\nWAITING FOR ALL PLAYERS TO FINISH";

                GameLogicManager.Instance.SetIsPlayerFinished();
                switch ((lapManager.racersfinished))
                {
                    case 1:
                        saveGameManager.GameState.FirstRaceTime = playerRaceTime;
                        saveGameManager.GameState.FirstPlaceID = vehicleBehavior.PlayerID;
                        break;
                    case 2:
                        saveGameManager.GameState.SecondRaceTime = playerRaceTime;
                        saveGameManager.GameState.SecondPlaceID = vehicleBehavior.PlayerID;
                        break;
                    case 3:
                        saveGameManager.GameState.ThirdRaceTime = playerRaceTime;
                        saveGameManager.GameState.ThirdPlaceID = vehicleBehavior.PlayerID;
                        break;
                    case 4:
                        saveGameManager.GameState.FourthRaceTime = playerRaceTime;
                        saveGameManager.GameState.FourthPlaceID = vehicleBehavior.PlayerID;
                        break;
                }

            }

            if (GameLogicManager.Instance.CheckEveryPlayerFinished() && timer < Time.time && timer > 0)
            {
                SceneManager.LoadScene(3);
            }
            else if (GameLogicManager.Instance.CheckEveryPlayerFinished() && timer <= 0)
            {
                GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>().Save();

                timer = Time.time + 5.0f;
                Debug.Log("Timer is" + timer);
            }

        }

        switch (currentLegID)
        {
            case LegId.Zero:
                GetLegDistance(leg4, leg1);
                GetPlayerDistanceToNextLeg(leg1);
                break;
            case LegId.One:
                GetLegDistance(leg1, leg2);
                GetPlayerDistanceToNextLeg(leg2);
                break;
            case LegId.Two:
                GetLegDistance(leg2, leg3);
                GetPlayerDistanceToNextLeg(leg3);
                break;
            case LegId.Three:
                GetLegDistance(leg3, leg4);
                GetPlayerDistanceToNextLeg(leg4);
                break;
            default:
                break;
        }

        RaceInfoUI_Update(playerTotalProgress);
    }

    private void RaceInfoUI_Update(float tempProgress)
    {
        if (IsPlayerFinishedRace) return;

        playerLapTime += Time.deltaTime;
        playerRaceTime += Time.deltaTime;

        //Since Laps need to go to -1 as the 'First' lap is Lap 0, they need to have a min of -1. But here we display only min of 0 laps
        playerHUD.TextRaceProgress.text = (Mathf.Max(0, Mathf.Round(playerTotalProgress * 100) / 100)).ToString();
        playerHUD.TextRaceTime.text = (Mathf.Round(playerRaceTime * 100) / 100).ToString();

        //if (currentTempProgress < tempProgress)
        //{
        //    playerHUD.TextRaceProgress.text = (Mathf.Max(0, Mathf.Round(tempProgress * 100) / 100)).ToString();
        //    currentTempProgress = tempProgress;
        //}
    }


    private void GetLegDistance(Transform legFrom, Transform legTo)
    {
        currentLegProgress = Vector3.Distance(legFrom.position, legTo.position);
    }

    private void GetPlayerDistanceToNextLeg(Transform legTo)
    {
        playerLegProgress = Vector3.Distance(transform.position, legTo.position);
        percentLegComplete = (int)((1 - Mathf.Min(playerLegProgress / currentLegProgress, 1)) * 100);
        playerTotalProgress = (LapsCompleted * 4 + ((int)currentLegID) + (percentLegComplete / 100)) / 4;
        if (isDebugMode) Debug.Log("Total Player Progress: " + playerTotalProgress);
    }


    public void IncrementLap()
    {
        LapsCompleted++;
        if (LapsCompleted == LapTrackerValue + 1)
        {
            LapTrackerValue++;
            StatManager.Instance.ChangePlayerStats(vehicleBehavior.PlayerID, StatType.LastLapTime, Mathf.Round(playerLapTime * 100) / 100);
            playerHUD.TextLastLapTime.text = (Mathf.Round(playerLapTime * 100) / 100).ToString();
            ResetLapTime();
        }
    }

    public void DecrementLap()
    {
        LapsCompleted--;
    }

    public void ResetLapTime()
    {
        playerLapTime = 0;
    }
}

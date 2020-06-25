using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.Controls;
using UnityEngine.UI;

public class VehicleLapData : MonoBehaviour
{
    public Text LapDisplay;
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

    Transform leg1, leg2, leg3, leg4, leg5, leg6, leg7, leg8, leg9, leg10, leg11,leg12, leg13, lapTransform;

    SimpleUI playerHUD;

    GameObject[] allPlayers;
    int currentPlacement;

    public Vector3 lastCheckPoint;

    void Awake()
    {
        saveGameManager = GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>();
        
        LapsCompleted = 0;
        currentLegID = LegId.Zero;
        legDatas = GameObject.FindObjectsOfType<LegTriggerBehavior>();
        lapManager = GameObject.FindObjectOfType<LapManager>();
        lapTransform = lapManager.transform;
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
                case LegId.Four:
                    leg4 = l.transform;
                    continue;
                case LegId.Five:
                    leg5 = l.transform;
                    continue;
                case LegId.Six:
                    leg6 = l.transform;
                    continue;
                case LegId.Seven:
                    leg7 = l.transform;
                    continue;
                case LegId.Eight:
                    leg8 = l.transform;
                    continue;
                case LegId.Nine:
                    leg9 = l.transform;
                    continue;
                case LegId.Ten:
                    leg10 = l.transform;
                    continue;
                case LegId.Eleven:
                    leg11 = l.transform;
                    continue;
                case LegId.Twelve:
                    leg12 = l.transform;
                    continue;
                case LegId.Thirteen:
                    leg13 = l.transform;
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
        LapDisplay.text ="Laps"+ LapsCompleted + "/1";
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
                        saveGameManager.GameState.P1Character = playerHUD.gameObject.GetComponent<SimpleCharacterSelection>().whichCharacterDidISelectDuringTheGameScene;
                       
                        break;
                    case 2:
                        saveGameManager.GameState.SecondRaceTime = playerRaceTime;
                        saveGameManager.GameState.SecondPlaceID = vehicleBehavior.PlayerID;
                        saveGameManager.GameState.P2Character = playerHUD.gameObject.GetComponent<SimpleCharacterSelection>().whichCharacterDidISelectDuringTheGameScene;
                        break;
                    case 3:
                        saveGameManager.GameState.ThirdRaceTime = playerRaceTime;
                        saveGameManager.GameState.ThirdPlaceID = vehicleBehavior.PlayerID;
                        saveGameManager.GameState.P3Character = playerHUD.gameObject.GetComponent<SimpleCharacterSelection>().whichCharacterDidISelectDuringTheGameScene;
                        break;
                    case 4:
                        saveGameManager.GameState.FourthRaceTime = playerRaceTime;
                        saveGameManager.GameState.FourthPlaceID = vehicleBehavior.PlayerID;
                        saveGameManager.GameState.P4Character = playerHUD.gameObject.GetComponent<SimpleCharacterSelection>().whichCharacterDidISelectDuringTheGameScene;
                        break;
                }

            }

            if (GameLogicManager.Instance.CheckEveryPlayerFinished() && timer < Time.time && timer > 0)
            {
                SceneManager.LoadScene(4);
            }
            else if (GameLogicManager.Instance.CheckEveryPlayerFinished() && timer <= 0)
            {
                saveGameManager.GameState.numPlayer = lapManager.racersfinished;
                GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>().Save();

                timer = Time.time + 5.0f;
                Debug.Log("Timer is" + timer);
            }

        }

        switch (currentLegID)
        {
            case LegId.Zero:
                GetLegDistance(leg13, leg1);
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
            case LegId.Four:
                GetLegDistance(leg4, leg5);
                GetPlayerDistanceToNextLeg(leg5);
                break;
            case LegId.Five:
                GetLegDistance(leg5, leg6);
                GetPlayerDistanceToNextLeg(leg6);
                break;
            case LegId.Six:
                GetLegDistance(leg6, leg7);
                GetPlayerDistanceToNextLeg(leg7);
                break;
            case LegId.Seven:
                GetLegDistance(leg7, leg8);
                GetPlayerDistanceToNextLeg(leg8);
                break;
            case LegId.Eight:
                GetLegDistance(leg8, leg9);
                GetPlayerDistanceToNextLeg(leg9);
                break;
            case LegId.Nine:
                GetLegDistance(leg9, leg10);
                GetPlayerDistanceToNextLeg(leg10);
                break;
            case LegId.Ten:
                GetLegDistance(leg10, leg11);
                GetPlayerDistanceToNextLeg(leg11);
                break;
            case LegId.Eleven:
                GetLegDistance(leg11, leg12);
                GetPlayerDistanceToNextLeg(leg12);
                break;
            case LegId.Twelve:
                GetLegDistance(leg12, leg13);
                GetPlayerDistanceToNextLeg(leg13);
                break;
            case LegId.Thirteen:
                GetLegDistance(leg13, lapTransform);
                GetPlayerDistanceToNextLeg(lapTransform);
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

    public Transform GetRespawnTransform()
    {
        switch (currentLegID)
        {
            case LegId.Zero:
                return lapTransform;
            case LegId.One:
                return leg1;
            case LegId.Two:
                return leg2;
            case LegId.Three:
                return leg3;
            case LegId.Four:
                return leg4;
            case LegId.Five:
                return leg5;
            case LegId.Six:
                return leg6;
            case LegId.Seven:
                return leg7;
            case LegId.Eight:
                return leg8;
            case LegId.Nine:
                return leg9;
            case LegId.Ten:
                return leg10;
            case LegId.Eleven:
                return leg11;
            case LegId.Twelve:
                return leg12;
            case LegId.Thirteen:
                return leg13;
            default:
                return lapTransform;
                break;
        }
    }
}

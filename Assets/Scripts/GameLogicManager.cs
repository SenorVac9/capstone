using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.Controls;

public class GameLogicManager : MonoBehaviour
{
    public static GameLogicManager Instance;
    public bool arePlayersFinished;

    [HideInInspector] private bool isPlayerStarted;
    [HideInInspector] private bool isPlayerFinished;

    public bool IsPlayerStarted { get { return isPlayerStarted; } set { isPlayerStarted = value; } }
    public bool IsPlayerFinished { get { return isPlayerFinished; } set { isPlayerFinished = value; } }

    private void Awake()
    {
        Instance = this;
    }

    public void SetIsPlayerFinished()
    {
        foreach (VehicleBehavior flickballarenaPlayer in ControllerHandler.Instance.vehicles)
        {
            if (flickballarenaPlayer.playerHUD.simpleCharacterSeleciton.isCharacterSelected)
            {
                if (flickballarenaPlayer.GetComponent<VehicleLapData>().IsPlayerFinishedRace == false)
                {
                    return;
                }
            }
        }
        IsPlayerFinished = true;
        Debug.Log("ALL " + LapManager.Instance.playerLapDataList.Count + " PLAYERS HAVE COMPLETED THE RACE");
    }


    public bool CheckEveryPlayerFinished()
    {
        foreach (VehicleBehavior v in ControllerHandler.Instance.vehicles)
        {
            if (v.playerHUD.simpleCharacterSeleciton.isCharacterSelected)
            {
                if (!v.GetComponent<VehicleLapData>().IsPlayerFinishedRace)
                {
                    arePlayersFinished = false;
                    return arePlayersFinished;
                }
            }

        }
        arePlayersFinished = true;
        return arePlayersFinished;
    }
}

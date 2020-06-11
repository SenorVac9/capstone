using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using ModuloKart.CustomVehiclePhysics;
using ModuloKart.PlayerSelectionMenu;
using ModuloKart.CountDown;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Initialization of All Players")]
    private bool beginCountDown;
    public bool GameStart;
    public int InitializedPlayers;
    // Start is called before the first frame update

    public Transform lastCheckpoint;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Y_ANYPLAYER"))
        {
            SceneManager.LoadScene(0);
        }
    }


    public void ReadyUp()
    {
        foreach (VehicleLapData playerLapData in LapManager.Instance.playerLapDataList)
        {
            switch (playerLapData.GetComponent<VehicleBehavior>().PlayerID)
            {
                case 1:
                    if (playerLapData.GetComponent<VehicleBehavior>().playerHUD.simpleCharacterSeleciton.isCharacterSelected)
                    {
                        InitializedPlayers++;
                        Debug.Log(InitializedPlayers + " out of: " + (int)PlayerSelectionManager.Instance.numPlayerOption + " Players have been initialized");
                    }
                    if (InitializedPlayers == (int)PlayerSelectionManager.Instance.numPlayerOption)
                        beginCountDown = true;
                    break;
                case 2:
                    if (playerLapData.GetComponent<VehicleBehavior>().playerHUD.simpleCharacterSeleciton.isCharacterSelected)
                    {
                        InitializedPlayers++;
                        Debug.Log(InitializedPlayers + " out of: " + (int)PlayerSelectionManager.Instance.numPlayerOption + " Players have been initialized");
                    }
                    if (InitializedPlayers == (int)PlayerSelectionManager.Instance.numPlayerOption)
                        beginCountDown = true;
                    break;
                case 3:
                    if (playerLapData.GetComponent<VehicleBehavior>().playerHUD.simpleCharacterSeleciton.isCharacterSelected)
                    {
                        InitializedPlayers++;
                        Debug.Log(InitializedPlayers + " out of: " + (int)PlayerSelectionManager.Instance.numPlayerOption + " Players have been initialized");
                    }
                    if (InitializedPlayers == (int)PlayerSelectionManager.Instance.numPlayerOption)
                        beginCountDown = true;
                    break;
                case 4:
                    if (playerLapData.GetComponent<VehicleBehavior>().playerHUD.simpleCharacterSeleciton.isCharacterSelected)
                    {
                        InitializedPlayers++;
                        Debug.Log(InitializedPlayers + " out of: " + (int)PlayerSelectionManager.Instance.numPlayerOption + " Players have been initialized");
                    }
                    if (InitializedPlayers == (int)PlayerSelectionManager.Instance.numPlayerOption)
                        beginCountDown = true;
                    break;
                default:
                    GameStart = false;
                    Debug.Log("Could Not Ready Up Player: " + playerLapData.GetComponent<VehicleBehavior>().PlayerID);
                    break;
            }
        }

        if (beginCountDown)
        {
            SimpleCountDown.Instance.StartCountDown();
        }
    }


}
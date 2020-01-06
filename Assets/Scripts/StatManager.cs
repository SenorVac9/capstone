using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.PlayerSelectionMenu;
using ModuloKart.Controls;
using ModuloKart.CustomVehiclePhysics;

public enum StatType
{
    Rank = 1,
    LastLapTime = 2,
}

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;
    public Dictionary<int, Dictionary<StatType, float>> statManagerDictionary;

    private void Awake()
    {
        Instance = this;

        //Initialize the Flickball Arena Player Stat Dictionary
        statManagerDictionary = new Dictionary<int, Dictionary<StatType, float>>();

        if (PlayerSelectionManager.Instance)
            InitializePlayerStats();
    }

    private void Start()
    {
    }

    public void InitializePlayerStats()
    {
        foreach (VehicleBehavior fPlayer in ControllerHandler.Instance.vehicles)
        {
            if (!statManagerDictionary.ContainsKey(fPlayer.PlayerID))
            {
                statManagerDictionary.Add(fPlayer.PlayerID, new Dictionary<StatType, float>());
                statManagerDictionary[fPlayer.PlayerID][StatType.Rank] = 0;
                statManagerDictionary[fPlayer.PlayerID][StatType.LastLapTime] = 0;
                Debug.Log("StatManager initialized Player: " + fPlayer.PlayerID + ", with a rank of: " + statManagerDictionary[fPlayer.PlayerID][StatType.Rank]);
            }
            else
            {
                Debug.Log("StatManager failed To initialize Player: " + fPlayer.PlayerID + ". It is already exists, value is: " + statManagerDictionary[fPlayer.PlayerID][StatType.Rank]);
            }
        }

    }

    public void ChangePlayerStats(int playerID, StatType statType, float statValue)
    {
        if (!statManagerDictionary.ContainsKey(playerID))
        {
            Debug.Log("StatManager failed to change stats for Player: " + playerID + ". It does not exist");
        }
        else
        {
            statManagerDictionary[playerID][statType] = statValue;
        }
    }

    public float GetPlayerStats(int playerID, StatType statType)
    {
        if (!statManagerDictionary.ContainsKey(playerID))
        {
            Debug.Log("StatManager failed to get stats for Player: " + playerID + ". It does not exist");
            return -999;
        }
        else
        {
            return statManagerDictionary[playerID][statType];
        }
    }
}

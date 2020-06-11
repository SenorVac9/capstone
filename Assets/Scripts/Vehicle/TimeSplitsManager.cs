using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.CustomVehiclePhysics;

//namespace ModuloKart.CustomTimeSplits
//{
    public class TimeSplitsManager : MonoBehaviour
    {
        public static TimeSplitsManager Instance;
        public GameObject[] PlayerTimeSplitObjs;
        public Text[] PlayerTimeSplitText;

        private float currentTimeSplit;
        private float firstPlaceTime;



        private void Awake()
        {
            Instance = this;
            InitTimeSplitActivity();
        }

        public void UpdatePlayerTimeSplit(int id, float timeSplitValue, bool isFirst)
        {
        Debug.Log("tIME Split: for player: " + id);

        if (isFirst) PlayerTimeSplitText[id - 1].text = "Gained First Place Position";
        else PlayerTimeSplitText[id - 1].text = timeSplitValue.ToString() + " Behind First Place";
            foreach (VehicleLapData _vehicleLapData in LapManager.Instance.playerLapDataList)
            {
                if (StatManager.Instance.GetPlayerStats(_vehicleLapData.GetComponent<VehicleBehavior>().PlayerID, StatType.Rank) == 1)
                {
                    firstPlaceTime = _vehicleLapData.playerRaceTime;
                }
            }
        }

        private void InitTimeSplitActivity()
        {
            foreach (GameObject g in TimeSplitsManager.Instance.PlayerTimeSplitObjs)
            {
                g.SetActive(false);
            }
        }
    }
//}
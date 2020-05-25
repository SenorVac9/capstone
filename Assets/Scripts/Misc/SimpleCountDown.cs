using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.Controls;

namespace ModuloKart.CountDown
{

    public class SimpleCountDown : MonoBehaviour
    {
        public static SimpleCountDown Instance;
        public Image bg_CountDown;
        public Text CountDown;
        public Text LabelReadyUp;

        private void Awake()
        {
            Instance = this;
            bg_CountDown.enabled = false;
            CountDown.enabled = false;
            LabelReadyUp.enabled = false;
        }

        public void StartCountDown()
        {
            if (StartCountDownHandle == null)
            {
                StartCountDownHandle = StartCountDownCoroutine();
                StartCoroutine(StartCountDownHandle);
            }
        }

        IEnumerator StartCountDownHandle;
        IEnumerator StartCountDownCoroutine()
        {
            LabelReadyUp.gameObject.SetActive(false);

            CountDown.enabled = true;
            bg_CountDown.enabled = true;
            LabelReadyUp.enabled = true;

            CountDown.text = "3";
            yield return new WaitForSeconds(1);
            CountDown.text = "2";
            yield return new WaitForSeconds(1);
            CountDown.text = "1";
            yield return new WaitForSeconds(1);
            CountDown.text = "MOVE YO ASSES!!";
            yield return new WaitForSeconds(1);

            GameManager.Instance.GameStart = true;

            bg_CountDown.gameObject.SetActive(false);
            CountDown.gameObject.SetActive(false);

           // ControllerHandler.Instance.AssignHUD(isActive: true);

            StartCountDownHandle = null;
            yield return null;
        }
    }


}
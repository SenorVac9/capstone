using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using ModuloKart.Controls;

namespace ModuloKart.OptionMenu {

    public enum numOptions
    {
        volume = 1,
        p1Pref = 2,
        p2Pref = 3,
        p3Pref = 4,
        p4Pref = 5,
        mainMenu = -1,

    }




public class OptionManager : MonoBehaviour
{

        OptionManager Instance;
        private gameObject bg_volume;
        private GameObject bg_p1Pref;
        private GameObject bg_p2Pref;
        private GameObject bg_p3Pref;
        private GameObject bg_p4Pref;
        private GameObject bg_mainMenu;

        public numOptions numOptions;
        public bool inGameScene;
        public int p1Preset = 1;
        public int p2Preset = 1;
        public int p3Preset = 1;
        public int p4Preset = 1;
        public int masterVolume = 7;
        public Text mV;
        public Text p1P;
        public Text p2P;
        public Text p3P;
        public Text p4P;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            GetPlayerSelectionOptions();
            InitPlayerOptions();
        }
            // Update is called once per frame
            void Update()
    {
            NumOptionSelectionNext();
            NumOptionSelectionPrev();
            ConfirmOption();
        }
        bool isPressPrev;
        bool isPressPrevRelease;
        private void NumOptionSelectionPrev()
        {
            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") < 0)
            {
                isPressPrev = true;
            }
            if (isPressPrev)
            {
                if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
                {
                    isPressPrev = false;
                    isPressPrevRelease = true;
                }
            }
            if (isPressPrevRelease)
            {
                isPressPrevRelease = false;

                switch (numOptions)
                {
                    case numOptions.volume:
                        numOptions = numOptions.mainMenu;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(false);
                        break;
                    case numOptions.p1Pref:
                        numOptions = numOptions.volume;
                        bg_volume.SetActive(false);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p2Pref:
                        numOptions = numOptions.p1Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(false);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p3Pref:
                        numOptions = numOptions.p2Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(false);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p4Pref:
                        numOptions = numOptions.p3Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(false);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.mainMenu:
                        numOptions = numOptions.p4Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(false);
                        bg_mainMenu.SetActive(true);
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }
        bool isPressNext;
        bool isPressNextRelease;
        private void NumOptionSelectionNext()
        {
            if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") > 0)
            {
                isPressNext = true;
            }
            if (isPressNext)
            {
                if (Input.GetAxis("LeftJoyStickY_ANYPLAYER") == 0)
                {
                    isPressNext = false;
                    isPressNextRelease = true;
                }
            }
            if (isPressNextRelease)
            {
                isPressNextRelease = false;

               switch (numOptions)
                {
                    case numOptions.volume:
                        numOptions = numOptions.p1Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(false);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p1Pref:
                        numOptions = numOptions.p2Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(false);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p2Pref:
                        numOptions = numOptions.p3Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(false);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p3Pref:
                        numOptions = numOptions.p4Pref;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(false);
                        bg_mainMenu.SetActive(true);
                        break;
                    case numOptions.p4Pref:
                        numOptions = numOptions.mainMenu;
                        bg_volume.SetActive(true);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(false);
                        break;
                    case numOptions.mainMenu:
                        numOptions = numOptions.volume;
                        bg_volume.SetActive(false);
                        bg_p1Pref.SetActive(true);
                        bg_p2Pref.SetActive(true);
                        bg_p3Pref.SetActive(true);
                        bg_p4Pref.SetActive(true);
                        bg_mainMenu.SetActive(true);
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }

        bool isPressLeft;
        bool isPressLeftRelease;
        private void OptionChangeLeft()
        { 
        if (Input.GetAxis("LeftJoyStickX_ANYPLAYER") < 0)
            {
                isPressLeft = true;
            }
            if (isPressRight)
            {
                if (Input.GetAxis("LeftJoyStickX_ANYPLAYER") == 0)
                {
                    isPressLeft = false;
                    isPressLeftRelease = true;
                }
            }
            if (isPressLeftRelease)
            {
                isPressLeftRelease = false;

               switch (numOptions)
                {
                    case numOptions.volume:
                        if (masterVolume <= 0)
                            masterVolume = 10;
                        else
                            masterVolume = masterVolume - 1;
                       mV.text = masterVolume.ToString();
                        break;
                    case numOptions.p1Pref:
                        if (p1Preset == 1)
                            p1Preset = 3;
                        
                        else
                            p1Preset = p1Preset - 1;
                        p1P.text = p1Preset.ToString();
                        break;
                    case numOptions.p2Pref:
                        if (p2Preset == 1)
                            p2Preset = 3;
                        else
                            p2Preset = p2Preset - 1;
                        p2P.text = p2Preset.ToString();
                        break;
                    case numOptions.p3Pref:
                        if (p3Preset == 1)
                            p3Preset = 3;
                        else
                            p3Preset = p3Preset - 1;
                        p3P.text = p3Preset.ToString();
                        break;
                    case numOptions.p4Pref:
                        if (p4Preset == 1)
                            p4Preset = 3;
                        else
                            p4Preset = p4Preset - 1;
                        p4P.text = p4Preset.ToString();
                        break;
                    case numOptions.mainMenu:
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }

        bool isPressRight;
        bool isPressRightRelease;
        private void OptionChangeRight()
        {
            if (Input.GetAxis("LeftJoyStickX_ANYPLAYER") > 0)
            {
                isPressRight = true;
            }
            if (isPressRight)
            {
                if (Input.GetAxis("LeftJoyStickX_ANYPLAYER") == 0)
                {
                    isPressRight = false;
                    isPressRightRelease = true;
                }
            }
            if (isPressRightRelease)
            {
                isPressRightRelease = false;

                switch (numOptions)
                {
                    case numOptions.volume:
                        if (masterVolume <= 10)
                            masterVolume = 0;
                        else
                            masterVolume = masterVolume + 1;
                        mV.text = masterVolume.ToString();
                        break;
                    case numOptions.p1Pref:
                        if (p1Preset == 3)
                            p1Preset = 1;
                        else
                            p1Preset = p1Preset + 1;
                        p1P.text = p1Preset.ToString();
                        break;
                    case numOptions.p2Pref:
                        if (p2Preset == 3)
                            p2Preset = 1;
                        else
                            p2Preset = p2Preset + 1;
                        p2P.text = p2Preset.ToString();
                        break;
                    case numOptions.p3Pref:
                        if (p3Preset == 3)
                            p3Preset = 1;
                        else
                            p3Preset = p3Preset + 1;
                        p3P.text = p3Preset.ToString();
                        break;
                    case numOptions.p4Pref:
                        if (p4Preset == 3)
                            p4Preset = 1;
                        else
                            p4Preset = p4Preset + 1;
                        p4P.text = p4Preset.ToString();
                        break;
                    case numOptions.mainMenu:
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }


        private void InitPlayerOptions()
        {
            numOptions = numOptions.volume;
            bg_volume.SetActive(false);
            bg_p1Pref.SetActive(true);
            bg_p2Pref.SetActive(true);
            bg_p3Pref.SetActive(true);
            bg_p4Pref.SetActive(true);
            bg_mainMenu.SetActive(true);
        }


        private void ConfirmOption()
        {
            if (Input.GetButtonDown("A_ANYPLAYER"))
            {
                switch (numOptions)
                {
                    case numOptions.volume:
                        //modify Volume Slider Here
                        break;
                    case numOptions.p1Pref:
                        //modify P1 Preset Here
                        break;
                    case numOptions.p2Pref:
                        //modify P2 Preset Here
                        break;
                    case numOptions.p3Pref:
                        //modify P3 Preset Here
                        break;
                    case numOptions.p4Pref:
                        //modify P4 Preset Here
                        break;
                    case numOptions.mainMenu:
                        ButtonBehavior_ReturnToMain();
                        break;
                    default:
                        Debug.Log("Unexpected Player Number Selection Option");
                        break;
                }
            }
        }

        private void GetPlayerSelectionOptions()
        {
            OptionSelector[] temp = GameObject.FindObjectsOfType<OptionSelector>();

            foreach (OptionSelector t in temp)
            {
                if (t.numoptions.Equals(numOptions.volume))
                {
                    bg_volume = t.bg;
                }
                else if (t.numoptions.Equals(numOptions.p1Pref))
                {
                    bg_p1Pref = t.bg;
                }
                else if (t.numoptions.Equals(numOptions.p2Pref))
                {
                    bg_p2Pref = t.bg;
                }
                else if (t.numoptions.Equals(numOptions.p3Pref))
                {
                    bg_p3Pref = t.bg;
                }
                else if (t.numoptions.Equals(numOptions.p4Pref))
                {
                    bg_p4Pref = t.bg;
                }
                else if (t.numoptions.Equals(numOptions.mainMenu))
                {
                    bg_mainMenu = t.bg;
                }
            }
        }

        public void ButtonBehavior_ReturnToMain()
        {
            SceneManager.LoadScene(0);
        }
    }
}

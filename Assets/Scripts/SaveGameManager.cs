using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.SceneManagement;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.PlayerSelectionMenu;
namespace ModuloKart.Controls

{
    public class SaveGameManager : MonoBehaviour
    {

        int GhostFrameCounter = 0;
        int GhostFrameMax = 0;
        GameObject GhostCar;
        bool GhostDone = false;
        bool GhostLoaded = false;
        public List<Vector3> newGhostPostions;
        public ControllerHandler Handler;
        public bool wantsToRaceGhost = true;
        GameObject GameCar;


        // Start is called before the first frame update
        void Start()
        {
            Ghost = new GhostData();


            if (SceneManager.GetActiveScene().buildIndex == 2)
            {



                Handler = GameObject.FindObjectOfType<ControllerHandler>();
               
                GameState = new GameStateData();

                GameState.FirstPlaceID = 0;
                GameState.SecondPlaceID = 0;
                GameState.ThirdPlaceID = 0;
                GameState.FourthPlaceID = 0;
                GameState.FirstRaceTime = 0;
                GameState.SecondRaceTime = 0;
                GameState.ThirdRaceTime = 0;
                GameState.FourthRaceTime = 0;
                GameState.numPlayer = Handler.assignedControllerCount;


                GhostCar = GameObject.FindGameObjectWithTag("Ghost");
                LoadGhost();
               
                
                
                if(GhostLoaded)
                GameState.oldGhostTime = Ghost.ghostTime;

            }



            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                LoadRaceData("GameData.xml");
                Debug.Log("Player 1 played " + GameState.P1Character.ToString());
            }


        }
        public bool isGhostLoaded()
        {
            return GhostLoaded;
        }
        public class GhostData
        {
            public List<Vector3> ghostPostions;
            public float ghostTime=0;
            public AVerySimpleEnumOfCharacters GhostCharacter;
        }
        public class RacerData
        {
            public float Lap1Time = 999, Lap2Time = 999, Lap3Time = 999;
            public int RacerID;
           
        }

        public class GameStateData
        {
            public int FirstPlaceID = 0, SecondPlaceID = 0, ThirdPlaceID = 0, FourthPlaceID = 0;
            public float FirstRaceTime = 0, SecondRaceTime = 0, ThirdRaceTime = 0, FourthRaceTime = 0;
            public float oldGhostTime;
            public int numPlayer = 999;
            public AVerySimpleEnumOfCharacters P1Character, P2Character, P3Character, P4Character = 0;
            
        }
       
        public GameStateData GameState = new GameStateData();
        public RacerData RacerStats = new RacerData();
        public GhostData Ghost = new GhostData();
        private void FixedUpdate()
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {


                if (Handler.ControllersToAssign == 1 || wantsToRaceGhost)
                {

                    if(Handler.assignedControllerCount == 1)
                    {
                        if (GhostLoaded)
                        {

                            if (GhostFrameCounter < GhostFrameMax)
                            {
                                //We are at the Last Array Index
                                if (GhostFrameCounter == GhostFrameMax - 1)
                                {
                                    //So if we are at the last position of the array, the next value should be the start.
                                    GhostCar.transform.LookAt(newGhostPostions[0]);
                                }
                                else
                                {
                                    GhostCar.transform.LookAt(newGhostPostions[GhostFrameCounter + 1]);
                                }
                                GhostCar.transform.position = newGhostPostions[GhostFrameCounter];

                                GhostFrameCounter++;

                            }
                            else if (GhostDone == false && GhostFrameCounter >= GhostFrameMax)
                            {
                                GhostDone = true;
                                GameObject.FindGameObjectWithTag("GameController").GetComponent<VehicleLapData>().GhostFinished();
                            }

                        }
                        else if (GhostCar)
                        {
                            Destroy(GhostCar);
                        }
                    //    Debug.Log(GameObject.FindGameObjectWithTag("GameController").transform.position);
                        Ghost.ghostPostions.Add(GameObject.FindGameObjectWithTag("GameController").transform.position);
                        
                    }
                   
                }
                else if (GhostCar)
                {
                    Destroy(GhostCar);
                }



            }
        }
        public void LoadGhost()
        {
            if (File.Exists("Ghost Data.xml"))
            {
                XmlSerializer Serializer = new XmlSerializer(typeof(GhostData));
                FileStream Stream = new FileStream("Ghost Data.xml", FileMode.Open);
                Ghost = Serializer.Deserialize(Stream) as GhostData;
                Stream.Close();


                GhostFrameMax = Ghost.ghostPostions.Count;
                newGhostPostions = Ghost.ghostPostions;
                Ghost.ghostPostions = new List<Vector3>();
                GhostLoaded = true;
                
                Debug.Log("Ghost Loaded");

            }
            else
            {
                Debug.Log("No Ghost");
                Ghost.ghostPostions = new List<Vector3>();
            }
                

          
        }

        public void SaveGhost()
        {
            Ghost.ghostTime = GameState.FirstRaceTime;
            Ghost.GhostCharacter = GameState.P1Character;
            XmlSerializer Serializers = new XmlSerializer(typeof(GhostData));
            FileStream Streams = new FileStream("Ghost Data.xml", FileMode.Create);
            Serializers.Serialize(Streams, Ghost);
            Streams.Close();
            Debug.Log("Ghost Saved");
        }
        public void Save(string FileName = "GameData.xml")
        {
            if (Ghost.ghostTime == 0 || GameState.FirstRaceTime <= Ghost.ghostTime)
            {
                SaveGhost();
            }
            else
                Debug.Log("Ghost not updated");


            // Save game data


            XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
            FileStream Stream = new FileStream(FileName, FileMode.Create);
            Serializer.Serialize(Stream, GameState);
            Debug.Log(GameState.FirstPlaceID + ", " + GameState.FirstRaceTime);
            Stream.Close();

         //   Debug.Log("Test ghost pos:" + Ghost.ghostPostions[0]);
        }
        public void SaveRacer(string FileName)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(RacerData));
            FileStream Stream = new FileStream(FileName, FileMode.Create);
            Serializer.Serialize(Stream, RacerStats);
            Stream.Close();
        }
        public void LoadRaceData(string FileName = "GameData.xml")
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(GameStateData));
            FileStream Stream = new FileStream(FileName, FileMode.Open);
            GameState = Serializer.Deserialize(Stream) as GameStateData;
            Stream.Close();

        }
        // Update is called once per frame
        void Update()
        {

            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if(GameState.FirstPlaceID == -1)
                {
                    GameObject.FindGameObjectWithTag("first").GetComponent<TMPro.TextMeshProUGUI>().text = "Your previous fastest time was better by " + Mathf.Abs(GameState.SecondRaceTime - GameState.FirstRaceTime) + " seconds";
                }
                else
                {
                    if(GameObject.FindGameObjectWithTag("PlayerCountTracker").GetComponent<PlayerSelectionManager>().numPlayerOption == NumPlayerOption.players1)
                    {
                        //GameObject.FindGameObjectWithTag("first").GetComponent<TMPro.TextMeshProUGUI>().text = "Your new time was faster than previous fastest time by  " + ( GameState.FirstRaceTime - GameState.oldGhostTime ) + " seconds ";
                        GameObject.FindGameObjectWithTag("first").GetComponent<TMPro.TextMeshProUGUI>().text = "Your new time was faster than previous fastest time by  " + Mathf.Abs(GameState.oldGhostTime - GameState.FirstRaceTime) + " seconds ";
                    }
                    else
                    GameObject.FindGameObjectWithTag("first").GetComponent<TMPro.TextMeshProUGUI>().text = "First Place was player " + GameState.FirstPlaceID + " with a time of " + GameState.FirstRaceTime;
                    if (GameState.SecondPlaceID != 0)
                        GameObject.FindGameObjectWithTag("second").GetComponent<TMPro.TextMeshProUGUI>().text = "Second Place was player " + GameState.SecondPlaceID + " with a time of " + GameState.SecondRaceTime;
                    if (GameState.ThirdPlaceID != 0)
                        GameObject.FindGameObjectWithTag("third").GetComponent<TMPro.TextMeshProUGUI>().text = "Third Place was player " + GameState.ThirdPlaceID + " with a time of " + GameState.ThirdRaceTime;
                    if (GameState.FourthPlaceID != 0)
                        GameObject.FindGameObjectWithTag("fourth").GetComponent<TMPro.TextMeshProUGUI>().text = "Fourth Place was player " + GameState.FourthPlaceID + " with a time of " + GameState.FourthRaceTime;

                }

                if (Input.anyKey || Input.GetButton("A_P1"))
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }

}

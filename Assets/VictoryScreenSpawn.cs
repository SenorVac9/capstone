using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;
using ModuloKart.Controls;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class VictoryScreenSpawn : MonoBehaviour
{
    [SerializeField]
    Transform First, Second, Third, Fourth;
    SaveGameManager SaveManager;
    [SerializeField]
    GameObject Felix, Maxine, Toby, Paul;
    AVerySimpleEnumOfCharacters character;
    

    // Start is called before the first frame update
    void Start()
    {
      

        SaveManager = GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>();
       
    }

   

    // Update is called once per frame
    void Update()
    {
        if (SaveManager.GameState.P1Character != AVerySimpleEnumOfCharacters.NotInGame)
        {
            character = SaveManager.GameState.P1Character;
            switch (character)
            {
                case AVerySimpleEnumOfCharacters.Maxine:
                    Instantiate(Maxine, First.position, First.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.First_The_Worst:
                    Instantiate(Felix, First.position, First.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Second_The_Best:
                    Instantiate(Toby, First.position, First.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Third_The_One_With_The_Hairy_Chest:
                    Instantiate(Paul, First.position, First.rotation);
                    break;

            }
        }
        if (SaveManager.GameState.P2Character != AVerySimpleEnumOfCharacters.NotInGame)
        {
            character = SaveManager.GameState.P2Character;
            switch (character)
            {
                case AVerySimpleEnumOfCharacters.Maxine:
                    Instantiate(Maxine, Second.position, Second.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.First_The_Worst:
                    Instantiate(Felix, Second.position, Second.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Second_The_Best:
                    Instantiate(Toby, Second.position, Second.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Third_The_One_With_The_Hairy_Chest:
                    Instantiate(Paul, Second.position, Second.rotation);
                    break;

            }
        }
        if (SaveManager.GameState.P3Character != AVerySimpleEnumOfCharacters.NotInGame)
        {
            character = SaveManager.GameState.P3Character;
            switch (character)
            {
                case AVerySimpleEnumOfCharacters.Maxine:
                    Instantiate(Maxine, Third.position, Third.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.First_The_Worst:
                    Instantiate(Felix, Third.position, Third.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Second_The_Best:
                    Instantiate(Toby, Third.position, Third.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Third_The_One_With_The_Hairy_Chest:
                    Instantiate(Paul, Third.position, Third.rotation);
                    break;

            }
        }
        if (SaveManager.GameState.P4Character != AVerySimpleEnumOfCharacters.NotInGame)
        {
            character = SaveManager.GameState.P4Character;
            switch (character)
            {
                case AVerySimpleEnumOfCharacters.Maxine:
                    Instantiate(Maxine, Fourth.position, Fourth.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.First_The_Worst:
                    Instantiate(Felix, Fourth.position, Fourth.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Second_The_Best:
                    Instantiate(Toby, Fourth.position, Fourth.rotation);
                    break;
                case AVerySimpleEnumOfCharacters.Third_The_One_With_The_Hairy_Chest:
                    Instantiate(Paul, Fourth.position, Fourth.rotation);
                    break;

            }
        }

    }
}

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
    bool hasSpawned;

    // Start is called before the first frame update
    void Start()
    {
      

        SaveManager = GameObject.FindGameObjectWithTag("SaveGameManager").GetComponent<SaveGameManager>();
       
    }

   

    // Update is called once per frame
    void Update()
    {

        if (!hasSpawned)
        {
            if (SaveManager.GameState.P1Character != AVerySimpleEnumOfCharacters.NotInGame)
            {
                character = SaveManager.GameState.P1Character;
                switch (character)
                {
                    case AVerySimpleEnumOfCharacters.Felix:
                        AudioManager.instance.Play("Felix_Victory_Podium");
                        Instantiate(Felix, First.position, First.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Toby:
                        AudioManager.instance.Play("Tobias_Victory_Podium");
                        Instantiate(Toby, First.position, First.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Maxine:
                        AudioManager.instance.Play("Max_Victory_Podium");
                        Instantiate(Maxine, First.position, First.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Paul:
                        AudioManager.instance.Play("Pauline_Victory_Podium");
                        Instantiate(Paul, First.position, First.rotation);
                        break;

                }
            }
            if (SaveManager.GameState.P2Character != AVerySimpleEnumOfCharacters.NotInGame)
            {
                character = SaveManager.GameState.P2Character;
                switch (character)
                {
                    case AVerySimpleEnumOfCharacters.Felix:
                        Instantiate(Felix, Second.position, Second.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Toby:
                        Instantiate(Toby, Second.position, Second.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Maxine:
                        Instantiate(Toby, Second.position, Second.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Paul:
                        Instantiate(Paul, Second.position, Second.rotation);
                        break;

                }
            }
            if (SaveManager.GameState.P3Character != AVerySimpleEnumOfCharacters.NotInGame)
            {
                character = SaveManager.GameState.P3Character;
                switch (character)
                {
                    case AVerySimpleEnumOfCharacters.Felix:
                        Instantiate(Felix, Third.position, Third.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Toby:
                        Instantiate(Toby, Third.position, Third.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Maxine:
                        Instantiate(Toby, Third.position, Third.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Paul:
                        Instantiate(Paul, Third.position, Third.rotation);
                        break;

                }
            }
            if (SaveManager.GameState.P4Character != AVerySimpleEnumOfCharacters.NotInGame)
            {
                character = SaveManager.GameState.P4Character;
                switch (character)
                {
                    case AVerySimpleEnumOfCharacters.Felix:
                        Instantiate(Felix, Fourth.position, Fourth.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Toby:
                        Instantiate(Toby, Fourth.position, Fourth.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Maxine:
                        Instantiate(Toby, Fourth.position, Fourth.rotation);
                        break;
                    case AVerySimpleEnumOfCharacters.Paul:
                        Instantiate(Paul, Fourth.position, Fourth.rotation);
                        break;

                }
            }
            hasSpawned = true;
        }

       
    }
}

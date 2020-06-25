using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;
using ModuloKart.HUD;

public class Nitro_effect : MonoBehaviour
{

    VehicleBehavior vehicleBehavior;
    SimpleUI simpleUI;
    public ParticleSystem nitro_P;
    int PlayerID;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
        simpleUI = GameObject.FindObjectOfType<SimpleUI>();
    }

    // Update is called once per frame
    void Update()
    {

        switch (simpleUI.PlayerID)
        {
            case 0:
                if (Input.GetButton("A_ANYPLAYER"))
                {
                    nitro_P.Stop();

                    if (!nitro_P.isEmitting)
                    {
                        // vehicleBehavior.GetComponent<VehicleBehavior>();
                        nitro_P.Play();
                        Debug.Log("nitro effect playing N p1");
                    }



                }
                else
                {
                    nitro_P.Stop();


                }
                break;
            case 1:
                if (Input.GetButtonDown("A_ANYPLAYER"))
                {
                    nitro_P.Stop();

                    if (!nitro_P.isEmitting)
                    {
                        // vehicleBehavior.GetComponent<VehicleBehavior>();
                        nitro_P.Play();
                        Debug.Log("nitro effect playing N p1");
                    }



                }
                else
                {
                    nitro_P.Stop();


                }
                break;

            case 2:
                if (Input.GetButtonDown("A_ANYPLAYER"))
                {
                    nitro_P.Stop();

                    if (!nitro_P.isEmitting)
                    {
                        // vehicleBehavior.GetComponent<VehicleBehavior>();
                        nitro_P.Play();
                        Debug.Log("nitro effect playing N p1");
                    }



                }
                else
                {
                    nitro_P.Stop();


                }
                break;

            case 3:
                if (Input.GetButtonDown("A_ANYPLAYER"))
                {
                    nitro_P.Stop();

                    if (!nitro_P.isEmitting)
                    {
                        // vehicleBehavior.GetComponent<VehicleBehavior>();
                        nitro_P.Play();
                        Debug.Log("nitro effect playing N p1");
                    }



                }
                else
                {
                    nitro_P.Stop();


                }
                break;

            default:
                Debug.Log("Nitro Particle Notworking");
                break;
        }
    }
}
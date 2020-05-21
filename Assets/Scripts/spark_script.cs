using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;



public class spark_script : MonoBehaviour
{
    ui_controller ui;
    VehicleBehavior vehicleBehavior;
   // public Transform Spark_Spawnpoint_Rear_Right, Spark_Spawnpoint_Rear_Left, Spark_Spawnpoint_Front_Right, Spark_Spawnpoint_Front_Left;
    public new ParticleSystem spark_forward_RR, spark_forward_RL, spark_forward_FR, spark_forward_FL, spark_reverse_RR,spark_reverse_RL, spark_reverse_FR, spark_reverse_FL;
    public bool isforward, isreverse;


    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
        ui = GameObject.FindObjectOfType<ui_controller>();


    }

    // Update is called once per frame
    void Update()
    {

        //wheel1
        if (ui.has_tire_1 == false)
        {
            if (vehicleBehavior.accel_magnitude_float > 0)
            {
                isforward = true;
                isreverse = false;


                spark_reverse_FR.Stop();

                if (!spark_forward_FR.isEmitting)
                {
                    spark_forward_FR.Play();
                    Debug.LogError("shooting forward sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else if (vehicleBehavior.accel_magnitude_float < 0)
            {
                isreverse = true;
                isforward = false;
                spark_forward_FR.Stop();
                if (!spark_reverse_FR.isEmitting)
                {
                    spark_reverse_FR.Play();
                    Debug.LogError("shooting reverse sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else
            {
                spark_reverse_FR.Stop();
                spark_forward_FR.Stop();
                Debug.LogError("Not shooting sparks");
            }




        }
        if (ui.has_tire_2 == false)
        {
            if (vehicleBehavior.accel_magnitude_float > 0)
            {
                isforward = true;
                isreverse = false;


                spark_reverse_FL.Stop();

                if (!spark_forward_FL.isEmitting)
                {
                    spark_forward_FL.Play();
                    Debug.LogError("shooting forward sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else if (vehicleBehavior.accel_magnitude_float < 0)
            {
                isreverse = true;
                isforward = false;
                spark_forward_FL.Stop();
                if (!spark_reverse_FL.isEmitting)
                {
                    spark_reverse_FL.Play();
                    Debug.LogError("shooting reverse sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else
            {
                spark_reverse_FL.Stop();
                spark_forward_FL.Stop();
                Debug.LogError("Not shooting sparks");
            }




        }
        if (ui.has_tire_3 == false)
        {
            if (vehicleBehavior.accel_magnitude_float > 0)
            {
                isforward = true;
                isreverse = false;


                spark_reverse_RR.Stop();

                if (!spark_forward_RR.isEmitting)
                {
                    spark_forward_RR.Play();
                    Debug.LogError("shooting forward sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else if (vehicleBehavior.accel_magnitude_float < 0)
            {
                isreverse = true;
                isforward = false;
                spark_forward_RR.Stop();
                if (!spark_reverse_RR.isEmitting)
                {
                    spark_reverse_RR.Play();
                    Debug.LogError("shooting reverse sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else
            {
                spark_reverse_RR.Stop();
                spark_forward_RR.Stop();
                Debug.LogError("Not shooting sparks");
            }




        }
        if (ui.has_tire_4 == false)
        {
            if (vehicleBehavior.accel_magnitude_float > 0)
            {
                isforward = true;
                isreverse = false;


                spark_reverse_RL.Stop();

                if (!spark_forward_RL.isEmitting)
                {
                    spark_forward_RL.Play();
                    Debug.LogError("shooting forward sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else if (vehicleBehavior.accel_magnitude_float < 0)
            {
                isreverse = true;
                isforward = false;
                spark_forward_RL.Stop();
                if (!spark_reverse_RL.isEmitting)
                {
                    spark_reverse_RL.Play();
                    Debug.LogError("shooting reverse sparks");
                }
                else
                {
                    Debug.LogWarning("forward sparks already shooting");
                }
            }
            else
            {
                spark_reverse_RL.Stop();
                spark_forward_RL.Stop();
                Debug.LogError("Not shooting sparks");
            }




        }



        /* if (ui.has_tire_1 == false && vehicleBehavior.accel_magnitude_float < 0)
         {
             isreverse = true;
             isforward = false;
             int cnt = 0;
             if (isreverse == true && cnt==0)
             {
                 spark_forward.Stop();
                 cnt++;

                 Instantiate(spark_reverse, Spark_Spawnpoint_Front_Right.transform);
                 Debug.Log("shooting sparks");
             }

         }
        if (vehicleBehavior.accel_magnitude_float==0 && isforward==false && isreverse==false)
        {
            spark_reverse.Stop();
            spark_forward.Stop();
            Debug.Log("Not shooting sparks");
        }

        //wheel2
        if (ui.has_tire_2 == false && vehicleBehavior.accel_magnitude_float > 0)
        {
            isforward = true;
            isreverse = false;
            int cnt = 0;
            if (isforward == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_forward, Spark_Spawnpoint_Front_Left.transform);
                Debug.Log("shooting sparks");
            }
        }
        if (ui.has_tire_2 == false && vehicleBehavior.accel_magnitude_float < 0)
        {
            isreverse = true;
            isforward = false;
            int cnt = 0;
            if (isreverse == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_reverse, Spark_Spawnpoint_Front_Left.transform);
                Debug.Log("shooting sparks");
            }
        }
        if(vehicleBehavior.accel_magnitude_float == 0)
        {
            spark_reverse.Stop();
            spark_forward.Stop();
            Debug.Log("not shooting sparks");
        }

        //wheel3
        if (ui.has_tire_3 == false && vehicleBehavior.accel_magnitude_float > 0)
        {
            isforward = true;
            isreverse = false;
            int cnt = 0;
            if (isforward == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_forward, Spark_Spawnpoint_Rear_Right.transform);
                Debug.Log("shooting sparks");
            }
        }
        if (ui.has_tire_3 == false && vehicleBehavior.accel_magnitude_float < 0)
        {
            isreverse = true;
            isforward = false;
            int cnt = 0;
            if (isreverse == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_reverse, Spark_Spawnpoint_Rear_Right.transform);
                Debug.Log("shooting sparks");
            }
        }
        if (vehicleBehavior.accel_magnitude_float == 0)
        {
            spark_reverse.Stop();
            spark_forward.Stop();
            Debug.Log("not shooting sparks");
        }

        //wheel4
        if (ui.has_tire_4 == false && vehicleBehavior.accel_magnitude_float > 0)
        {
            isforward = true;
            isreverse = false;
            int cnt = 0;
            if (isforward == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_forward, Spark_Spawnpoint_Rear_Left.transform);
                Debug.Log("shooting sparks");
            }
        }
        if (ui.has_tire_4 == false && vehicleBehavior.accel_magnitude_float < 0)
        {
            isreverse = true;
            isforward = false;
            int cnt = 0;
            if (isreverse == true && cnt == 0)
            {
                spark_reverse.Stop();
                spark_forward.Stop();
                cnt++;
                Instantiate(spark_reverse, Spark_Spawnpoint_Rear_Left.transform);
                Debug.Log("shooting sparks");
            }
        }
        if (vehicleBehavior.accel_magnitude_float == 0)
        {
            spark_reverse.Stop();
            spark_forward.Stop();
            Debug.Log("not shooting sparks");
        }
        */






        // impact particle effect





        /* for (int i = 0; i <= 3;i++)
         {
             SparkPlay(tirenum[i], sparks_spawnpoint[i]);
         }
     } 

     public void SparkPlay(bool game,Transform mytransform)
     {
         if (game ==false && vehicleBehavior.accel_magnitude_float > 0)
         {
             Instantiate(sparks, mytransform.transform);
         }
         else if (game == false && vehicleBehavior.accel_magnitude_float < 0)
         {
             mytransform.rotation = Quaternion.Euler(0, 0, 0);
             Instantiate(sparks, mytransform.transform);
         }
         else
         {
             sparks.Stop();
         }
     } */

    }
}

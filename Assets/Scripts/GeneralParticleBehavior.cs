using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public enum ParticleType
{
    CarTireParticle,
    //CarExhaustParticle,
    CarRimSparkParticle_rear_left,
    CarRimSparkParticle_rear_right,
    CarRimSparkParticle_front_left,
    CarRimSparkParticle_front_right

}
public class GeneralParticleBehavior : MonoBehaviour
{
    ui_controller ui;
    public float multiplier = 1;
    [SerializeField] public VehicleBehavior vehicleBehavior;
    public ParticleType particleType;
    public new ParticleSystem particleSystem;
    public ParticleSystem.MainModule mainModule;
   // public Color DirtColor;
    //public Color StopColor;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        vehicleBehavior = GameObject.FindObjectOfType<VehicleBehavior>();
        ui = GameObject.FindObjectOfType<ui_controller>();
        mainModule = particleSystem.main;
        mainModule.startSizeMultiplier *= multiplier;
        mainModule.startSpeedMultiplier *= multiplier;
        mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
        particleSystem.Clear();
        mainModule.loop = true;
        particleSystem.Play();

    }

    private void Update()
    {
        switch (particleType)
        {
            case ParticleType.CarTireParticle:
                //if (particleSystem == null) break;

                if (vehicleBehavior.is_drift== true)
                {
                    if (!mainModule.loop)
                    {
                        //particleSystem.emission.rateOverTime = 20;
                        //particleSystem.Clear();
                        mainModule.loop = true;
                        particleSystem.Play();
                    }
                }
                else
                {
                    //particleSystem.Clear();
                    if (mainModule.loop)
                    {
                        //particleSystem.emission.rateOverTime = 5;
                        //particleSystem.Clear();
                        mainModule.loop = false;
                        particleSystem.Stop();
                    }
                }
                break;

            case ParticleType.CarRimSparkParticle_rear_left:
                {
                    if (ui.has_tire_4 == false && vehicleBehavior.accel_magnitude_float != 0)
                    {
                        if (!mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 20;
                            //particleSystem.Clear();
                            mainModule.loop = true;
                            particleSystem.Play();
                        }
                    }
                    else 
                    {
                        //particleSystem.Clear();
                        if (mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 5;
                            //particleSystem.Clear();
                            mainModule.loop = false;
                            particleSystem.Stop();
                        }
                    }
                    break;

                }
            case ParticleType.CarRimSparkParticle_rear_right    :
                {
                    if (ui.has_tire_3 == false && vehicleBehavior.accel_magnitude_float != 0)
                    {
                        if (!mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 20;
                            //particleSystem.Clear();
                            mainModule.loop = true;
                            particleSystem.Play();
                        }
                    }
                    else 
                    {
                        //particleSystem.Clear();
                        if (mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 5;
                            //particleSystem.Clear();
                            mainModule.loop = false;
                            particleSystem.Stop();
                        }
                    }

                    break;

                }

            case ParticleType.CarRimSparkParticle_front_left:
                {
                    if (ui.has_tire_2 == false && vehicleBehavior.accel_magnitude_float != 0)
                    {
                        if (!mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 20;
                            //particleSystem.Clear();
                            mainModule.loop = true;
                            particleSystem.Play();
                        }
                    }
                    else 
                    {
                        //particleSystem.Clear();
                        if (mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 5;
                            //particleSystem.Clear();
                            mainModule.loop = false;
                            particleSystem.Stop();
                        }
                    }
                    break;

                }
            case ParticleType.CarRimSparkParticle_front_right:
                {
                    if (ui.has_tire_1 == false && vehicleBehavior.accel_magnitude_float != 0)
                    {
                        if (!mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 20;
                            //particleSystem.Clear();
                            mainModule.loop = true;
                            particleSystem.Play();
                        }
                    }
                    else 
                    {
                        //particleSystem.Clear();
                        if (mainModule.loop)
                        {
                            //particleSystem.emission.rateOverTime = 5;
                            //particleSystem.Clear();
                            mainModule.loop = false;
                            particleSystem.Stop();
                        }
                    }
                    break;

                }
            default:
                break;
        }
    }
}

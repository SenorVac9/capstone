using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public enum ParticleType
{
    CarTireParticle,
    CarExhaustParticle,
}
public class GeneralParticleBehavior : MonoBehaviour
{
    public float multiplier = 1;
    [SerializeField] private VehicleBehavior vehicleBehavior;
    public ParticleType particleType;
    public new ParticleSystem particleSystem;
    public ParticleSystem.MainModule mainModule;
    public Color DirtColor;
    public Color StopColor;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
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

                if (vehicleBehavior.accel_magnitude_float > 1)
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
            case ParticleType.CarExhaustParticle:
                break;
            default:
                break;
        }
    }
}

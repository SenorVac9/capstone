using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public class NitroBoost : MonoBehaviour
{
    VehicleBehavior vehicleBehavior;
    public bool isSpeedboost = false;
    public float speedBoost_float = 10;
    public float max_speedboost_float = 60;
    public float speedboost_meter_float = 50;
    public float startTime = 0;
    public float timeLimit = 5;
    public float timetaken = 0;
    public float target_speedboost_accel;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = FindObjectOfType<VehicleBehavior>();
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("GameController"))
        {
            //vehicleBehavior.nitros_meter_float += 50;            
            //StartCoroutine(BoostOver());
            if (!isSpeedboost)
            {

                isSpeedboost = true;
                target_speedboost_accel = vehicleBehavior.target_accel_modified + max_speedboost_float;
                //
                StartCoroutine(TimeLimiter());

            }
        }
    }

    IEnumerator TimeLimiter()
    {
        timetaken = 0;
        while (timetaken < timeLimit)
        {

            vehicleBehavior.target_accel_modified = target_speedboost_accel;
            timetaken += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
            if (timetaken >= timeLimit)
                isSpeedboost = false;
            Debug.Log("is off" + isSpeedboost);
        }


    }

}

/*
 speedBoost_float = max_speedboost_float;
            if (vehicleBehavior.accel_magnitude_float > max_speedboost_float)
            {
                vehicleBehavior.accel_magnitude_float = vehicleBehavior.accel_magnitude_float - vehicleBehavior.DRAG * Time.fixedDeltaTime > max_speedboost_float ? vehicleBehavior.accel_magnitude_float -= vehicleBehavior.DRAG * Time.fixedDeltaTime : max_speedboost_float;
            }
            else
            {
                vehicleBehavior.accel_magnitude_float = vehicleBehavior.accel_magnitude_float + speedBoost_float * Time.fixedDeltaTime < max_speedboost_float ? vehicleBehavior.accel_magnitude_float += speedBoost_float * Time.fixedDeltaTime : speedBoost_float;
            }
            if (vehicleBehavior.isPostProfile)
            {
                if (vehicleBehavior.is_MotionBlur) vehicleBehavior.vehicle_camera_postprocess_behavior.profile.motionBlur.enabled = true;
            }
            //}

            if (vehicleBehavior.is_Cinematic_View)
            {
                if (vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView < vehicleBehavior.max_fov_float)
                {
                    vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView += Time.fixedDeltaTime * vehicleBehavior.pan_away_float;
                }
            }
            speedboost_meter_float = speedboost_meter_float > 0 ? speedboost_meter_float -= Time.fixedDeltaTime * vehicleBehavior.nitros_depletion_rate : 0;
            if (vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView > vehicleBehavior.min_fov_float)
            {
                vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView -= Time.fixedDeltaTime * vehicleBehavior.pan_toward_float;
            }
            return;
            //speedboost_meter_float = speedboost_meter_float > 0 ? speedboost_meter_float -= Time.fixedDeltaTime * vehicleBehavior.nitros_depletion_rate : 0;
        }
        if (isSpeedboost)
        {
            if (vehicleBehavior.isPostProfile)
            {
                if (vehicleBehavior.is_MotionBlur) vehicleBehavior.vehicle_camera_postprocess_behavior.profile.motionBlur.enabled = false;
            }
            if (vehicleBehavior.is_Cinematic_View)
            {
                if (vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView > vehicleBehavior.min_fov_float)
                {
                    vehicleBehavior.vehicle_camera_transform.GetComponent<Camera>().fieldOfView -= Time.fixedDeltaTime * vehicleBehavior.pan_toward_float;
                }
            }
            isSpeedboost = false;
        }
 */

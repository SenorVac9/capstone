using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using ModuloKart.HUD;
using Assets.MultiAudioListener;

namespace ModuloKart.CustomVehiclePhysics
{
    public enum InputType
    {
        KeyboardAndMouse,
        Xbox,
    }

    [RequireComponent(typeof(Rigidbody))]
    public class VehicleBehavior : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] public bool isCodeDebug = false;
        public GameObject wheel1, wheel2, wheel3, wheel4, hood;

        public bool isEditorGUI = false;
        public bool keepTabsOpen;
        public bool showRunTimeVariablesOnly;
        public SimpleUI playerHUD;
        //Car Engine Audio
        private float pitch = 0;
        #region Public Variables
        [Header("Vehicle Components")]
        public Transform vehicle_transform;
        public Rigidbody vehicle_rigidbody;
        public Transform vehicle_heading_transform;
        public Transform vehicleSpinOutTransform;
        public Transform vehicle_model_transform;
        public Transform vehicle_camera_transform;
        public PostProcessingBehaviour vehicle_camera_postprocess_behavior;
        public PostProcessingProfile vehicle_camera_profile;
        private bool isPostProfile;

        public Transform axel_rr_transform;
        public Transform axel_rl_transform;
        public Transform axel_fr_transform;
        public Transform axel_fl_transform;

        [Header("Vehicle Physical Attributes")]
        public float width_float = 4f;
        public float length_float = 6f;
        public float height_float = 2f;
        public bool is_4wd = false;
        public AVerySimpleEnumOfCharacters selectedCharacter;

        [Header("Cinematics")]
        public bool is_Cinematic_View;
        [Range(60, 110)] public float min_fov_float = 90;
        [Range(110, 160)] public float max_fov_float = 130;
        [Range(5, 50)] public float pan_away_float = 10;
        [Range(5, 50)] public float pan_toward_float = 30;
        public bool is_MotionBlur;




        [Header("[Run-Time] Vehicle Movement Variables")]
        public bool is_grounded;
        [SerializeField] public Ray[] ground_check_ray = new Ray[5];
        [SerializeField] public RaycastHit[] groundCheck_hits = new RaycastHit[255];
        public float gravity_float = 0;
        [SerializeField] public float target_accel_modified;
        [SerializeField] public float wheel_steer_float;
        [SerializeField] public float target_steer_modified;
        public float accel_magnitude_float = 0;
        public float steer_magnitude_float = 0;
        public float brake_magnitude_float = 0;
        public float drift_correction_float = 0;
        public float nitros_meter_float = 0;
        public float nitros_speed_float = 0;
        [Tooltip("How responsive the slope tilt adjustment is. This is a function of vehicle speed modified in runtime")]
        [SerializeField] public float tiltLerp_float;
        public float vehicle_air_height = 0;
        public float vehicle_air_control = 0;
        public float vehicle_speed_turn_ratio = 0;

        public bool isReverse;
        public bool is_drift;
        public bool is_nitrosboost;
        [Header("Vehicle Spin Out")]
        public bool hasVehicleControl = false;


        [Header("Maximum and Minimum Vehicle Movement Values")]
        [Range(0, 500)] public float max_gravity_float = 250f;
        [Range(0, 50)] public float max_steer_float = 15f;
        [Range(0, 50)] public float min_steer_float = 1f;
        [Range(0, 500)] public float max_accel_float = 250f;
        [Range(-500, 0)] public float min_accel_float = -69;
        [Range(0, 500)] public float max_brake_float = 100f;
        [Range(0, 1)] public float min_drift_correction_float = 0.05f;
        [Range(0, 1)] public float max_drift_correction_float = 1f;
        [Tooltip("Speed at which Model Rotation snaps away from the Heading Rotation")]
        [Range(0, 0.1f)] public float drift_correction_multiplier = 0.05f;
        [Tooltip("Speed at which Model Rotation snaps back to the Heading Rotation")]
        [Range(0, 0.1f)] public float drift_accel_multiplier_float = 0.05f;
        [Tooltip("Turn Angle multiplier when drifting")]
        [Range(0, 5f)] public float drift_turn_ratio_float = 0.25f;
        [Range(0, 100f)] public float max_nitros_meter_float = 100f;
        [Range(0, 100f)] public float max_nitros_speed_float = 100f;
        [Range(0, 20f)] public float nitros_depletion_rate = 2.5f;
        [Range(0, 1)] public float min_air_control = 0.1f;


        [Header("Layers Used for Ground Check")]
        [Tooltip("Make sure your vehicle layers are not included as it will trick the vehicle into being grounded when it is not")]
        [SerializeField] public LayerMask rayCast_layerMask = ~(1 << 1 | 1 << 2 | 1 << 8);
        [Tooltip("This is for how far off the ground do we detect and adjust the angle of the car based on terrain slopes")]
        [Range(10, 10000f)] public float slope_ray_dist_float;
        #endregion

        #region 'Fixed' values, but serialized to allow tweaking of variables
        [Header("Input Responsiveness")]
        [Tooltip("Higher ACCEL Values are more responsive")]
        [Range(0, 100)] [SerializeField] public float ACCEL = 50f;
        [Tooltip("Higher REVERSE Values are more responsive")]
        [Range(0, 100)] [SerializeField] public float REVERSE = 25;
        [Tooltip("Higher DRAG Values are more responsive")]
        [Range(0, 100)] [SerializeField] public float DRAG = 25f;
        [Tooltip("Higher DRAG Values are more responsive")]
        [Range(0, 100)] [SerializeField] public float ROTATIONAL_DRAG = 25f;
        [Tooltip("Higher GRAVITY Values are more responsive")]
        [Range(1, 2000)] [SerializeField] public float GRAVITY = 1000f;
        [Tooltip("Higher STEER Values are more responsive")]
        [Range(1, 30)] [SerializeField] public float STEER = 15f;
        [Tooltip("Higher STEER_DECELERATION Values decrease max turning speed")]
        [Range(1, 50)] [SerializeField] public float STEER_DECELERATION = 10f;
        [Tooltip("Higher DRIFT_ACCELERATION Values increase max drifting speed")]
        [Range(1, 50)] [SerializeField] public float DRIFT_ACCELERATION = 40f;
        [Range(0, 50)] [SerializeField] public float DRIFT_STEER_DAMPEN = 10f;
        #endregion

        #region InputSelection
        [Header("InputType")]
        public InputType input_type_enum = InputType.KeyboardAndMouse;
        [Range(1, 4f)] public int PlayerID;
        [Range(-1, 20f)] public int JoyStick;
        public bool isControllerInitialized;
        public string input_steering = "LeftJoyStickX_P";
        public string input_projectile = "X_P";
        public string input_accelerate = "RightTrigger_P";
        public string input_reverse = "LeftTrigger_P";
        public string input_drift = "B_P";
        public string input_nitros = "A_P";
        public string input_ItemPrev = "LeftBumper_P";
        public string input_ItemNext = "RightBumper_P";
        #endregion

        //Transform tempTransform;

        private void Start()
        {
            //Caching variables
            //tempTransform = GameObject.FindGameObjectWithTag("TempTransform").transform;
            //vehicle_transform = GetComponent<Transform>();
            //vehicle_rigidbody = GetComponent<Rigidbody>();
            //vehicle_heading_transform = vehicle_transform.GetChild(0).GetChild(0);
            //vehicle_model_transform = vehicle_transform.GetChild(0).GetChild(1);
            //axel_rr_transform = vehicle_model_transform.GetChild(1).GetChild(0).GetChild(0);
            //axel_rl_transform = vehicle_model_transform.GetChild(1).GetChild(0).GetChild(1);
            //axel_fr_transform = vehicle_model_transform.GetChild(1).GetChild(0).GetChild(2);
            //axel_fl_transform = vehicle_model_transform.GetChild(1).GetChild(0).GetChild(3);

            vehicle_camera_transform = vehicle_heading_transform.GetChild(0);

            if (vehicle_heading_transform.GetChild(0).GetComponent<PostProcessingBehaviour>() != null)
            {
                vehicle_camera_postprocess_behavior = vehicle_heading_transform.GetChild(0).GetComponent<PostProcessingBehaviour>();
                vehicle_camera_profile = vehicle_camera_postprocess_behavior.profile;
                isPostProfile = true;
            }
            groundCheck_hits = new RaycastHit[255];
            ground_check_ray = new Ray[255];

            input_steering = "Horizontal";
            input_accelerate = "Vertical";
            input_drift = "Jump";
            input_nitros = "NitroKey";

            hasVehicleControl = true;

        }


        //vehicle performace loss with track collsions
        //just add yags to colliders and keep the code as it is

        int isCollisionHit;
        public void OnCollisionEnter(Collision c)
        {
            if (!GameManager.Instance.GameStart) return;

            if (c.gameObject.tag == "TrackColliders")

            {


                Vector3 tempReflect = Vector3.Reflect(vehicle_heading_transform.forward, c.contacts[0].normal);
                isCollisionHit = 1;
                if (Vector3.Dot(vehicle_heading_transform.forward, c.contacts[0].normal) <= 0.75)
                {
                    accel_magnitude_float *= 0.25f;
                    //Vector3 reflectXZDirection = new Vector3()
                    //vehicle_heading_transform.forward = Vector3.Reflect(vehicle_heading_transform.forward, c.contacts[0].normal);
                    //vehicle_heading_transform.forward = Vector3.Lerp(vehicle_heading_transform.forward, new Vector3(tempReflect.x, 0, tempReflect.z),0.1f);
                    //vehicle_heading_transform.forward = new Vector3(tempReflect.x, 0, tempReflect.z);
                    Debug.Log("hit non direct collision (at an angle)");



                }
                else if (Vector3.Dot(vehicle_heading_transform.forward, c.contacts[0].normal) > 0.75)
                {
                    accel_magnitude_float *= 0.1f;
                    //vehicle_heading_transform.forward = Vector3.Reflect(vehicle_heading_transform.forward, c.contacts[0].normal);
                    //vehicle_heading_transform.forward = Vector3.Lerp(vehicle_heading_transform.forward, new Vector3(tempReflect.x, 0, tempReflect.z), 0.1f);
                    //vehicle_heading_transform.forward = new Vector3(tempReflect.x, 0, tempReflect.z);
                    Debug.Log("hit - more or less directly");
                }
            }
            isCollisionHit = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.StartsWith("MilkSpill"))
            {
                StartSpinOut();
            }
        }

        private void FixedUpdate()
        {
            //InitializePlayerJoystick();
            //if (!isControllerInitialized) return;
            if (!playerHUD.simpleCharacterSeleciton.isCharacterSelected) return;
            if (!GameManager.Instance.GameStart) return;

            VehicleGroundCheck();
            VehicleMovement();

            //Audio for the engine updating according to acceleration
            pitch = accel_magnitude_float / max_accel_float;
            GetComponent<MultiAudioSource>().Pitch = pitch;
            //if(wheel1.active == false && wheel2.active == true && wheel3.active == true && wheel4.active == true && hood.active==true)
            //{
            //    max_accel_float = 245.0f;
            //
            //}
            //else if (wheel2.active == false && wheel1.active == true && wheel3.active == true && wheel4.active == true && hood.active == true)
            //{
            //    max_accel_float = 245.0f;
            //
            //}
            //else if (wheel3.active == false && wheel1.active == true && wheel2.active == true && wheel4.active == true && hood.active == true)
            //{
            //    max_accel_float = 245.0f;
            //
            //}
            //else if (wheel4.active == false && wheel1.active == true && wheel2.active == true && wheel3.active == true && hood.active == true)
            //{
            //    max_accel_float = 245.0f;
            //
            //}
            //else if(wheel1.active == true && wheel2.active == true && wheel3.active == true && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 240.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == true && wheel4.active == true && hood.active == true)
            //{
            //    max_accel_float = 240.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == true && wheel3.active == false && wheel4.active == true && hood.active == true)
            //{
            //    max_accel_float = 240.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == true && wheel3.active == true && wheel4.active == false && hood.active == true)
            //{
            //    max_accel_float = 240.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == true && wheel3.active == true && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 235.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == false && wheel4.active == true && hood.active == true)
            //{
            //    max_accel_float = 235.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == true && wheel4.active == false && hood.active == true)
            //{
            //    max_accel_float = 235.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == true && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 230.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == false && wheel4.active == false && hood.active == true)
            //{
            //    max_accel_float = 230.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == false && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 225.0f;
            //
            //}
            //else if (wheel1.active == false && wheel2.active == false && wheel3.active == false && wheel4.active == false && hood.active == false)
            //{
            //    max_accel_float = 220.0f;
            //
            //}
            //else if (wheel1.active == true && wheel2.active == false && wheel3.active == true && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 230.0f;
            //
            //}
            //else if (wheel1.active == true && wheel2.active == true && wheel3.active == false && wheel4.active == true && hood.active == false)
            //{
            //    max_accel_float = 230.0f;
            //
            //}
            //else if (wheel1.active == true && wheel2.active == true && wheel3.active == true && wheel4.active == false && hood.active == false)
            //{
            //    max_accel_float = 230.0f;
            //
            //}

        }

        #region public methods to get and set variables on this script
        public float GetNitrosMeter()
        {
            return nitros_meter_float;
        }
        public void SetNitrosMeter(float value)
        {
            nitros_meter_float = value;
        }
        #endregion

        #region GroundCheck Methods
        private void VehicleGroundCheck()
        {
            //Middle Ray
            //Forward Right
            //Back Left
            //Forward Left
            //Back Right
            ground_check_ray[0] = new Ray(vehicle_transform.position, -tempUpVectorForSlopeMeasurement);
            ground_check_ray[1] = new Ray(vehicle_transform.position + tempForwardVectorForSlopeMeasurement * length_float + tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement);
            ground_check_ray[2] = new Ray(vehicle_transform.position - tempForwardVectorForSlopeMeasurement * length_float - tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement);
            ground_check_ray[3] = new Ray(vehicle_transform.position + tempForwardVectorForSlopeMeasurement * length_float - tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement);
            ground_check_ray[4] = new Ray(vehicle_transform.position - tempForwardVectorForSlopeMeasurement * length_float + tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement);

            is_grounded = VehicleGroundRaycast(ground_check_ray, height_float);
        }

        private bool VehicleGroundRaycast(Ray[] rays, float dist)
        {
            foreach (Ray ray in rays)
            {
                if (isCodeDebug)
                {
                    Debug.DrawRay(vehicle_transform.position, -tempUpVectorForSlopeMeasurement * height_float * 2, Color.red);
                    Debug.DrawRay(vehicle_transform.position + tempForwardVectorForSlopeMeasurement * length_float + tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement * (height_float + 0.1f), Color.red);
                    Debug.DrawRay(vehicle_transform.position - tempForwardVectorForSlopeMeasurement * length_float - tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement * (height_float + 0.1f), Color.red);
                    Debug.DrawRay(vehicle_transform.position + tempForwardVectorForSlopeMeasurement * length_float - tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement * (height_float + 0.1f), Color.red);
                    Debug.DrawRay(vehicle_transform.position - tempForwardVectorForSlopeMeasurement * length_float + tempRightVectorForSlopeMeasurement * width_float, -tempUpVectorForSlopeMeasurement * (height_float + 0.1f), Color.red);
                }
                if (Physics.RaycastNonAlloc(ray, groundCheck_hits, dist, rayCast_layerMask) > 0)
                {
                    if (isCodeDebug)
                    {
                        Debug.Log("hit: " + groundCheck_hits[0].transform.name);
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Movement Methods
        private void VehicleMovement()
        {
            //Get the Inputs first before any Movement is made
            VehicleAccelInput();
            VehicleSteerInput();

            //Steering
            VehicleSteerRotation();
            //Wheel Rotation
            RotateWheels();

            //Slope Tilt
            VehicleTiltSlope();

            //Nitros Boost
            VehicleNitrosBoostInput();

            SpinOutBehavior();


            vehicle_rigidbody.velocity = Vector3.zero;
            vehicle_rigidbody.useGravity = false;
            vehicle_rigidbody.useConeFriction = false;

            if (accel_magnitude_float == 0)
                vehicle_rigidbody.isKinematic = true;
            else
                vehicle_rigidbody.isKinematic = false;

            if (is_grounded)
            {
                if (!isJump)
                    gravity_float = 0;
                vehicle_transform.position += (vehicle_heading_transform.forward * accel_magnitude_float - Vector3.up * gravity_float) * Time.fixedDeltaTime;
            }
            else
            {
                if (!isJump)
                    gravity_float = gravity_float < max_gravity_float ? gravity_float += Time.fixedDeltaTime * GRAVITY : max_gravity_float;

                if (gravity_float > max_gravity_float)
                {
                    gravity_float = max_gravity_float;
                }
                vehicle_transform.position += (vehicle_heading_transform.forward * accel_magnitude_float - Vector3.up * gravity_float) * Time.fixedDeltaTime;
            }
            //Jump();
            if (isJump)
            {
                if (gravity_float > 0) gravity_float = 0;
                gravity_float -= Time.fixedDeltaTime * GRAVITY;
                if (gravity_float <= -250)
                {
                    isJump = false;
                }
            }
        }

        Vector3 vert;
        bool isJump;
        float jumpTime;
        public void Jump()
        {
            if (jumpTime<.1f)
            {
                jumpTime += Time.fixedDeltaTime;
                isJump = true;
                vert = Vector3.up * max_gravity_float * 10;
                //gravity_float = -max_gravity_float * 10;
                Debug.Log("Jump!: " + gravity_float);
            }
            else
            {
                jumpTime = 0;
                vert = Vector3.zero;
            }
        }

        #endregion

        #region Steer Rotation Methods
        bool isAudioDrift;
        public float totalRotationAmount;
        Quaternion tempFinalRotationXAlwaysNegative;
        private void VehicleSteerRotation()
        {
            if (!hasVehicleControl) return;

            //This is the direction vector we use to accelerate
            if (vehicle_air_height > 0)
            {
                vehicle_air_control = Mathf.Clamp(1 / vehicle_air_height, min_air_control, 1f);
            }
            else
            {
                vehicle_air_control = 1;
            }

            if (Mathf.Abs(accel_magnitude_float) > 0)
            {
                vehicle_speed_turn_ratio = Mathf.Clamp(Mathf.Abs(accel_magnitude_float) / 100, 0.1f, 1f);
            }
            else
            {
                vehicle_speed_turn_ratio = 0.1f;
            }
            totalRotationAmount = steer_magnitude_float * STEER * vehicle_air_control * vehicle_speed_turn_ratio;
            vehicle_heading_transform.rotation *= Quaternion.Euler(0, totalRotationAmount * Time.fixedDeltaTime, 0);

            //Initiate Drift Mode
            //if (Input.GetKey(KeyCode.Space) || Input.GetAxis("RightTrigger_P1") > 0)
            if (Input.GetKey(KeyCode.Space) || Input.GetButton(input_drift))
            {

                if (!isAudioDrift)
                {
                    isAudioDrift = true;
                    AudioManager.instance.Play("Drift");
                }
                if (!is_drift)
                {
                    drift_correction_float = 1f;

                    //20191025: We divide it instead of multiplying the 'drift_turn_ratio_float' to gain more steering ability when drift is on, not lose steering

                    //INITIAL STEERING BOOST WHEN DRIFT HAS INITIATED
                    target_steer_modified = max_steer_float / drift_turn_ratio_float;
                }

                is_drift = true;


                if (drift_correction_float > min_drift_correction_float)
                {
                    drift_correction_float -= drift_correction_multiplier;
                }
                else
                {
                    drift_correction_float = min_drift_correction_float;
                }

                //20191025: Increase Vehicle Steering Ability as drifting progresses
                //if (target_steer_modified < max_steer_float * DRIFT_STEER_DAMPEN)

                //WHILE DRIFTING OVER TIME, STEERING DECREASES VALUE
                if (target_steer_modified > min_steer_float * DRIFT_STEER_DAMPEN)
                {
                    //target_steer_modified += Time.fixedDeltaTime;
                    target_steer_modified -= Time.fixedDeltaTime * 5;
                    Debug.Log("decreasing steering: " + target_steer_modified);
                }
                else
                {
                    //target_steer_modified = max_steer_float * DRIFT_STEER_DAMPEN;
                    target_steer_modified = min_steer_float * DRIFT_STEER_DAMPEN;
                }

                if (!isReverse)
                {
                    //Calculation of Max speed is a function of: max_accel_modified - Steering Deceleration + Drifting Acceleration
                    if (is_nitrosboost)
                    {
                        if (target_accel_modified < max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION + nitros_speed_float)
                        {
                            //Just make this a factor of 10 in inspector
                            target_accel_modified += drift_accel_multiplier_float * 10;
                            if (target_accel_modified > max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION + nitros_speed_float)
                            {
                                target_accel_modified = max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION + nitros_speed_float;
                            }
                        }
                        else
                        {
                            //Just make this a factor of 10 in inspector
                            target_accel_modified -= drift_accel_multiplier_float * 10;
                            if (target_accel_modified < max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION + nitros_speed_float)
                            {
                                target_accel_modified = max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION + nitros_speed_float;
                            }
                        }
                    }
                    else
                    {
                        if (target_accel_modified > max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION)
                        {
                            //Drag should be new var 'Friction'
                            target_accel_modified -= ROTATIONAL_DRAG * Time.fixedDeltaTime;
                            if (target_accel_modified < max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION)
                            {
                                target_accel_modified = max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION;
                            }
                        }
                        else
                        {
                            //Drag should be new var 'Friction'
                            target_accel_modified += ROTATIONAL_DRAG * Time.fixedDeltaTime;
                            if (target_accel_modified > max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION)
                            {
                                target_accel_modified = max_accel_float - max_steer_float * STEER_DECELERATION + Mathf.Abs(steer_magnitude_float) * DRIFT_ACCELERATION;
                            }

                        }
                    }


                    Quaternion tempQ = vehicle_heading_transform.rotation;
                    float startdriftTime;

                    float tempSteerValue = Input.GetAxis(input_steering);
                    if (tempSteerValue < 0)
                    {
                        if (tempDriftModelRotationValue > -45)
                        {
                            tempDriftModelRotationValue -= Time.fixedDeltaTime * 150;
                        }
                        tempQ = vehicle_heading_transform.rotation * Quaternion.Euler(0, tempDriftModelRotationValue * Mathf.Abs(accel_magnitude_float / target_accel_modified), 0);
                    }
                    else if (tempSteerValue > 0)
                    {
                        if (tempDriftModelRotationValue < 45)
                        {
                            tempDriftModelRotationValue += Time.fixedDeltaTime * 150;
                        }
                        tempQ = vehicle_heading_transform.rotation * Quaternion.Euler(0, tempDriftModelRotationValue * Mathf.Abs(accel_magnitude_float / target_accel_modified), 0);
                    }
                    else
                    {
                        if (tempDriftModelRotationValue < 0)
                        {
                            tempDriftModelRotationValue += Time.fixedDeltaTime * 150;
                            if (tempDriftModelRotationValue > 0)
                                tempDriftModelRotationValue = 0;
                        }
                        else if (tempDriftModelRotationValue > 0)
                        {
                            tempDriftModelRotationValue -= Time.fixedDeltaTime * 150;
                            if (tempDriftModelRotationValue < 0)
                                tempDriftModelRotationValue = 0;
                        }
                        tempQ = vehicle_heading_transform.rotation * Quaternion.Euler(0, tempDriftModelRotationValue * Mathf.Abs(accel_magnitude_float / target_accel_modified), 0);
                    }

                    vehicle_model_transform.rotation = Quaternion.Lerp(tempQ, vehicle_model_transform.rotation, drift_correction_float);
                }
                else
                {
                    if (tempDriftModelRotationValue < 0)
                    {
                        tempDriftModelRotationValue += Time.fixedDeltaTime * 150;
                    }
                    else if (tempDriftModelRotationValue > 0)
                    {
                        tempDriftModelRotationValue -= Time.fixedDeltaTime * 150;
                    }
                    Quaternion tempQ = vehicle_heading_transform.rotation * Quaternion.Euler(0, tempDriftModelRotationValue, 0);

                    vehicle_model_transform.rotation = Quaternion.Lerp(tempQ, vehicle_model_transform.rotation, drift_correction_float);
                }
            }
            //Not Drifting
            else
            {
                if (isAudioDrift)
                {
                    isAudioDrift = false;
                    AudioManager.instance.Stop("Drift");
                }
                if (is_drift)
                {
                    drift_correction_float = 0;
                }

                if (tempDriftModelRotationValue < 0)
                {
                    tempDriftModelRotationValue += Time.fixedDeltaTime * 10;
                }
                else if (tempDriftModelRotationValue > 0)
                {
                    tempDriftModelRotationValue -= Time.fixedDeltaTime * 10;
                }
                tempDriftModelRotationValue = 0;

                //if(Mathf.Abs(target_steer_modified) < max_steer_float/10)
                //{
                //    Debug.Log("Drift is no OFF, when Target_Steer_Modified is: " + target_steer_modified);
                is_drift = false;
                //}

                //Debug.Log("Drift is OFF, when Target_Steer_Modified is: " + target_steer_modified);

                //When not drifting, the model will eventually align its rotation to the actual 'Heading' direction of the Vehicle.
                if (drift_correction_float < max_drift_correction_float)
                {
                    drift_correction_float += drift_correction_multiplier;
                }
                else
                {
                    drift_correction_float = max_drift_correction_float;
                }

                //WHILE NOT DRIFTING OVER, STEERING REGAINS NORMAL STEERING VALUE
                if (target_steer_modified > max_steer_float * DRIFT_STEER_DAMPEN)
                {
                    //target_steer_modified += Time.fixedDeltaTime;
                    target_steer_modified -= Time.fixedDeltaTime * 5;
                    Debug.Log("decreasing steering:     " + target_steer_modified);
                }
                else
                {
                    //target_steer_modified = max_steer_float * DRIFT_STEER_DAMPEN;
                    target_steer_modified = max_steer_float * DRIFT_STEER_DAMPEN;
                }


                //Rotate: Note there should be DECREMENTAL deviation between the Heading rotation V.S. Model rotation
                if (Quaternion.Angle(vehicle_model_transform.rotation, vehicle_heading_transform.rotation) > 0)
                {
                    vehicle_model_transform.rotation = Quaternion.Lerp(vehicle_model_transform.rotation, vehicle_heading_transform.rotation, drift_correction_float);
                }
                else
                    vehicle_model_transform.rotation = Quaternion.Lerp(vehicle_model_transform.rotation, vehicle_heading_transform.rotation, 1f);
            }


        }
        #endregion

        #region Slope Tilt Methods
        private RaycastHit left_rear_hit;
        private RaycastHit left_forward_hit;
        private RaycastHit right_rear_hit;
        private RaycastHit right_forward_hit;
        private Vector3 vehicleUpDirection;

        private Vector3 tempForwardVectorForSlopeMeasurement = Vector3.forward;
        private Vector3 tempRightVectorForSlopeMeasurement = Vector3.right;
        private Vector3 tempUpVectorForSlopeMeasurement = Vector3.up;

        private RaycastHit center_hit;

        private Vector3 VehicleGetSlope(Transform tr)
        {
            Physics.Raycast(tr.position - tempForwardVectorForSlopeMeasurement * length_float - (tempRightVectorForSlopeMeasurement * width_float) + tempUpVectorForSlopeMeasurement, -tempUpVectorForSlopeMeasurement * height_float, out left_rear_hit, slope_ray_dist_float, rayCast_layerMask);
            Physics.Raycast(tr.position - tempForwardVectorForSlopeMeasurement * length_float + (tempRightVectorForSlopeMeasurement * width_float) + tempUpVectorForSlopeMeasurement, -tempUpVectorForSlopeMeasurement * height_float, out right_rear_hit, slope_ray_dist_float, rayCast_layerMask);
            Physics.Raycast(tr.position + tempForwardVectorForSlopeMeasurement * length_float - (tempRightVectorForSlopeMeasurement * width_float) + tempUpVectorForSlopeMeasurement, -tempUpVectorForSlopeMeasurement * height_float, out left_forward_hit, slope_ray_dist_float, rayCast_layerMask);
            Physics.Raycast(tr.position + tempForwardVectorForSlopeMeasurement * length_float + (tempRightVectorForSlopeMeasurement * width_float) + tempUpVectorForSlopeMeasurement, -tempUpVectorForSlopeMeasurement * height_float, out right_forward_hit, slope_ray_dist_float, rayCast_layerMask);

            if (left_rear_hit.collider)
            {
                if (left_rear_hit.collider.GetComponent<Loop>())
                {
                    isOnLoop = true;
                    tempForwardVectorForSlopeMeasurement = vehicle_transform.forward;
                    tempRightVectorForSlopeMeasurement = vehicle_transform.right;
                    tempUpVectorForSlopeMeasurement = vehicle_transform.up;
                }
                else
                {
                    isOnLoop = false;
                    tempForwardVectorForSlopeMeasurement = Vector3.forward;
                    tempRightVectorForSlopeMeasurement = Vector3.right;
                    tempUpVectorForSlopeMeasurement = Vector3.up;
                }
            }
            if (right_rear_hit.collider)
            {
                if (right_rear_hit.collider.GetComponent<Loop>())
                {
                    isOnLoop = true;
                    tempForwardVectorForSlopeMeasurement = vehicle_transform.forward;
                    tempRightVectorForSlopeMeasurement = vehicle_transform.right;
                    tempUpVectorForSlopeMeasurement = vehicle_transform.up;
                }
                else
                {
                    isOnLoop = false;
                    tempForwardVectorForSlopeMeasurement = Vector3.forward;
                    tempRightVectorForSlopeMeasurement = Vector3.right;
                    tempUpVectorForSlopeMeasurement = Vector3.up;
                }
            }
            if (left_forward_hit.collider)
            {
                if (left_forward_hit.collider.GetComponent<Loop>())
                {
                    isOnLoop = true;
                    tempForwardVectorForSlopeMeasurement = vehicle_transform.forward;
                    tempRightVectorForSlopeMeasurement = vehicle_transform.right;
                    tempUpVectorForSlopeMeasurement = vehicle_transform.up;
                }
                else
                {
                    isOnLoop = false;
                    tempForwardVectorForSlopeMeasurement = Vector3.forward;
                    tempRightVectorForSlopeMeasurement = Vector3.right;
                    tempUpVectorForSlopeMeasurement = Vector3.up;
                }
            }
            if (right_forward_hit.collider)
            {
                if (right_forward_hit.collider.GetComponent<Loop>())
                {
                    isOnLoop = true;
                    tempForwardVectorForSlopeMeasurement = vehicle_transform.forward;
                    tempRightVectorForSlopeMeasurement = vehicle_transform.right;
                    tempUpVectorForSlopeMeasurement = vehicle_transform.up;
                }
                else
                {
                    isOnLoop = false;
                    tempForwardVectorForSlopeMeasurement = Vector3.forward;
                    tempRightVectorForSlopeMeasurement = Vector3.right;
                    tempUpVectorForSlopeMeasurement = Vector3.up;
                }
            }

            //if (left_rear_hit.collider.GetComponent<Loop>() || right_rear_hit.collider.GetComponent<Loop>() || left_forward_hit.collider.GetComponent<Loop>() || right_forward_hit.collider.GetComponent<Loop>())
            //{
            //    tempForwardVectorForSlopeMeasurement = vehicle_transform.forward;
            //    tempRightVectorForSlopeMeasurement = vehicle_transform.right;
            //    tempUpVectorForSlopeMeasurement = vehicle_transform.up;
            //}

            vehicleUpDirection = (Vector3.Cross(right_rear_hit.point - tempUpVectorForSlopeMeasurement, left_rear_hit.point - tempUpVectorForSlopeMeasurement) +
                                                    Vector3.Cross(left_rear_hit.point - tempUpVectorForSlopeMeasurement, left_forward_hit.point - tempUpVectorForSlopeMeasurement) +
                                                    Vector3.Cross(left_forward_hit.point - tempUpVectorForSlopeMeasurement, right_forward_hit.point - tempUpVectorForSlopeMeasurement) +
                                                    Vector3.Cross(right_forward_hit.point - tempUpVectorForSlopeMeasurement, right_rear_hit.point - tempUpVectorForSlopeMeasurement)
                                                    ).normalized;
            if (isCodeDebug)
            {
                Debug.DrawRay(tr.position - Vector3.forward * length_float - (Vector3.right * width_float) + Vector3.up, Vector3.down * 20, Color.green);
                Debug.DrawRay(tr.position - Vector3.forward * length_float + (Vector3.right * width_float) + Vector3.up, Vector3.down * 20, Color.green);
                Debug.DrawRay(tr.position + Vector3.forward * length_float - (Vector3.right * width_float) + Vector3.up, Vector3.down * 20, Color.green);
                Debug.DrawRay(tr.position + Vector3.forward * length_float + (Vector3.right * width_float) + Vector3.up, Vector3.down * 20, Color.green);
            }

            Physics.Raycast(tr.position, -tempUpVectorForSlopeMeasurement, out center_hit, slope_ray_dist_float, rayCast_layerMask);
            vehicle_air_height = Vector3.Distance(tr.position, center_hit.point) - height_float;


            return vehicleUpDirection;
        }

        Vector3 tempUpDirection;
        Quaternion tempVehicleRotation;
        Vector3 properUpDirection;
        [SerializeField] bool isOnLoop;

        private void VehicleTiltSlope()
        {
            tiltLerp_float = Mathf.Max(.1f, (1f - accel_magnitude_float / 60));
            properUpDirection = vehicle_transform.up;
            if (isOnLoop)
                vehicle_transform.up = VehicleGetSlope(vehicle_transform);// Vector3.Lerp(vehicle_transform.up, VehicleGetSlope(vehicle_transform), 1);
            else
                vehicle_transform.up = Vector3.Lerp(vehicle_transform.up, VehicleGetSlope(vehicle_transform), tiltLerp_float);
            //if(!(tempUpDirection.x < 0 && properUpDirection.x < 0) || (tempUpDirection.x > 0 && properUpDirection.x > 0))

            //tempTransform.up = tempUpDirection;
            //vehicle_transform.up = tempUpDirection;
            //if (tempTransform.rotation.y != 0)
            //{
            //transform.up = new Quaternion(tempTransform.rotation.x, 0, tempTransform.rotation.z, tempTransform.rotation.w);
            //vehicle_transform.rotation = tempVehicleRotation;
            //}
            //Debug.Log("Neg X" + vehicle_transform.rotation.y);
            //vehicle_transform.up = Vector3.Lerp(vehicle_transform.up, VehicleGetSlope(vehicle_transform), tiltLerp_float);
        }

        #endregion

        #region Spin Out Behavior

        public void SpinOutBehavior()
        {
            if (!hasVehicleControl)
            {
                vehicleSpinOutTransform.rotation *= Quaternion.Euler(Vector3.up * 500 * Time.deltaTime);
            }
            else
                vehicleSpinOutTransform.rotation = Quaternion.Slerp(vehicleSpinOutTransform.rotation, vehicle_model_transform.rotation, 1f);
        }

        public void StartSpinOut()
        {
            //if (spinOutPlayerID == pv.viewID)
            //{
            hasVehicleControl = false;
            if (SpinOutCoroutine == null)
            {
                SpinOutCoroutine = SpinOutCO();
                StartCoroutine(SpinOutCoroutine);
                hasVehicleControl = true;
            }
            //}
        }

        private IEnumerator SpinOutCoroutine;
        private IEnumerator SpinOutCO()
        {
            hasVehicleControl = false;
            yield return new WaitForSeconds(2);

            hasVehicleControl = true;
            SpinOutCoroutine = null;
        }

        //[SerializeField] private float LaunchMagnitude;
        //private Vector3 LaunchVector;
        //private IEnumerator LaunchVehicle()
        //{
        //    LaunchMagnitude = 100f;
        //    LaunchVector = -transform.up * LaunchMagnitude;
        //    yield return new WaitForSeconds(.1f);
        //    if(LaunchMagnitude > 0)
        //    {
        //        LaunchVector = +transform.up * LaunchMagnitude;
        //    }
        //    else
        //    {
        //
        //    }
        //
        //}



        #endregion


        #region wheels
        private void RotateWheels()
        {
            if (accel_magnitude_float > 0)
            {
                axel_fr_transform.localRotation = Quaternion.Euler(0, wheel_steer_float, 0);
                axel_fl_transform.localRotation = Quaternion.Euler(0, wheel_steer_float, 0);
                if (is_4wd)
                {
                    axel_rr_transform.localRotation = Quaternion.Euler(0, -wheel_steer_float, 0);
                    axel_rl_transform.localRotation = Quaternion.Euler(0, -wheel_steer_float, 0);
                }
            }
            else
            {
                axel_fr_transform.localRotation = Quaternion.Euler(0, -wheel_steer_float, 0);
                axel_fl_transform.localRotation = Quaternion.Euler(0, -wheel_steer_float, 0);
                if (is_4wd)
                {
                    axel_rr_transform.localRotation = Quaternion.Euler(0, wheel_steer_float, 0);
                    axel_rl_transform.localRotation = Quaternion.Euler(0, wheel_steer_float, 0);
                }

            }
        }
        #endregion

        #region Inputs

        private void VehicleNitrosBoostInput()
        {
            //CameraFOVBehavior();

            if (nitros_meter_float <= 0)
            {
                is_nitrosboost = false;
                nitros_speed_float = 0;
                nitros_meter_float = 0;
                if (vehicle_camera_transform.GetComponent<Camera>().fieldOfView > min_fov_float)
                {
                    vehicle_camera_transform.GetComponent<Camera>().fieldOfView -= Time.fixedDeltaTime * pan_toward_float;
                }
                return;
            }

            //if (Input.GetKey(KeyCode.RightControl) || Input.GetAxis("LeftTrigger") > 0)
            if (Input.GetKey(KeyCode.RightControl) || Input.GetButton(input_nitros))
            {
                if (!is_nitrosboost)
                {
                    is_nitrosboost = true;
                    nitros_speed_float = max_nitros_speed_float;
                    if (isPostProfile)
                    {
                        if (is_MotionBlur) vehicle_camera_postprocess_behavior.profile.motionBlur.enabled = true;
                    }
                }

                if (is_Cinematic_View)
                {
                    if (vehicle_camera_transform.GetComponent<Camera>().fieldOfView < max_fov_float)
                    {
                        vehicle_camera_transform.GetComponent<Camera>().fieldOfView += Time.fixedDeltaTime * pan_away_float;
                    }
                }

                nitros_meter_float = nitros_meter_float > 0 ? nitros_meter_float -= Time.fixedDeltaTime * nitros_depletion_rate : 0;

            }
            else
            {
                if (is_nitrosboost)
                {
                    if (isPostProfile)
                    {
                        if (is_MotionBlur) vehicle_camera_postprocess_behavior.profile.motionBlur.enabled = false;
                    }
                }

                if (is_Cinematic_View)
                {
                    if (vehicle_camera_transform.GetComponent<Camera>().fieldOfView > min_fov_float)
                    {
                        vehicle_camera_transform.GetComponent<Camera>().fieldOfView -= Time.fixedDeltaTime * pan_toward_float;
                    }
                }


                is_nitrosboost = false;
                nitros_speed_float = 0;
            }
        }

        private void VehicleAccelInput()
        {
            if (!hasVehicleControl) return;

            VehicleReverseInput();
            if (isReverse) return;



            //if (!is_drift)
            //{
            //
            //    if (is_nitrosboost)
            //    {
            //        max_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
            //    }
            //    else
            //    {
            //        if (max_accel_modified > max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
            //        {
            //            max_accel_modified -= DRAG * Time.fixedDeltaTime;
            //            if (max_accel_modified < max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
            //            {
            //                max_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
            //            }
            //        }
            //        else
            //        {
            //            max_accel_modified += DRAG * Time.fixedDeltaTime;
            //            if (max_accel_modified > max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
            //            {
            //                max_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
            //            }
            //        }
            //    }
            //}

            //if (Input.GetKey(KeyCode.W) || Input.GetButton("A"))
            if (Input.GetKey(KeyCode.W) || Input.GetAxis(input_accelerate) > 0)
            {

                if (accel_magnitude_float < (target_accel_modified - isCollisionHit * target_accel_modified))
                {
                    if (isCollisionHit == 1)
                    {
                        Debug.Log("isCollisionHit: " + (isCollisionHit * target_accel_modified) + "vs. " + target_accel_modified);
                    }
                    accel_magnitude_float += (ACCEL + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float > target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }
                else
                {
                    accel_magnitude_float -= (ACCEL + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float < target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }


                if (!is_drift || 1 == 1)
                {
                    if (target_accel_modified < 0) target_accel_modified = 0;

                    if (is_nitrosboost)
                    {
                        target_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
                    }
                    else
                    {
                        if (target_accel_modified > max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
                        {
                            target_accel_modified -= DRAG * Time.fixedDeltaTime;
                            if (target_accel_modified < max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
                            {
                                target_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
                            }
                        }
                        else
                        {
                            target_accel_modified += DRAG * Time.fixedDeltaTime;
                            if (target_accel_modified > max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
                            {
                                target_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
                            }
                        }
                    }
                }






                if (accel_magnitude_float < target_accel_modified)
                {
                    accel_magnitude_float += (ACCEL + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float > target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }
                else
                {
                    accel_magnitude_float -= (ACCEL + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float < target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }
            }
            else
            {
                if (is_nitrosboost)
                {
                    if (accel_magnitude_float > max_nitros_speed_float)
                    {
                        accel_magnitude_float = accel_magnitude_float - DRAG * Time.fixedDeltaTime > max_nitros_speed_float ? accel_magnitude_float -= DRAG * Time.fixedDeltaTime : max_nitros_speed_float;
                    }
                    else
                    {
                        accel_magnitude_float = accel_magnitude_float + nitros_speed_float * Time.fixedDeltaTime < max_nitros_speed_float ? accel_magnitude_float += nitros_speed_float * Time.fixedDeltaTime : nitros_speed_float;
                    }
                }
                else
                {
                    //accel_magnitude_float = accel_magnitude_float > 0 ? accel_magnitude_float -= DRAG * Time.fixedDeltaTime : 0;
                    if (accel_magnitude_float > 0)
                    {
                        accel_magnitude_float -= DRAG * Time.fixedDeltaTime;
                        if (accel_magnitude_float < 0)
                        {
                            accel_magnitude_float = 0;
                        }
                    }
                    else if (accel_magnitude_float < 0)
                    {
                        accel_magnitude_float += DRAG * Time.fixedDeltaTime;
                        if (accel_magnitude_float > 0)
                        {
                            accel_magnitude_float = 0;
                        }
                    }

                }
            }
        }

        private void VehicleReverseInput()
        {
            if (Input.GetKey(KeyCode.B) || Input.GetAxis(input_reverse) > 0)
            {
                isReverse = true;
                //max_accel_modified -= DRAG * Time.fixedDeltaTime;
                //if (max_accel_modified < max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float)
                //{
                //    max_accel_modified = max_accel_float - Mathf.Abs(steer_magnitude_float) * STEER_DECELERATION + nitros_speed_float;
                //}

                if (target_accel_modified > 0) target_accel_modified = 0;

                if (!is_drift)
                {
                    target_accel_modified -= REVERSE * Time.fixedDeltaTime;
                    if (target_accel_modified < min_accel_float + nitros_speed_float)
                    {
                        target_accel_modified = min_accel_float + nitros_speed_float;
                    }
                }
                else
                {
                    Debug.Log("Drift and Reverse");
                    if (target_accel_modified > 0)
                    {
                        target_accel_modified -= 10 * Time.fixedDeltaTime;
                        if (target_accel_modified < 0)
                        {
                            target_accel_modified = 0;
                        }
                    }
                    else if (target_accel_modified < 0)
                    {
                        target_accel_modified += 10 * Time.fixedDeltaTime;
                        if (target_accel_modified > 0)
                        {
                            target_accel_modified = 0;
                        }
                    }

                }

                if (accel_magnitude_float < target_accel_modified)
                {
                    accel_magnitude_float += (REVERSE + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float > target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }
                else
                {
                    accel_magnitude_float -= (REVERSE + nitros_speed_float) * Time.fixedDeltaTime;
                    if (accel_magnitude_float < target_accel_modified)
                    {
                        accel_magnitude_float = target_accel_modified;
                    }
                }
            }
            else
            {
                isReverse = false;
            }
        }


        float tempDriftModelRotationValue;
        bool tempTurnRight;
        bool tempBeginSteerRight;
        private void VehicleSteerInput()
        {
            if (!hasVehicleControl) return;

            if (!is_drift)
            {
                if (target_steer_modified < max_steer_float)
                {
                    target_steer_modified += Time.deltaTime * 5;
                    if (target_steer_modified > max_steer_float)
                        target_steer_modified = max_steer_float;
                }
                //target_steer_modified = max_steer_float;
            }




            if (Input.GetKey(KeyCode.A) || Input.GetAxis(input_steering) < 0)
            {
                //Debug.Log("Player" + PlayerNum + ", Pressed: " + input_steering.ToString());

                if (accel_magnitude_float >= 0)
                {
                    if (wheel_steer_float > 0) wheel_steer_float = 0;
                    wheel_steer_float = wheel_steer_float > -max_steer_float ? wheel_steer_float -= STEER * Time.fixedDeltaTime : -max_steer_float;

                    if (steer_magnitude_float > 0) steer_magnitude_float = 0;
                    steer_magnitude_float = steer_magnitude_float > -target_steer_modified ? steer_magnitude_float -= STEER * Time.fixedDeltaTime : -target_steer_modified;
                }
                else
                {
                    if (wheel_steer_float < 0) wheel_steer_float = 0;
                    wheel_steer_float = wheel_steer_float < max_steer_float ? wheel_steer_float += STEER * Time.fixedDeltaTime : max_steer_float;

                    if (steer_magnitude_float < 0) steer_magnitude_float = 0;
                    steer_magnitude_float = steer_magnitude_float < target_steer_modified ? steer_magnitude_float += STEER * Time.fixedDeltaTime : target_steer_modified;
                }

                tempTurnRight = false;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetAxis(input_steering) > 0)
            {
                //Debug.Log("Player" + PlayerNum + ", Pressed: " + input_steering.ToString());

                if (accel_magnitude_float >= 0)
                {
                    if (wheel_steer_float < 0) wheel_steer_float = 0;
                    wheel_steer_float = wheel_steer_float < max_steer_float ? wheel_steer_float += STEER * Time.fixedDeltaTime : max_steer_float;

                    if (steer_magnitude_float < 0) steer_magnitude_float = 0;
                    steer_magnitude_float = steer_magnitude_float < target_steer_modified ? steer_magnitude_float += STEER * Time.fixedDeltaTime : target_steer_modified;
                }
                else
                {
                    if (wheel_steer_float > 0) wheel_steer_float = 0;
                    wheel_steer_float = wheel_steer_float > -max_steer_float ? wheel_steer_float -= STEER * Time.fixedDeltaTime : -max_steer_float;

                    if (steer_magnitude_float > 0) steer_magnitude_float = 0;
                    steer_magnitude_float = steer_magnitude_float > -target_steer_modified ? steer_magnitude_float -= STEER * Time.fixedDeltaTime : -target_steer_modified;
                }

                tempTurnRight = true;
            }
            else
            {
                if (Quaternion.Angle(vehicle_model_transform.rotation, vehicle_heading_transform.rotation) > 0)
                {
                    vehicle_model_transform.rotation = Quaternion.Lerp(vehicle_model_transform.rotation, vehicle_heading_transform.rotation, drift_correction_float);
                }
                else
                    vehicle_model_transform.rotation = Quaternion.Lerp(vehicle_model_transform.rotation, vehicle_heading_transform.rotation, 1f);


                //Return Vehicle Rotation to initial state
                if (steer_magnitude_float > 0)
                {
                    if (steer_magnitude_float - STEER * Time.fixedDeltaTime < 0)
                    {
                        steer_magnitude_float = 0;
                    }
                    else
                    {
                        steer_magnitude_float -= STEER * Time.fixedDeltaTime;
                    }
                }
                else if (steer_magnitude_float < 0)
                {
                    if (steer_magnitude_float + STEER * Time.fixedDeltaTime > 0)
                    {
                        steer_magnitude_float = 0;
                    }
                    else
                    {
                        steer_magnitude_float += STEER * Time.fixedDeltaTime;
                    }
                }

                //Return Wheel Rotation to initial state
                if (wheel_steer_float > 0)
                {
                    if (wheel_steer_float - STEER * Time.fixedDeltaTime < 0)
                    {
                        wheel_steer_float = 0;
                    }
                    else
                    {
                        wheel_steer_float -= STEER * Time.fixedDeltaTime;
                    }
                }
                else if (wheel_steer_float < 0)
                {
                    if (wheel_steer_float + STEER * Time.fixedDeltaTime > 0)
                    {
                        wheel_steer_float = 0;
                    }
                    else
                    {
                        wheel_steer_float += STEER * Time.fixedDeltaTime;
                    }
                }

            }
        }
        #endregion

        #region Applying Default Preset Values for custom vehicles
        [ContextMenu("Default Vehicle Values")]
        public void DefaultVehicleValuesPreset()
        {
            width_float = 4f;
            length_float = 6f;
            height_float = 2f;
            is_4wd = true;


            is_grounded = true;
            gravity_float = 0;
            target_accel_modified = 0;
            wheel_steer_float = 0;
            target_steer_modified = 0;
            accel_magnitude_float = 0;
            steer_magnitude_float = 0;
            brake_magnitude_float = 0;
            drift_correction_float = 0;
            nitros_meter_float = 100;
            nitros_speed_float = 0;


            is_Cinematic_View = true;
            min_fov_float = 90;
            max_fov_float = 130;
            pan_away_float = 10;
            pan_toward_float = 30;
            is_MotionBlur = true;

            //nitros_meter_float = 99999;

            max_gravity_float = 250f;
            max_steer_float = 15f;
            max_accel_float = 125f;
            min_accel_float = -69f;
            max_brake_float = 100f;
            min_drift_correction_float = 0.05f;
            max_drift_correction_float = 1f;
            drift_correction_multiplier = 0.05f;
            drift_accel_multiplier_float = 0.05f;
            drift_turn_ratio_float = 0.25f;
            max_nitros_meter_float = 100f;
            max_nitros_speed_float = 100f;
            nitros_depletion_rate = 10f;
            min_air_control = 0.1f;

            rayCast_layerMask = ~(1 << 1 | 1 << 2 | 1 << 8);
            slope_ray_dist_float = 1000;

            ACCEL = 50f;
            REVERSE = 25;
            DRAG = 25f;
            ROTATIONAL_DRAG = 25f;
            GRAVITY = 1000f;
            STEER = 15f;
            STEER_DECELERATION = 5f;
            DRIFT_ACCELERATION = 2.5f;
            DRIFT_STEER_DAMPEN = 1f;

            Start();
        }
        #endregion

    }








}
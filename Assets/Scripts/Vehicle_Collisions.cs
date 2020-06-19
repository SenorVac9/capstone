using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public class Vehicle_Collisions : MonoBehaviour
{
    public GameObject[] carParts;
    public Queue<GameObject> lostParts = new Queue<GameObject>();
    GameObject lostPart;
    VehicleBehavior vehicleBehavior;
    ui_controller ui_Controller;
    Player_Wheel_Detach Wheel_Detach;
    public float damageForce = 5000;
    float collisionDamage = 5;
    public Rigidbody dF;
    float temp_accel = 0;
    public AudioSource crashSource;
    ForceField forceField;
    public AudioClip crash;
    private bool isLimitCollision = false;
    bool collisionsDisabled = true;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = FindObjectOfType<VehicleBehavior>();
        ui_Controller = FindObjectOfType<ui_controller>();
        Wheel_Detach = gameObject.GetComponentInChildren<Player_Wheel_Detach>();
        forceField = gameObject.GetComponentInChildren<ForceField>();
        //dF = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Car Damage and parts loss Upon collision.
    private void OnCollisionEnter(Collision c)
    {

        if (c.gameObject.CompareTag("TrackColliders"))
        {
            //if (!isLimitCollision)
            //{
            //  isLimitCollision = true;
            temp_accel = 1 + (vehicleBehavior.accel_magnitude_float / 100);
            vehicleBehavior.accel_magnitude_float = 0;
            //vehicleBehavior.hasVehicleControl = false;
            // Calculate Angle Between the collision point and the player
            Vector3 dir = c.contacts[0].point - transform.position;
            //Vector3 dir = gameObject.transform.position + new Vector3(0,0,-100);
            Debug.Log("Contact Point" + dir);
            // We then get the opposite (-Vector3) and normalize it
            dir.Normalize();
            // And finally we add force in the direction of dir and multiply it by force. 
            //This will push back the player
            //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, dir, 0.1f);
            dF.AddForce(dir.x, dir.y, dir.z * (damageForce + temp_accel), (ForceMode.Acceleration));
            crashSource.PlayOneShot(crash);
            vehicleBehavior.accel_magnitude_float = -((temp_accel) * 25);
            //StartCoroutine(LimitCollision());
            //}
        }
        if (c.transform != transform)
        {
            if (c.gameObject.CompareTag("Obstacle"))
            {
                
                if (!isLimitCollision && !collisionsDisabled)
                {
                    isLimitCollision = true;
                    vehicleBehavior.hasVehicleControl = false;
                    vehicleBehavior.accel_magnitude_float = 0;
                    vehicleBehavior.SpinOutBehavior();
                    int randLostPartindex = Random.Range(0, carParts.Length - 1);
                    lostPart = carParts[randLostPartindex];//carParts[Random.Range(0, carParts.Count - 1)];

                    if (carParts != null)
                    {

                        carParts[randLostPartindex].SetActive(false);
                        DamageFromCollisions();
                    }

                    Debug.Log(lostPart);
                    //lostPart.active = false;
                    if (ui_Controller.has_Shield)
                    {
                        ui_Controller.has_Shield = false;
                        forceField.Activate();
                        return;
                    }
                    ui_Controller.ui_item[randLostPartindex].SetActive(false);
                    switch (randLostPartindex)
                    {
                        case 0:
                            ui_Controller.has_tire_1 = false;
                           Wheel_Detach.reservePartsList.Add(4);
                            //ui_Controller.ui_item[0].SetActive(false);
                            break;
                        case 2:
                            ui_Controller.has_tire_2 = false;
                            Wheel_Detach.reservePartsList.Add(3);
                            //ui_Controller.ui_item[1].SetActive(false);
                            break;
                        case 5:
                            ui_Controller.has_tire_3 = false;
                            Wheel_Detach.reservePartsList.Add(2);
                            //ui_Controller.ui_item[5].SetActive(false);
                            break;
                        case 7:
                            ui_Controller.has_tire_4 = false;
                            Wheel_Detach.reservePartsList.Add(1);
                            //ui_Controller.ui_item[7].SetActive(false);
                            break;
                        case 6:
                            ui_Controller.has_Milk = false;
                            //ui_Controller.ui_item[6].SetActive(false);
                            break;
                        case 3:
                            ui_Controller.has_door_1 = false;
                            //ui_Controller.ui_item[3].SetActive(false);
                            break;
                        case 4:
                            ui_Controller.has_door_2 = false;
                            //ui_Controller.ui_item[4].SetActive(false);
                            break;
                        default:
                            Debug.Log("nothing happened");
                            break;
                    }
                    DamageFromCollisions();
                    // Stores all the lost item in queue.
                    if (lostParts != null)
                    {
                        lostParts.Enqueue(lostPart);
                        //Debug.Log("see this" + lostParts.Peek());
                        //DamageFromCollisions();
                    }

                    else
                    {
                        Debug.Log("No qu found");
                        lostParts.Enqueue(lostPart);
                        Debug.Log("see this" + lostParts.Peek());
                    }
                    Destroy(c.gameObject);
                    StartCoroutine(LimitCollision());

                }
            }
        }

        //Debug.Log()
    }

    //Decrease performance of vehicle by a certain amount.
    void DamageFromCollisions()
    {
        vehicleBehavior.max_accel_float -= collisionDamage;
    }

    IEnumerator LimitCollision()
    {
        yield return new WaitForSeconds(1);
        isLimitCollision = false;
        vehicleBehavior.hasVehicleControl = true;

    }
     public void CollisionDisabler()
    {
        StartCoroutine(PreventPartCollision());
    }
    IEnumerator PreventPartCollision()
    {
        yield return new WaitForSeconds(2);
        collisionsDisabled = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        //isLimitCollision = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        isLimitCollision = true;
        StartCoroutine(LimitCollision());
    }
    /*void AddDamageForce()
    {
        
        
    }*/


}
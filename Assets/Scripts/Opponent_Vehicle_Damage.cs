using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModuloKart.CustomVehiclePhysics;

public class Opponent_Vehicle_Damage : MonoBehaviour
{
    public List<GameObject> carParts = new List<GameObject>(); // stores all the car parts that can be detached.
    public Queue<GameObject> lostParts = new Queue<GameObject>();
    GameObject lostPart;
    VehicleBehavior vehicleBehavior;
    public float damageForce = 50000;
    float collisionDamage = 5;
    public Rigidbody dF;
    private bool isLimitCollision = false;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = FindObjectOfType<VehicleBehavior>();
        //dF = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Car Damage and parts loss Upon collision.
    private void OnCollisionEnter(Collision c)
    {
        if (c.transform != transform)
        {
            if (c.gameObject.CompareTag("Obstacle"))
            {

                if (!isLimitCollision)
                {
                    isLimitCollision = true;

                    //Debug.Log("Collided With" + c.gameObject.name);

                    // Calculate Angle Between the collision point and the player
                    Vector3 dir = c.contacts[0].point - transform.position;
                    // We then get the opposite (-Vector3) and normalize it
                    //dir.Normalize();
                    // And finally we add force in the direction of dir and multiply it by force. 
                    // This will push back the player
                    //GetComponent<Rigidbody>().AddForce(dir * damageForce);

                    //DamageFromCollisions();
                    //AddDamageForce();
        
                    dF.AddForceAtPosition(dir.normalized, transform.position, ForceMode.Impulse);
                    //dF.GetComponent<Rigidbody>().AddForce(dF.transform.right * damageForce, ForceMode.Impulse);
                    //Debug.Log("see this" + " Force is applied");
                    //Randomly Detaches any one caar part when collided with an obstacles
                    int randLostPartindex = Random.Range(0, carParts.Count - 1);
                    lostPart = carParts[randLostPartindex];//carParts[Random.Range(0, carParts.Count - 1)];
                    if (carParts != null)
                    {
                        
                        carParts.Remove(carParts[randLostPartindex]);
                        //DamageFromCollisions();
                    }

                    Debug.Log(lostPart);
                    lostPart.active = false;
                    // Stores all the lost item in queue.
                    if (lostParts != null)
                    {
                        lostParts.Enqueue(lostPart);
                        //Debug.Log("see this" + lostParts.Peek());
                        DamageFromCollisions();
                    }

                    else
                    {
                        Debug.Log("No qu found");
                        lostParts.Enqueue(lostPart);
                        Debug.Log("see this" + lostParts.Peek());
                    }

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
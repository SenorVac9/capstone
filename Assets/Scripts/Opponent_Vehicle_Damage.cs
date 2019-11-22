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
    float collisionDamage = 15;
    public Rigidbody dF;
    // Start is called before the first frame update
    void Start()
    {
        vehicleBehavior = FindObjectOfType<VehicleBehavior>();
        dF = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Car Damage and parts loss Upon collision.
    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided With" + c.gameObject.name);
           
            // Calculate Angle Between the collision point and the player
            Vector3 dir = c.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir.Normalize();
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            GetComponent<Rigidbody>().AddForce(dir * damageForce);

            DamageFromCollisions();
            //AddDamageForce();
            //c.gameObject.GetComponent<Rigidbody>().AddForce(c.gameObject.transform.forward * damageForce, ForceMode.Impulse);
            //Randomly Detaches any one caar part when collided with an obstacles
            int randLostPartindex = Random.Range(0, carParts.Count - 1);
            lostPart = carParts[randLostPartindex];//carParts[Random.Range(0, carParts.Count - 1)];


            Debug.Log(lostPart);
            lostPart.active = false;
            // Stores all the lost item in queue.
            if (lostParts != null)
            {
                lostParts.Enqueue(lostPart);
                Debug.Log("see this" + lostParts.Peek());
            }

            else
            {
                Debug.Log("No qu found");
                lostParts.Enqueue(lostPart);
                Debug.Log("see this" + lostParts.Peek());
            }
        }

        //Debug.Log()
    }

    //Decrease performance of vehicle by a certain amount.
    void DamageFromCollisions ()
    {
       vehicleBehavior.max_accel_float -= collisionDamage;
    }
    
    /*
    void AddDamageForce()
    {
        //dF.AddForce(0, 0, damageForce, ForceMode.Impulse);
        
    }
    */

}

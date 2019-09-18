using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//AI script that moves towards a transform specified by path gameobject(which is an array) and then 
//updates the transform it will travel towards when it reaches its destination.
public class Node_Follower_AI : MonoBehaviour
{
    [Tooltip("integer representing the Node in the path that the ai is travelling towards.")]
    public int current_node;
    [Tooltip("the transform of the current Node in the path that the ai is travellign towards.")]
    public Transform target;
    [Tooltip("float value representing the top speed of the ai")]
    public float maxvelocity;
    [Tooltip("float value representing the acceleration of the ai")]
    public float acceleration;
    Rigidbody rb;
    [Tooltip("The path of nodes that the ai will follow")]
    public AI_Path path;
    public int current_lap;
    public int current_leg;
    public int max_legs;
    public float distance_to_next_node;

   
    void Start()
    {
        //gets reference to the rigidbody attached to this game object.
        rb = GetComponent<Rigidbody>();
        //Sets the initial target point
        target = path.waypoints[current_node];
        foreach(Transform t in path.waypoints)
        {
            max_legs++;
        }
        distance_to_next_node = Vector3.Distance(target.position, transform.position);
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Waypoint")
        {
            //handles resetting the node counter once the end of the nodes have been reached.
            if (current_node+1 >= path.waypoints.Count)
            {
                current_leg = 0;
                current_node = 0;
                current_lap++;
                target = path.waypoints[current_node];
                
            }
            //handles incrementing node counter and setting the new target node.
            else if (current_node < path.waypoints.Count + 1)
            
            {
                current_node++;
                current_leg++;
                target = path.waypoints[current_node];
                
            }
           
           
        }
    }

    
    void FixedUpdate()
    {
        distance_to_next_node = Vector3.Distance(target.position, transform.position);

        //This forces the ai to point towards its target node.
        transform.LookAt(target.transform);

        {

            //only accelerates if max velocity has not been reached.
            if (rb.velocity.magnitude < maxvelocity)
            {
                rb.AddForce(transform.forward * acceleration);
            }
          
        }
    }
}

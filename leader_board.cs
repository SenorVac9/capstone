using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class leader_board : MonoBehaviour
{
    public GameObject Racer_1;
    public GameObject Racer_2;
    public GameObject Racer_3;
    public GameObject Lead_board;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Lead_board.GetComponent<Text>().text = "Yellow Racer Lap " + Racer_1.GetComponent<Node_Follower_AI>().current_lap + " Leg " + Racer_1.GetComponent<Node_Follower_AI>().current_leg + " distance to next node " + Racer_1.GetComponent<Node_Follower_AI>().distance_to_next_node 
            + "\n" + "Red Racer Lap" + Racer_2.GetComponent<Node_Follower_AI>().current_lap + " Leg " + Racer_2.GetComponent<Node_Follower_AI>().current_leg + " Distance  to next node " + Racer_2.GetComponent<Node_Follower_AI>().distance_to_next_node + "\n"
             +"Blue Racer Lap " + Racer_3.GetComponent<Node_Follower_AI>().current_lap + " Leg " + Racer_3.GetComponent<Node_Follower_AI>().current_leg +"Distance to next node " + Racer_3.GetComponent<Node_Follower_AI>().distance_to_next_node + "\n";


    }
}

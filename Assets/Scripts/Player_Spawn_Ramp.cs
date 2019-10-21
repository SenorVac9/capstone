using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawn_Ramp : MonoBehaviour
{
    public GameObject ramp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Track")
            Instantiate(ramp, transform.position, Quaternion.Euler(50f, -0f, -90f));
    }
}

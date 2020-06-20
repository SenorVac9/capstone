using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_pickup : MonoBehaviour
{
    public GameObject particle_pickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="GameController")
        {
            Instantiate(particle_pickup, transform.position,transform.rotation);
            
        }
    }
}

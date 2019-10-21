using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Hood: MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag=="Hood_projectile")
        {
            Destroy(c.gameObject);

            //Instantiate(ramp, transform.position, Quaternion.Euler(50f, -90f, -90f));
        }
    }
}

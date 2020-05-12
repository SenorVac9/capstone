using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkSpill : MonoBehaviour
{
    private void Update()
    {
        if(transform.localScale.x < 30.5f)
        transform.localScale += new Vector3(0.1f, 0.0f, 0.1f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            //Call spin out function
            Debug.Log("Hit Milk");
            Destroy(gameObject);
        }
    }

   
}

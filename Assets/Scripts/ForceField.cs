using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("TestField"))
        {
            rend.enabled = true;
            Destroy(gameObject, 1.5f);
        }
        
    }
    
        
        
    
}

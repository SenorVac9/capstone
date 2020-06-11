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

  
    public void Activate()
    {
        rend.enabled = true;
        Destroy(gameObject, 1.5f);
    }
        
        
    
}

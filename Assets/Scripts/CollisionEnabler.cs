using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnabler : MonoBehaviour
{
    public Collider collider;
    public Collider trigger;
    // Start is called before the first frame update
    void Start()
    {
        collider.enabled = false;
        trigger.enabled = false;
        StartCoroutine(ColliderEnableTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ColliderEnableTimer()
    {
        yield return new WaitForSeconds(2);
        collider.enabled = true;
        trigger.enabled = true;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    GameObject PickUp;
    public float Timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        PickUp = gameObject.GetComponentInChildren<PartReplenishScript>().gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Timer > 0)
        {
            if(Time.time > Timer)
            {
                PickUp.SetActive(true);
                Timer = 0;
                int r = Random.Range(0, 3);
                switch (r)
                {
                    case 0:
                        PickUp.GetComponent<PartReplenishScript>().setPickUpType(PartReplenishScript.PickUpType.Character);
                        break;
                    case 1:
                        PickUp.GetComponent<PartReplenishScript>().setPickUpType(PartReplenishScript.PickUpType.Nitro);
                        break;
                    case 2:
                        PickUp.GetComponent<PartReplenishScript>().setPickUpType(PartReplenishScript.PickUpType.Tires);
                        break;
                }
            }
        }
    }
}

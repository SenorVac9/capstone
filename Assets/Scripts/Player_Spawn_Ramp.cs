using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawn_Ramp : MonoBehaviour
{
    public GameObject ramp;
    Player_Projectile projectile;
   
    int cnt = 0;
    //bool enter = false;
    // Start is called before the first frame update
    void Start()
    {
        projectile = GameObject.FindObjectOfType<Player_Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Ramp_timer()
    {
        //enter = true;
        Debug.Log("Your enter Coroutine at" + Time.time);
        yield return new WaitForSeconds(100.0f);
        //enter = false;
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Track")
        {
            if (cnt == 0)
            {
                Instantiate(ramp, transform.position, projectile.spawnpoint.transform.rotation) ;
               // DestroyImmediate(projectile.prefab1);
                projectile.prefab1.gameObject.SetActive(false);
                Debug.Log("spawning");
                cnt++;
              //  StartCoroutine(Ramp_timer());
                //  ramp.gameObject.SetActive(false);


            }
        }
        if (collision.gameObject.tag == "Road_Bridge")
        {
            if (cnt == 0)
            {
                ramp.transform.Rotate(0,-120, 0);
                Instantiate(ramp, transform.position, projectile.spawnpoint.transform.rotation);
                // DestroyImmediate(projectile.prefab1);
                projectile.prefab1.gameObject.SetActive(false);
                Debug.Log("spawning");
                cnt++;
               // StartCoroutine(Ramp_timer());
               // ramp.gameObject.SetActive(false);
                

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Spawn_Ramp : MonoBehaviour
{
    public GameObject ramp;
    Player_Projectile projectile;
    int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        projectile = GameObject.FindObjectOfType<Player_Projectile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Track")
        {
            if (cnt == 0)
            {
                Instantiate(ramp, transform.position, projectile.spawnpoint.transform.rotation);
               // DestroyImmediate(projectile.prefab1);
                projectile.prefab1.SetActive(false);
                Debug.Log("spawning");
                cnt++;
            }
        }
    }
}

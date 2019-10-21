using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {

    public GameObject prefab;
    public Transform SpawnPoint;
	// Use this for initialization
	void Start () {
        Instantiate(prefab, SpawnPoint.transform.position, Quaternion.identity);   
	}
	
	// Update is called once per frame
	void Update () {
		
	}
            
}

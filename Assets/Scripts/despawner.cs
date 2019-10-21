using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawner : MonoBehaviour {
    private GameObject play1;
    private GameObject play2;
    private GameObject play3;
    private GameObject play4;
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public Camera camera4;
	// Use this for initialization
	void Start () {

        play1 = GameObject.Find("Sphere");
        play2 = GameObject.Find("Sphere (1)");
        play3 = GameObject.Find("Sphere (2)");
        play4 = GameObject.Find("Sphere (3)");
        //find the player objects
        camera1 = play1.GetComponentInChildren<Camera>();
        camera2 = play2.GetComponentInChildren<Camera>();
        camera3 = play3.GetComponentInChildren<Camera>();
        camera4 = play4.GetComponentInChildren<Camera>();
        //and their attached cameras

        if (ScreenChange.players == 1)
        {
            Destroy(play2);
            Destroy(play3);
            Destroy(play4);
            camera1.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
            //destroy all but player 1; P1 gets fullscreen
            
        }
        if (ScreenChange.players == 2)
        {
            Destroy(play3);
            Destroy(play4);
            camera1.GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
            camera2.GetComponent<Camera>().rect = new Rect(0, 0, 1, 0.5f);
            //Destroy players 3 and 4; P1 and P2 are horizontal splitscreen
        }
        if (ScreenChange.players == 3)
        {
            Destroy(play4);
            camera1.GetComponent<Camera>().rect = new Rect(0, 0.5f, 1, 0.5f);
            camera2.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
            camera3.GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            //Destory player 4; P1 has the top half, P2 and P3 share the bottom half
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class ScreenChange : MonoBehaviour {
    public bool p1;
    public bool p2;
    public bool p3;
    public bool p4;
    public bool quit;
    public static int players = 1;
	// Use this for initialization
	void Start () {
		
	}
	//how the screen changes
	// Update is called once per frame
	void Update () {
        if(Input.GetKey("escape"))
            {
            SceneManager.LoadScene(0);
        }
	}

    private void OnMouseUp()
    {
        if(p1)
        {
            players = 1;
            Debug.Log("one player");
            SceneManager.LoadScene(1);
        }
        if(p2)
        {
            players = 2;
            Debug.Log("two player");
            SceneManager.LoadScene(1);
        }
        if (p3)
        {
            players = 3;
            Debug.Log("three player");
            SceneManager.LoadScene(1);
        }
        if (p4)
        {
            players = 4;
            Debug.Log("four player");
            SceneManager.LoadScene(1);
        }
        if(quit)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}

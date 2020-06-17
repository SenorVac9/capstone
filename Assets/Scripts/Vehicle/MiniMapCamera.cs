using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{

    public GameObject localPlayer;
    public Camera MinimapCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MinimapCam.transform.position = (new Vector3(localPlayer.transform.position.x, MinimapCam.transform.position.y, localPlayer.transform.position.z));
    }
}

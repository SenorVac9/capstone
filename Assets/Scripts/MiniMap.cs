using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private LineRenderer lineRender;
    private GameObject MiniMapPath;
    int num_of_path;
    public GameObject localPlayer; 
    public GameObject MiniMapCam;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        MiniMapPath = this.gameObject;
        num_of_path = MiniMapPath.transform.childCount;
        
        lineRender.positionCount = num_of_path + 1;

        for(int i = 0; i < num_of_path; i++)
        {
            lineRender.SetPosition(i, new Vector3(MiniMapPath.transform.GetChild(i).transform.position.x, 4, MiniMapPath.transform.GetChild(i).transform.position.z));
        }

        lineRender.SetPosition(num_of_path, lineRender.GetPosition(0));
        lineRender.startWidth = 1000f;
        lineRender.endWidth = 1000f;

    }

    // Update is called once per frame
    void Update()
    {
        //MiniMapCam.transform.position = (new Vector3(localPlayer.transform.position.x, MiniMapCam.transform.position.y, localPlayer.transform.position.z));
        
    }
}

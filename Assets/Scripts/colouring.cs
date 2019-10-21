using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colouring : MonoBehaviour {

    public Color color = Color.black;

    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(color.r, color.g, color.b, color.a);
    }
    //colours the balls
    void Update()
    {
        if (gameObject.name == "Sphere")
            color.g = 255f;
            color.a = 255f;
        if (gameObject.name == "Sphere(1)")
            color.r = 255f;
            color.a = 255f;
        if (gameObject.name == "Sphere(2)")
            color.b = 255f;
            color.a = 255f;
        if (gameObject.name == "Sphere(3)")
        {
            color.a = 255f;
            color.g = 255f;
            color.b = 255f;
            color.r = 255f;
        }
    }
}

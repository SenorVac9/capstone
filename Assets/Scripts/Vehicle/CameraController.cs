using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public void lookattarget()
    //{
    //    vector3 _lookdirection = objecttofollow.position - transform.position;
    //    quaternion _rot = quaternion.lookrotation(_lookdirection, vector3.up);
    //    transform.rotation = quaternion.lerp(transform.rotation, _rot, lookspeed * time.deltatime);
    //}

    //public void movetotarget()
    //{
    //    vector3 _targetpos = objecttofollow.position +
    //                         objecttofollow.forward * offset.z +
    //                         objecttofollow.right * offset.x +
    //                         objecttofollow.up * offset.y;
    //    transform.position = vector3.lerp(transform.position, _targetpos, followspeed * time.deltatime);
    //}

    //void fixedupdate()
    //{
    //    lookattarget();
    //    movetotarget();
    //}

    //public transform objecttofollow;
    //public vector3 offset;
    //public float followspeed = 10;
    //public float lookspeed = 5;


    
    public Transform cameraTarget;
    public float _speed = 20.0f;
    public Vector3 dist;
    public Transform lookTarget;

    private void Start()
    {
    }
    void FixedUpdate()
    {
        Vector3 dPos = cameraTarget.position + dist;
        Vector3 sPos = Vector3.Lerp(transform.position, dPos, _speed * Time.deltaTime);
        transform.position = sPos;
        transform.LookAt(lookTarget.position,cameraTarget.up);
    }

}
//public Transform vehicle;
//public float distance = 6.4f;
//public float height = 1.4f;
//public float rotationDamping = 3.0f;
//public float heightDamping = 2.0f;
//public float zoomRatio = 0.5f;
//public float defaultFOV = 60f;
//private Vector3 rotationVector;

//private void LateUpdate()
//{
//    if (vehicle != null)
//    {
//        float wantedAngle = rotationVector.y;
//        float wantedHeight = vehicle.position.y + height;
//        float myAngle = transform.eulerAngles.y;
//        float myHeight = transform.position.y;

//        myAngle = Mathf.LerpAngle(myAngle, wantedAngle, rotationDamping * Time.deltaTime);
//        myHeight = Mathf.Lerp(myHeight, wantedHeight, heightDamping * Time.deltaTime);

//        Quaternion currentRotation = Quaternion.Euler(0, myAngle, 0);
//        transform.position = vehicle.position;
//        transform.position -= currentRotation * Vector3.forward * distance;
//        Vector3 temp = transform.position;
//        temp.y = myHeight;
//        transform.position = temp;
//        transform.LookAt(vehicle);
//    }
//}

//private void FixedUpdate()
//{
//    if (vehicle != null)
//    {
//        Vector3 temp = vehicle.InverseTransformDirection(vehicle.GetComponent<Rigidbody>().velocity);
//        temp.y = vehicle.eulerAngles.y + 180;
//        rotationVector = temp;
//    }
//    else
//    {
//        Vector3 temp = rotationVector;
//        temp.y = vehicle.eulerAngles.y;
//        rotationVector = temp;
//    }
//    float acc = vehicle.GetComponent<Rigidbody>().velocity.magnitude;
//    GetComponent<Camera>().fieldOfView = defaultFOV + acc * zoomRatio * Time.deltaTime;
//}





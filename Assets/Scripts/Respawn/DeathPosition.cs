using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPosition : MonoBehaviour
{
    public Transform startPos;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            other.gameObject.transform.position = GameManager.Instance.lastCheckpoint.position;
        }
    }
}

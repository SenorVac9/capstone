using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("GameController"))
        {
            GameManager.Instance.lastCheckpoint = transform;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    
    public Transform PlayerRespawnPoint;
    public GameObject PlayerPrefab;

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Boundaries")
        {
            Instantiate(PlayerRespawnPoint, PlayerRespawnPoint.position, PlayerRespawnPoint.rotation);
        }
        else if (col.gameObject.tag == "Death")
        {
            Instantiate(PlayerPrefab, PlayerRespawnPoint.position, PlayerRespawnPoint.rotation);
        }
    }

}

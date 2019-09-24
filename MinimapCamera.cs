using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float yOffset;

    private void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position;
        targetPosition.y += yOffset;
        transform.position = targetPosition;
        
    }
}

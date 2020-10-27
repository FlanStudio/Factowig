using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = Camera.main.transform.position;        
    }

    private void Update()
    {
        targetPosition.x = transform.position.x;
        transform.rotation = Quaternion.LookRotation((transform.position - targetPosition).normalized);
    }
}
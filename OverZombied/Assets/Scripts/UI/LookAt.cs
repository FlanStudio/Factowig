using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = target.position;        
    }

    private void Update()
    {
        targetPosition.x = transform.position.x;
        transform.rotation = Quaternion.LookRotation((transform.position - targetPosition).normalized);
    }
}
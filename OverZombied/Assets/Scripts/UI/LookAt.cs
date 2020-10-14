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
        targetPosition.x = transform.position.x;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - targetPosition).normalized);
    }
}
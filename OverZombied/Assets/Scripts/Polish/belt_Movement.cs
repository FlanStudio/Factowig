using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class belt_Movement : MonoBehaviour
{
    public Transform endpoint;
    public float speed = 0.0f;

    private void OnTriggerStay(Collider other)
    {
        other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
    }
}

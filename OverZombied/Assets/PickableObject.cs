using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickableObject : MonoBehaviour
{
    public PickableObjectData data;
    public Rigidbody rb;

    public bool throwable = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


}

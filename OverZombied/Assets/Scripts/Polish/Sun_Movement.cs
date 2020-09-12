using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;

public class Sun_Movement : MonoBehaviour
{
    //public Transform initial_pos = null;
    //public Transform final_pos = null;
    public Transform sun_light = null;
    public float day_time = 24.0f;
    public float rotation_amount = 0.0f;

    private float rotation_per_second = 0.0f;
    private Quaternion final_rotation;

    // Start is called before the first frame update
    void Start()
    {
        //rotation_amount = final_pos.y - initial_pos.y;
        rotation_per_second = rotation_amount / day_time;
        final_rotation = sun_light.transform.rotation * Quaternion.AngleAxis(rotation_amount, Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        if (sun_light.transform.rotation != final_rotation)
        {
            sun_light.transform.rotation = sun_light.transform.rotation * Quaternion.AngleAxis(rotation_per_second * Time.deltaTime, Vector3.right);
        }
    }
}

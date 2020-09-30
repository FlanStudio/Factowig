using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.ProBuilder;

public class Sun_Movement : MonoBehaviour
{
    //public float LOG;
    public Transform sun_light = null;
    public float day_time = 24.0f;
    public float decreasing_speed = 0.025f;
    public float rotation_amount = 0.0f;
    public float afternoon_angle = 25.0f;
    public float minimum_intensity = 0.3f;

    private float initial_intensity = 0.0f;
    private float rotation_per_second = 0.0f;
    //private float initial_x_rotation_pos = 0.0f;
    private Quaternion final_rotation;
    private new Light light;

    private void Awake()
    {
        light = gameObject.GetComponent<Light>();
    }

    void Start()
    {
        //Total rotation to do + final rotation position assigned
        rotation_per_second = rotation_amount / day_time;
        final_rotation = sun_light.transform.rotation * Quaternion.AngleAxis(rotation_amount, Vector3.right);
        initial_intensity = light.intensity;

        //initial_x_rotation_pos = sun_light.transform.localEulerAngles.x;
        //LOG = ((afternoon_angle - initial_x_rotation_pos) / Mathf.Abs(rotation_per_second));
        //decreasing_speed = light.intensity / (day_time - ((afternoon_angle - initial_x_rotation_pos) / Mathf.Abs(rotation_per_second)));
    }

    void Update()
    {
        //Rotate the light
        if (sun_light.transform.rotation != final_rotation)
        {
            sun_light.transform.rotation = sun_light.transform.rotation * Quaternion.AngleAxis(rotation_per_second * Time.deltaTime, Vector3.right);

            //Decrease shynyness
            if (sun_light.transform.localEulerAngles.x > afternoon_angle &&
                light.intensity > minimum_intensity)
            {
                light.intensity -= decreasing_speed * Time.deltaTime;
            }
            //else if(light.intensity <= initial_intensity)
            //{
            //    light.intensity += decreasing_speed * Time.deltaTime;
            //}
        }
    }
}

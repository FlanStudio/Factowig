using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    
    void Update()
    {
        GetComponent<Text>().text = Time.time.ToString("0.00");
    }
}

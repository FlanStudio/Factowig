using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private PlaceableSurfaceSelector selector;

    private void Awake()
    {
        selector = GetComponent<PlaceableSurfaceSelector>();
    }

    private void Update()
    {
        switch(InputController.Instance.controls)
        {
            case InputController.Controls.KeyboardMouse:
                {

                    break;
                }
            case InputController.Controls.Controller:
                {
                    break;
                }
        }
    }
}

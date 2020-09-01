using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private PlaceableSurfaceSelector selector;

    [SerializeField]
    private GameObject pickedGameObject;

    private void Awake()
    {
        selector = GetComponent<PlaceableSurfaceSelector>();
    }

    private void Update()
    {
        #region ACTION KEY CHECK
        switch (InputController.Instance.controls)
        {
            case InputController.Controls.KeyboardMouse:
                {
                    if(InputController.Instance.keyboard.eKey.wasPressedThisFrame)
                    {
                        ActionKeyPressed();
                    }

                    break;
                }
            case InputController.Controls.Controller:
                {
                    if(InputController.Instance.gamepad.buttonSouth.wasPressedThisFrame)
                    {
                        ActionKeyPressed();
                    }

                    break;
                }
        }
        #endregion
    }

    private void ActionKeyPressed()
    {
        if (selector.selectedSurface == null)
            return;

        if(selector.selectedSurface.pickableObject != null)
        {
            if(pickedGameObject == null)
            {
                pickedGameObject = selector.selectedSurface.pickableObject;
                selector.selectedSurface.pickableObject = null;
                pickedGameObject.SetActive(false);
            }
        }
        else
        {
            if(pickedGameObject != null)
            {
                selector.selectedSurface.PlacePickableObject(pickedGameObject);
                pickedGameObject = null;
            }
        }
    }
}
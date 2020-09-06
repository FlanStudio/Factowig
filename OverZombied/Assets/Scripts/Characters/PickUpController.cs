using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private MovementController movementController;

    private Selector selector;

    [SerializeField]
    private GameObject pickedGameObject;

    private void Awake()
    {
        selector = GetComponent<Selector>();
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        #region ACTION KEY CHECK
        switch (InputController.Instance.controlsMode[movementController.playerID])
        {
            case InputController.ControlsMode.KeyboardMouse:
                {
                    if(InputController.Instance.playerInput[movementController.playerID].keyboard.eKey.wasPressedThisFrame)
                    {
                        ActionKeyPressed();
                    }

                    break;
                }
            case InputController.ControlsMode.Controller:
                {
                    if (InputController.Instance.playerInput[movementController.playerID].gamepad == null)
                        break;

                    if(InputController.Instance.playerInput[movementController.playerID].gamepad.buttonSouth.wasPressedThisFrame)
                    {
                        ActionKeyPressed();
                    }

                    break;
                }
        }
        #endregion

        #region THROW KEY CHECK
        switch (InputController.Instance.controlsMode[movementController.playerID])
        {
            case InputController.ControlsMode.KeyboardMouse:
                {
                    if (InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.wasPressedThisFrame)
                    {
                        ThrowKeyPressed();
                    }
                    else if(InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.wasReleasedThisFrame)
                    {
                        ThrowKeyReleased();
                    }

                    break;
                }
            case InputController.ControlsMode.Controller:
                {
                    if (InputController.Instance.playerInput[movementController.playerID].gamepad == null)
                        break;

                    if (InputController.Instance.playerInput[movementController.playerID].gamepad.buttonWest.wasPressedThisFrame)
                    {
                        ThrowKeyPressed();
                    }
                    else if (InputController.Instance.playerInput[movementController.playerID].gamepad.buttonWest.wasReleasedThisFrame)
                    {
                        ThrowKeyReleased();
                    }

                    break;
                }
        }
        #endregion
    }

    private void ActionKeyPressed()
    {
        if(selector.selectedSurface != null)
        {
            if (selector.selectedSurface.pickableObject != null)
            {
                if (pickedGameObject == null)
                {
                    pickedGameObject = selector.selectedSurface.pickableObject;
                    selector.selectedSurface.pickableObject = null;
                    pickedGameObject.SetActive(false);
                }
            }
            else
            {
                if (pickedGameObject != null)
                {
                    selector.selectedSurface.PlacePickableObject(pickedGameObject);
                    pickedGameObject = null;
                }
            }
        }

        if(selector.selectedGenerator != null)
        {
            if(pickedGameObject == null)
            {
                GameObject obj = selector.selectedGenerator.GetObject();
                pickedGameObject = obj;
            }
        }
    }

    private void ThrowKeyPressed()
    {
        //IF YOU HAVE SOMETHING THROWABLE IN HAND
        movementController.move = false;
    }

    private void ThrowKeyReleased()
    {
        //TODO: THROW THE HOLDED OBJECT IN THE LOOKING DIRECTION
        movementController.move = true;
    }
}
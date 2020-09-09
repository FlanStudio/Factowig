using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private MovementController movementController;

    private Selector selector;

    [SerializeField]
    private Ingredient pickedObject;

    public Vector3 localThrowDirection = Vector3.forward;
    public float throwStrength = 10f;

    public float dropDistance = 1f;

    private bool startedThrowing = false;

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
                        UseKeyPressed();
                    }
                    else if(InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.wasReleasedThisFrame)
                    {
                        UseKeyReleased();
                    }
                    else if (InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.isPressed)
                    {
                        UseKeyRepeated();
                    }

                    break;
                }
            case InputController.ControlsMode.Controller:
                {
                    if (InputController.Instance.playerInput[movementController.playerID].gamepad == null)
                        break;

                    if (InputController.Instance.playerInput[movementController.playerID].gamepad.buttonWest.wasPressedThisFrame)
                    {
                        UseKeyPressed();
                    }
                    else if (InputController.Instance.playerInput[movementController.playerID].gamepad.buttonWest.wasReleasedThisFrame)
                    {
                        UseKeyReleased();
                    }
                    else if (InputController.Instance.playerInput[movementController.playerID].gamepad.buttonWest.isPressed)
                    {
                        UseKeyRepeated();
                    }

                    break;
                }
        }
        #endregion
    }

    private void ActionKeyPressed()
    {
        selector.Select();

        if(selector.selectedClient != null)
        {
            if (pickedObject != null && pickedObject.data.type == IngredientData.TYPE.RESOURCE)
            {
                selector.selectedClient.GiveIngredient(pickedObject);
                pickedObject = null;
            }
        }

        else if(selector.selectedSurface != null)
        {
            if (selector.selectedSurface.pickableObject != null)
            {
                if (pickedObject == null)
                {
                    pickedObject = selector.selectedSurface.pickableObject.GetComponent<Ingredient>();
                    selector.selectedSurface.pickableObject = null;
                    pickedObject.gameObject.SetActive(false);
                }
            }
            else
            {
                if (pickedObject != null)
                {
                    selector.selectedSurface.PlacePickableObject(pickedObject.gameObject);
                    pickedObject = null;
                }
            }
        }

        else if(selector.selectedGenerator != null)
        {
            if(pickedObject == null)
            {
                GameObject obj = selector.selectedGenerator.GetObject();
                pickedObject = obj.GetComponent<Ingredient>();
            }
        }

        else if(selector.groundObject != null)
        {
            if(pickedObject == null)
            {
                GameObject obj = selector.groundObject;
                pickedObject = obj.GetComponent<Ingredient>();
                pickedObject.rb.isKinematic = true;
                pickedObject.gameObject.SetActive(false);
            }
        }

        else
        {
            if(pickedObject != null)
            {
                //Drop on the floor
                pickedObject.transform.position = transform.position + transform.forward * dropDistance + new Vector3(0f, pickedObject.GetComponent<Renderer>().bounds.extents.y, 0f); ;
                pickedObject.gameObject.SetActive(true);
                pickedObject.rb.isKinematic = false;
                pickedObject = null;
            }
        }
    }

    private void UseKeyRepeated()
    {
        selector.Select();

        if (selector.selectedClient != null)
        {
            if (pickedObject != null && pickedObject.data.type == IngredientData.TYPE.TOOL)
            {
                selector.selectedClient.UseTool(pickedObject);
            }
        }
    }

    private void UseKeyPressed()
    {
        if (selector.selectedClient != null)
            return;

        if(pickedObject != null && pickedObject.throwable)
        {
            startedThrowing = true;
            movementController.move = false;
        }    
    }

    private void UseKeyReleased()
    {
        if(startedThrowing && pickedObject != null && pickedObject.throwable)
        {
            pickedObject.rb.isKinematic = false;
            pickedObject.gameObject.SetActive(true);
            pickedObject.transform.position = transform.position + new Vector3(0f, 1f, 0f) + transform.forward * dropDistance;
            pickedObject.rb.AddForce(transform.TransformDirection(localThrowDirection).normalized * throwStrength, ForceMode.Impulse);
            pickedObject = null;
            startedThrowing = false;
        }

        movementController.move = true;
    }
}
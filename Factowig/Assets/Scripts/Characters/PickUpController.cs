using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpController : MonoBehaviour
{
    public MovementController movementController;

    private Selector selector;

    [SerializeField]
    private Ingredient pickedObject;

    public Vector3 localThrowDirection = Vector3.forward;
    public float throwStrength = 10f;
    public float throwHeight = 1f;

    public float dropDistance = 1f;

    public GameObject hand = null;
    public Image sandwich = null;

    private bool startedThrowing = false;

    private void Awake()
    {
        selector = GetComponent<Selector>();
        movementController = GetComponent<MovementController>();
    }

    private void Update()
    {
        #region ACTION KEY CHECK
        switch (InputController.Instance.playerInput[movementController.playerID].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                {
                    if(movementController.playerID == 1 && InputController.Instance.playerInput[0].controlMode == InputController.ControlsMode.KeyboardMouse)
                    {
                        if (InputController.Instance.playerInput[movementController.playerID].keyboard.kKey.wasPressedThisFrame)
                        {
                            StartCoroutine(ActionKeyPressed());
                        }
                    }

                    else if(InputController.Instance.playerInput[movementController.playerID].keyboard.eKey.wasPressedThisFrame)
                    {
                        StartCoroutine(ActionKeyPressed());
                    }

                    break;
                }
            case InputController.ControlsMode.Controller:
                {
                    if (InputController.Instance.playerInput[movementController.playerID].gamepad == null)
                        break;

                    if(InputController.Instance.playerInput[movementController.playerID].gamepad.buttonSouth.wasPressedThisFrame)
                    {
                        StartCoroutine(ActionKeyPressed());
                    }

                    break;
                }
        }
        #endregion

        #region THROW KEY CHECK
        switch (InputController.Instance.playerInput[movementController.playerID].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                {
                    if (movementController.playerID == 1 && InputController.Instance.playerInput[0].controlMode == InputController.ControlsMode.KeyboardMouse)
                    {
                        if (InputController.Instance.playerInput[movementController.playerID].keyboard.jKey.wasPressedThisFrame)
                        {
                            UseKeyPressed();
                        }
                        else if (InputController.Instance.playerInput[movementController.playerID].keyboard.jKey.wasReleasedThisFrame)
                        {
                            UseKeyReleased();
                        }
                        else if (InputController.Instance.playerInput[movementController.playerID].keyboard.jKey.isPressed)
                        {
                            UseKeyRepeated();
                        }
                    }
                    else
                    {
                        if (InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.wasPressedThisFrame)
                        {
                            UseKeyPressed();
                        }
                        else if (InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.wasReleasedThisFrame)
                        {
                            UseKeyReleased();
                        }
                        else if (InputController.Instance.playerInput[movementController.playerID].keyboard.qKey.isPressed)
                        {
                            UseKeyRepeated();
                        }
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

        #region UPDATE YOUR HOLDED ITEM SPRITE
        if (pickedObject != null)
        {
            if (sandwich.sprite != pickedObject.data.sandwich)
            {
                sandwich.enabled = true;
                sandwich.sprite = pickedObject.data.sandwich;
            }
        }
        else
        {
            sandwich.enabled = false;
            sandwich.sprite = null;
        }
        #endregion
    }

    private IEnumerator ActionKeyPressed()
    {
        selector.Select();

        if(selector.selectedChair != null)
        {
            if(pickedObject != null)
            {
                if(pickedObject.data.isWig && selector.selectedChair.PlaceWig(pickedObject))
                    pickedObject = null;               
            }
            else
            {
                Ingredient ret = selector.selectedChair.RemoveWig();
                if (ret) pickedObject = ret;
            }
        }

        else if(selector.selectedDeliverer != null)
        {            
            if (pickedObject != null)
            {
                pickedObject.transform.SetParent(null);
                pickedObject.transform.position = transform.position;
                if (selector.selectedDeliverer.Deliver(pickedObject))
                {
                    pickedObject = null;
                    AudioManager.Instance.PlaySoundEffect(AudioManager.FX.BELT);
                }
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

                    if (pickedObject)
                    {
                        pickedObject.transform.SetParent(hand.transform);
                        pickedObject.transform.localPosition = Vector3.zero;
                        pickedObject.transform.localRotation = Quaternion.identity;
                        pickedObject.rb.isKinematic = true;
                        pickedObject.collider.enabled = false;
                    }
                }
            }
            else
            {
                if (pickedObject != null)
                {
                    selector.selectedSurface.PlacePickableObject(pickedObject);
                    pickedObject = null;

                    AudioManager.Instance.PlaySoundEffect(AudioManager.FX.PLACE);
                }
            }
        }

        else if(selector.selectedWigDispenser != null)
        {
            if(pickedObject == null)
            {
                GameObject obj = selector.selectedWigDispenser.GetObject();
                pickedObject = obj != null ? obj.GetComponent<Ingredient>() : null;
            }
        }

        else if(selector.selectedGenerator != null)
        {
            if(pickedObject == null)
            {
                GameObject obj = selector.selectedGenerator.GetObject();
                pickedObject = obj.GetComponent<Ingredient>();
                movementController.playerAnimator.SetTrigger("Pick or Place");

                AudioManager.Instance.PlaySoundEffect(AudioManager.FX.PICK);

                yield return new WaitUntil(() => { AnimatorStateInfo stateInfo = movementController.playerAnimator.GetCurrentAnimatorStateInfo(0); if (stateInfo.IsName("Pick or Place") && stateInfo.normalizedTime >= 0.5f ) return true; else return false; });

                if(pickedObject)
                {
                    pickedObject.transform.SetParent(hand.transform);
                    pickedObject.transform.localPosition = Vector3.zero;
                    pickedObject.transform.localRotation = Quaternion.identity;
                    pickedObject.rb.isKinematic = true;
                    pickedObject.collider.enabled = false;
                    pickedObject.gameObject.SetActive(true);
                }

                yield return new WaitUntil(() => { AnimatorStateInfo stateInfo = movementController.playerAnimator.GetCurrentAnimatorStateInfo(0); if ((stateInfo.IsName("idle") || stateInfo.IsName("running"))) return true; else return false; });

                if(pickedObject)
                    pickedObject.gameObject.SetActive(false);
            }
        }

        else if(selector.groundObject != null)
        {
            if(pickedObject == null)
            {
                pickedObject = selector.groundObject;
                pickedObject.transform.SetParent(hand.transform);

                pickedObject.transform.localPosition = Vector3.zero;
                pickedObject.transform.localRotation = Quaternion.identity;
                pickedObject.rb.isKinematic = true;
                pickedObject.collider.enabled = false;

                pickedObject.gameObject.SetActive(false);            
            }
        }

        else
        {
            if(pickedObject != null && pickedObject.data.throwable)
            {
                //Drop on the floor
                pickedObject.transform.SetParent(null);
                pickedObject.transform.rotation = transform.rotation;
                pickedObject.transform.position = transform.position + transform.forward * dropDistance + new Vector3(0f, pickedObject.renderer.bounds.extents.y, 0f); ;
                pickedObject.rb.isKinematic = false;
                pickedObject.collider.enabled = true;
                pickedObject.gameObject.SetActive(true);
                pickedObject = null;
            }
        }

        yield return null;
    }

    private void UseKeyRepeated()
    {
        selector.Select();

        if (selector.selectedChair != null)
        {
            bool stayHere;
            if (selector.selectedChair.ApplyIngredient(pickedObject, out stayHere))
            {
                pickedObject = null;
            }

            if(stayHere)
            {
                movementController.move = movementController.rotate = false;
                transform.position = selector.selectedChair.transform.position + selector.selectedChair.transform.forward * dropDistance;
                transform.LookAt(selector.selectedChair.transform.position);

                movementController.playerAnimator.SetBool("working", true);

                if (!AudioManager.Instance.IsPlayingFX(AudioManager.FX.WORKING))
                    AudioManager.Instance.PlaySoundEffect(AudioManager.FX.WORKING, true);
            }
            else
            {
                movementController.move = movementController.rotate = true;
                movementController.playerAnimator.SetBool("working", false);
            }
        }
    }

    private void UseKeyPressed()
    {
        if (selector.selectedChair != null)
        {
            //selector.selectedChair.UseToolStarted(this, pickedObject);            
            return;
        }
        else if(pickedObject != null && pickedObject.data.throwable)
        {
            startedThrowing = true;
            movementController.move = false;
            pickedObject.collider.enabled = true;
        }    
    }

    private void UseKeyReleased()
    {
        if(selector.selectedChair != null)
        {
            movementController.playerAnimator.SetBool("working", false);
            AudioManager.Instance.StopSoundEffects();
        }
        else if(startedThrowing && pickedObject != null && pickedObject.data.throwable)
        {
            pickedObject.transform.SetParent(null);
            pickedObject.collider.enabled = true;
            pickedObject.rb.isKinematic = false;
            pickedObject.transform.rotation = transform.rotation;
            pickedObject.transform.position = transform.position + new Vector3(0f, throwHeight, 0f) + transform.forward * dropDistance;
            pickedObject.gameObject.SetActive(true);
            pickedObject.rb.AddForce(transform.TransformDirection(localThrowDirection).normalized * throwStrength, ForceMode.Impulse);
            pickedObject = null;
            startedThrowing = false;

            AudioManager.Instance.PlaySoundEffect(AudioManager.FX.CLICK);
        }

        movementController.move = true;
        movementController.rotate = true;
    }
}
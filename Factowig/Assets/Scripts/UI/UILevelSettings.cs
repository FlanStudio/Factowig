using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelSettings : UILevel
{
    public GameObject BBack = null;
    public GameObject escBack = null;

    private bool corStarted = false;

    protected override void OnEnable()
    {
        base.OnEnable();

        AudioManager.Instance.PlayBSO(AudioManager.BSO.SETTINGS);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic) || Gamepad.current.leftStick.ReadValue().magnitude >= InputController.idleStickThreshold)
            {
                if (!BBack.activeSelf)
                {
                    BBack.SetActive(true);
                    escBack.SetActive(false);
                }
            }

            if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.delta.IsActuated(0.2f))
            {
                if (!escBack.activeSelf)
                {
                    escBack.SetActive(true);
                    BBack.SetActive(false);
                }
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                StartCoroutine(OnBackPressed());
            }

            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                StartCoroutine(OnBackPressed());
            }
        }
        else
        {
            InputController.PlayerInput playerInput = InputController.Instance.playerInput[0];

            switch (playerInput.controlMode)
            {
                case InputController.ControlsMode.KeyboardMouse:
                    if (BBack.activeSelf)
                    {
                        BBack.SetActive(false);
                        escBack.SetActive(true);
                    }

                    if (playerInput.keyboard.escapeKey.wasPressedThisFrame)
                    {
                        StartCoroutine(OnBackPressed());
                    }

                    break;
                case InputController.ControlsMode.Controller:
                    if (escBack.activeSelf)
                    {
                        escBack.SetActive(false);
                        BBack.SetActive(true);
                    }

                    if (playerInput.gamepad.buttonEast.wasPressedThisFrame)
                    {
                        StartCoroutine(OnBackPressed());
                    }
                    break;
            }
        }
    }

    private IEnumerator OnBackPressed()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (!corStarted)
            {
                corStarted = true;

                AudioManager.Instance.PlayBSO(AudioManager.BSO.LEVEL);

                StartCoroutine(FadeOut());

                yield return new WaitUntil(() => fadeEnded == true);

                VCamReferencer.Instance.vcam1.gameObject.SetActive(true);
                VCamReferencer.Instance.vcam2.gameObject.SetActive(false);

                yield return new WaitForSeconds(2);

                UIController.Instance.TransitionFromTo(id, 0);

                corStarted = false;
            }
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", AudioManager.musicVolume);
            PlayerPrefs.SetFloat("fxVolume", AudioManager.fxVolume);
            PlayerPrefs.Save();

            UIController.Instance.TransitionFromTo(id, 0);
        }  
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSettings : UILevel
{
    public GameObject BBack = null;
    public GameObject escBack = null;

    protected override void OnEnable()
    {
        base.OnEnable();

        AudioManager.Instance.PlaySettingsBSO();
    }

    private void Update()
    {
        InputController.PlayerInput playerInput = InputController.Instance.playerInput[0];

        switch(playerInput.controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if(BBack.activeSelf)
                {
                    BBack.SetActive(false);
                    escBack.SetActive(true);
                }

                if(playerInput.keyboard.escapeKey.wasPressedThisFrame)
                {
                    SaveAndGoPauseMenu();
                }

                break;
            case InputController.ControlsMode.Controller:
                if(escBack.activeSelf)
                {
                    escBack.SetActive(false);
                    BBack.SetActive(true);
                }

                if(playerInput.gamepad.buttonEast.wasPressedThisFrame)
                {
                    SaveAndGoPauseMenu();
                }
                break;
        }
    }

    private void SaveAndGoPauseMenu()
    {
        PlayerPrefs.SetFloat("musicVolume", AudioManager.musicVolume);
        PlayerPrefs.SetFloat("fxVolume", AudioManager.fxVolume);
        PlayerPrefs.Save();

        UIController.Instance.TransitionFromTo(id, 0);
    }
}
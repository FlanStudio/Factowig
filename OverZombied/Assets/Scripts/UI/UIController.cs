using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject PauseMenu;

    [SerializeField]
    private Selectable startSelected = null;

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void Awake()
    {
        if (startSelected)
            startSelected.Select();
    }

    private void Update()
    {
        for (int i = 0; i < InputController.Instance.playerInput.Length; ++i)
        {
            InputController.PlayerInput playerInput = InputController.Instance.playerInput[i];

            switch(playerInput.controlMode)
            {
                case InputController.ControlsMode.KeyboardMouse:
                    if(playerInput.keyboard.escapeKey.wasPressedThisFrame)
                    {
                        TogglePauseMenu();
                    }
                    break;
                case InputController.ControlsMode.Controller:
                    if(playerInput.gamepad.startButton.wasPressedThisFrame)
                    {
                        TogglePauseMenu();
                    }
                    break;
            }
        }
    }

    private void TogglePauseMenu()
    {
        if(PauseMenu != null)
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            Time.timeScale = PauseMenu.activeSelf ? 0f : 1f;
        }
    }
}
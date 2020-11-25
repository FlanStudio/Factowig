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
        //TODO: SAVE SETTINGS AND PROGRESSION AND QUIT THE GAME
        Application.Quit();
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnRestartPressed()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnSaveAndQuitPressed()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(0);
    }

    public void TogglePauseMenu()
    {
        if (PauseMenu != null)
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);
            Time.timeScale = PauseMenu.activeSelf ? 0f : 1f;
        }
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
}
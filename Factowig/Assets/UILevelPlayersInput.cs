using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UILevelPlayersInput : UILevel
{
    [SerializeField]
    private Image player1 = null;
    
    [SerializeField]
    private Image player2 = null;

    [SerializeField]
    private Image playLabel = null;

    [SerializeField]
    private Sprite keyboardImg = null;

    [SerializeField]
    private Sprite gamepadImg = null;

    [SerializeField]
    private Sprite spaceLabel = null;

    private void Update()
    {
        InputController.PlayerInput player1Input = InputController.Instance.playerInput[0];
        switch (player1Input.controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if (player1Input.keyboard.spaceKey.wasPressedThisFrame)
                    Play();
                break;
            case InputController.ControlsMode.Controller:
                if (player1Input.gamepad.startButton.wasPressedThisFrame)
                    Play();
                break;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(InputController.Instance.SetInput(InputController.ControlsMode.KeyboardMouse, 0) == 0)
            {
                player1.sprite = keyboardImg;
                playLabel.sprite = spaceLabel;
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if(InputController.Instance.SetInput(InputController.ControlsMode.KeyboardMouse, 1) == 1)
            {
                player2.sprite = keyboardImg;
            }
        }

        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (gamepad.startButton.wasPressedThisFrame)
            {
                int id = InputController.Instance.SetInput(InputController.ControlsMode.Controller, -1, gamepad);

                switch (id)
                {
                    case 0: player1.sprite = gamepadImg; break;
                    case 1: player2.sprite = gamepadImg; break;
                }
            }
        }  
    }

    private void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
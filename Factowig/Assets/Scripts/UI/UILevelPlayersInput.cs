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
    private Image escImage = null;

    [SerializeField]
    private Image BImage = null;

    [SerializeField]
    private Sprite player1DefaultLabel = null;

    [SerializeField]
    private Sprite player2DefaultLabel = null;

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
                if (player1Input.keyboard.escapeKey.wasPressedThisFrame)
                    Back();
                break;
            case InputController.ControlsMode.Controller:
                if (player1Input.gamepad.startButton.wasPressedThisFrame)
                    Play();
                if (player1Input.gamepad.buttonEast.wasPressedThisFrame)
                    Back();
                break;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if(InputController.Instance.SetInput(InputController.ControlsMode.KeyboardMouse, 0) == 0)
            {
                player1.sprite = keyboardImg;
                playLabel.sprite = spaceLabel;
                BImage.gameObject.SetActive(false);
                escImage.gameObject.SetActive(true);
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
                    case 0: 
                        player1.sprite = gamepadImg; 
                        escImage.gameObject.SetActive(false); 
                        BImage.gameObject.SetActive(true);
                        break;
                    case 1: player2.sprite = gamepadImg; break;
                }
            }
        }  
    }

    private void Play()
    {
        SceneManager.LoadScene("Tutorial");
    }

    private void Back()
    {
        player1.sprite = player1DefaultLabel;
        player2.sprite = player2DefaultLabel;

        for(int i = 0; i < 2; ++i)
        {
            InputController.Instance.playerInput[i].controlMode = InputController.ControlsMode.None;
            InputController.Instance.playerInput[i].gamepad = null;
        }

        UIController.Instance.TransitionFromTo(2, 1);
    }
}
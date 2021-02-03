using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILevelScore : MonoBehaviour
{
    #region REFERENCES
    [System.Serializable]
    private struct Star
    {
        public Image empty;
        public Image filled;
    }

    [System.Serializable]
    private struct ButtonUI
    {
        public Image controller;
        public Image keyboard;
    }

    [SerializeField]
    private TextMeshProUGUI deliveriesText = null;

    [SerializeField]
    private TextMeshProUGUI bonusText = null;

    [SerializeField]
    private TextMeshProUGUI failsText = null;

    [SerializeField]
    private TextMeshProUGUI totalText = null;

    [SerializeField]
    private Star[] stars = new Star[3];

    [SerializeField]
    private ButtonUI replayButton;

    [SerializeField]
    private ButtonUI backButton;
    #endregion

    private void OnEnable()
    {
        Time.timeScale = 0f;

        deliveriesText.text = GameManager.Instance.deliveredMoney.ToString("");
        bonusText.text = GameManager.Instance.bonusMoney.ToString("0.00");
        failsText.text = GameManager.Instance.failedMoney.ToString("");
        totalText.text = GameManager.Instance.currentMoney.ToString("0.00");

        for(int i = 0; i < 3; ++i)
        {
            if(i+1 <= GameManager.Instance.stars)
            {
                stars[i].empty.gameObject.SetActive(false);
                stars[i].filled.gameObject.SetActive(true);
            }
            else
            {
                stars[i].empty.gameObject.SetActive(true);
                stars[i].filled.gameObject.SetActive(false);
            }
        }

        switch(InputController.Instance.playerInput[0].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                replayButton.keyboard.gameObject.SetActive(true);
                replayButton.controller.gameObject.SetActive(false);

                backButton.keyboard.gameObject.SetActive(true);
                backButton.controller.gameObject.SetActive(false);
                break;
            case InputController.ControlsMode.Controller:
                replayButton.keyboard.gameObject.SetActive(false);
                replayButton.controller.gameObject.SetActive(true);

                backButton.keyboard.gameObject.SetActive(false);
                backButton.controller.gameObject.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        switch (InputController.Instance.playerInput[0].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if(InputController.Instance.playerInput[0].keyboard.spaceKey.wasPressedThisFrame)
                    Restart();
                else if(InputController.Instance.playerInput[0].keyboard.escapeKey.wasPressedThisFrame)
                    MainMenu(); 
                break;
            case InputController.ControlsMode.Controller:
                if (InputController.Instance.playerInput[0].gamepad.buttonSouth.wasPressedThisFrame)
                    Restart();
                else if (InputController.Instance.playerInput[0].gamepad.buttonEast.wasPressedThisFrame)
                    MainMenu();
                    break;
        }
    }

    private void Restart()
    {
        //TODO: SAVE PROGRESS ON DISK

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MainMenu()
    {
        //TODO: SAVE PROGRESS ON DISK
        
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
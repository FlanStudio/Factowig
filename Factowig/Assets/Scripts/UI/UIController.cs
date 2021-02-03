using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField]
    private GameObject PauseMenu = null;

    [SerializeField]
    private GameObject ScoreScreen = null;

    [SerializeField]
    private List<GameObject> uiLevels = new List<GameObject>();
    private int currentLevel = 0;

    public void OnQuitPressed()
    {
        //TODO: SAVE SETTINGS AND PROGRESSION AND QUIT THE GAME
        Application.Quit();
    }

    public void OnRestartPressed()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnSettingsPressed()
    {
        uiLevels[currentLevel].SetActive(false);
        currentLevel += 1;
        uiLevels[currentLevel].SetActive(true);
    }

    public void OnSaveAndQuitPressed()
    {
        TogglePauseMenu();
        SceneManager.LoadScene(0);
    }

    public void TogglePauseMenu()
    {
        PauseMenu.SetActive(!PauseMenu.activeSelf);
        Time.timeScale = PauseMenu.activeSelf ? 0f : 1f;

        if (!PauseMenu.activeSelf)
        {
            AudioManager.Instance.PlayBSO(AudioManager.BSO.LEVEL);
        }
    }

    public void EnableScoreScreen()
    {
        ScoreScreen.SetActive(true);
    }

    public void TransitionFromTo(int from, int to)
    {
        if (currentLevel != from)
            return;

        uiLevels[from].SetActive(false);
        currentLevel = to;
        uiLevels[to].SetActive(true);
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        #region TOGGLE PAUSE MENU
        if (PauseMenu && uiLevels.Count > 0 && currentLevel == 0)
        {
            InputController.PlayerInput playerInput = InputController.Instance.playerInput[0];

            switch (playerInput.controlMode)
            {
                case InputController.ControlsMode.KeyboardMouse:
                    if (playerInput.keyboard.escapeKey.wasPressedThisFrame)
                        TogglePauseMenu();
                    break;
                case InputController.ControlsMode.Controller:
                    if (playerInput.gamepad.startButton.wasPressedThisFrame || (playerInput.gamepad.buttonEast.wasPressedThisFrame && PauseMenu.activeSelf))
                        TogglePauseMenu();

                    break;
            }  
        }
        #endregion
    }
}
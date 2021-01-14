using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelSaves : UILevel
{
    [SerializeField]
    private GameObject escLabel;
    [SerializeField]
    private GameObject bLabel;

    private void Update()
    {
        switch(InputController.Instance.playerInput[0].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if(!escLabel.activeSelf)
                {
                    escLabel.SetActive(true);
                    bLabel.SetActive(false);
                }
                break;
            case InputController.ControlsMode.Controller:
                if (!bLabel.activeSelf)
                {
                    bLabel.SetActive(true);
                    escLabel.SetActive(false);
                }
                break;
        }
    }
}
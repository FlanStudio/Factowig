using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSettings : UILevel
{
    public GameObject BBack = null;
    public GameObject escBack = null;

    public Selectable startSelected = null;

    private void OnEnable()
    {
        if(startSelected)
            startSelected.Select();
    }

    private void Update()
    {
        switch(InputController.Instance.playerInput[0].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if(BBack.activeSelf)
                {
                    BBack.SetActive(false);
                    escBack.SetActive(true);
                }
                break;
            case InputController.ControlsMode.Controller:
                if(escBack.activeSelf)
                {
                    escBack.SetActive(false);
                    BBack.SetActive(true);
                }
                break;
        }
    }
}
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    public enum ControlsMode { None, KeyboardMouse, Controller };

    public class PlayerInput
    {
        public ControlsMode controlMode = ControlsMode.None;
        public Gamepad gamepad = null;
        public Keyboard keyboard = null;
    }

    public PlayerInput[] playerInput { get { return InmutableData.Instance.playerInput; } set { InmutableData.Instance.playerInput = value; } }

    [SerializeField]
    private MovementController[] movementControllers = null;

    public static float idleStickThreshold = 0.2f;

    private void Awake()
    {
        Instance = this;

        SetInputs();

        //InputSystem.onDeviceChange +=
        //(device, change) =>
        //{
        //    switch (change)
        //    {
        //        case InputDeviceChange.Added:                 
        //        case InputDeviceChange.Disconnected:                 
        //        case InputDeviceChange.Reconnected:                   
        //        case InputDeviceChange.Removed:
        //        default:
        //            SetInputs();
        //            break;
        //    }
        //};
    }

    private void Update()
    {
        //for(int i = 0; i < 2; ++i)
        //{
        //    if(playerInput[i].keyboard != null)
        //    {
        //        if(playerInput[i].keyboard.anyKey.wasPressedThisFrame)
        //        {
        //            playerInput[i].controlMode = ControlsMode.KeyboardMouse;
        //        }
        //    }

        //    if(playerInput[i].gamepad != null)
        //    {
        //        if ((playerInput[i].gamepad.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic)) || playerInput[i].gamepad.leftStick.ReadValue() != Vector2.zero)
        //        {
        //            playerInput[i].controlMode = ControlsMode.Controller;
        //        }
        //    }
        //}

        #region SINGLEPLAYER
        if (playerInput[1].controlMode == ControlsMode.None)
            switch (playerInput[0].controlMode)
            {
                case ControlsMode.KeyboardMouse:
                    if (playerInput[0].keyboard.tabKey.wasPressedThisFrame)
                    {
                        foreach(MovementController controller in movementControllers)
                        {
                            if (controller.playerID == 0)
                                controller.playerID = 1;
                            else
                                controller.playerID = 0;
                        }                       
                    }
                    break;
                case ControlsMode.Controller:
                    if (playerInput[0].gamepad != null && InputController.Instance.playerInput[0].gamepad.buttonNorth.wasPressedThisFrame)
                    {
                        foreach (MovementController controller in movementControllers)
                        {
                            if (controller.playerID == 0)
                                controller.playerID = 1;
                            else
                                controller.playerID = 0;
                        }                        
                    }
                    break;
            }
        #endregion
    }

    public int SetInput(ControlsMode mode, int id = -1, Gamepad gamepad = null)
    {
        switch(id)
        {
            case -1:
                for (int i = 0; i < 2; ++i)
                {
                    if (playerInput[i].controlMode == ControlsMode.None)
                    {
                        playerInput[i].controlMode = mode;
                        if (gamepad != null)
                            playerInput[i].gamepad = gamepad;

                        return i;
                    }
                    else if (playerInput[i].gamepad == gamepad && gamepad != null)
                        return -1;
                }
                break;
            case 0:
            case 1:
                if(playerInput[id].controlMode == ControlsMode.None)
                {
                    playerInput[id].controlMode = mode;
                    return id;
                }
                break;
        }

        return -2;
    }

    private void SetInputs()
    {
        foreach(PlayerInput input in playerInput)
            input.keyboard = Keyboard.current;
    }
}
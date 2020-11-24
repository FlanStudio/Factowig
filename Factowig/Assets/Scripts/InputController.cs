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

    public PlayerInput[] playerInput = new PlayerInput[4] {new PlayerInput(), new PlayerInput(), new PlayerInput(), new PlayerInput()};

    [SerializeField]
    private MovementController[] movementControllers = null;

    private void Awake()
    {
        Instance = this;

        SetInputs();

        InputSystem.onDeviceChange +=
        (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:                 
                case InputDeviceChange.Disconnected:                 
                case InputDeviceChange.Reconnected:                   
                case InputDeviceChange.Removed:
                default:
                    SetInputs();
                    break;
            }
        };
    }

    private void Update()
    {
        for(int i = 0; i < 4; ++i)
        {
            if(playerInput[i].keyboard != null)
            {
                if(playerInput[i].keyboard.anyKey.wasPressedThisFrame)
                {
                    playerInput[i].controlMode = ControlsMode.KeyboardMouse;
                }
            }

            if(playerInput[i].gamepad != null)
            {
                if(playerInput[i].gamepad.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic))
                {
                    playerInput[i].controlMode = ControlsMode.Controller;
                }
            }
        }

        #region SINGLEPLAYER
        if (Gamepad.all.Count < 2)
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

    private void SetInputs()
    {
        playerInput[0].keyboard = Keyboard.current;
        playerInput[0].controlMode = ControlsMode.KeyboardMouse;

        for (int i = 0; i < 4; ++i)
        {
            if (i < Gamepad.all.Count)
            {
                playerInput[i].gamepad = Gamepad.all[i];
                playerInput[i].controlMode = ControlsMode.Controller;
            }
        }
    }
}

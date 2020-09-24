using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    public enum ControlsMode { KeyboardMouse, Controller };

    public class PlayerInput
    {
        public Gamepad gamepad = null;
        public Keyboard keyboard = null;
    }

    public ControlsMode[] controlsMode { get; private set; } = new ControlsMode[4] {ControlsMode.Controller, ControlsMode.Controller, ControlsMode.Controller, ControlsMode.Controller};
    public PlayerInput[] playerInput = new PlayerInput[4] {new PlayerInput(), new PlayerInput(), new PlayerInput(), new PlayerInput()};

    private void Awake()
    {
        Instance = this;

        playerInput[0].keyboard = Keyboard.current;
        controlsMode[0] = ControlsMode.KeyboardMouse;

        for (int i = 0; i < 4; ++i)
        {        
            if (i < Gamepad.all.Count)
            {
                playerInput[i].gamepad = Gamepad.all[i];
                controlsMode[i] = ControlsMode.Controller;
            }
        }

        //Hardcoding
        playerInput[0].keyboard = Keyboard.current;
        controlsMode[0] = ControlsMode.KeyboardMouse;

        controlsMode[1] = ControlsMode.Controller;
        playerInput[1].gamepad = Gamepad.all[0];
    }
}

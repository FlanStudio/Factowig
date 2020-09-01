using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    public enum Controls { KeyboardMouse, Controller };

    private Controls _controls = Controls.KeyboardMouse;
    public Controls controls { get { return _controls; } }

    public Gamepad gamepad = null;
    public Keyboard keyboard = null;

    private void Awake()
    {
        Instance = this;

        keyboard = Keyboard.current;

        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
            _controls = Controls.Controller;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{    
    private Vector2 movementNorm = Vector2.zero;

    public float speed = 15f;
    public float rotationSpeed = 10f;

    private void Awake()
    {
        Debug.Log("Game Started");
    }

    [System.Obsolete]
    private void Update()
    {
        movementNorm = Vector2.zero;

        #region MOVEMENT VECTOR
        switch (InputController.Instance.controls)
        {
            case InputController.Controls.KeyboardMouse:
                {
                    if (InputController.Instance.keyboard.wKey.isPressed)
                        movementNorm += new Vector2(0, 1);
                    if (InputController.Instance.keyboard.sKey.isPressed)
                        movementNorm += new Vector2(0, -1);
                    if (InputController.Instance.keyboard.aKey.isPressed)
                        movementNorm += new Vector2(-1, 0);
                    if (InputController.Instance.keyboard.dKey.isPressed)
                        movementNorm += new Vector2(1, 0);

                    break;
                }
                
            case InputController.Controls.Controller:
                {
                    Vector2 value = Vector2.zero;
                    value = InputController.Instance.gamepad.leftStick.ReadValue();

                    movementNorm = value;
                    
                    break;
                }
        }
        #endregion

        #region APPLY MOVEMENT
        transform.Translate(new Vector3(movementNorm.x, 0, movementNorm.y) * speed * Time.deltaTime, Space.World);
        #endregion

        #region LOOK AHEAD

        float diffAngle = Vector3.SignedAngle(transform.forward.normalized, new Vector3(movementNorm.x, 0, movementNorm.y), Vector3.up);
        transform.Rotate(Vector3.up, diffAngle * rotationSpeed * Time.deltaTime);

        #endregion
    }
}

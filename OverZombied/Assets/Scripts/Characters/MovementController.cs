using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementController : MonoBehaviour
{    
    private Vector2 movementNorm = Vector2.zero;

    public float idleStickThreshold = 0.2f;
    public float speed = 5f;
    public float rotationSpeed = 15f;

    private Selector selector;
    private Rigidbody rb;

    private int _playerID = -1;
    public int playerID { get { return _playerID; } }

    public bool move = true;
    public bool rotate = true;

    private void Awake()
    {
        _playerID = InputController.Instance.GetMyPlayerID();
        selector = GetComponent<Selector>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movementNorm = Vector2.zero;

        #region MOVEMENT VECTOR
        switch (InputController.Instance.controlsMode[playerID])
        {
            case InputController.ControlsMode.KeyboardMouse:
                {
                    if (InputController.Instance.playerInput[playerID].keyboard.wKey.isPressed)
                        movementNorm += new Vector2(0, 1);
                    if (InputController.Instance.playerInput[playerID].keyboard.sKey.isPressed)
                        movementNorm += new Vector2(0, -1);
                    if (InputController.Instance.playerInput[playerID].keyboard.aKey.isPressed)
                        movementNorm += new Vector2(-1, 0);
                    if (InputController.Instance.playerInput[playerID].keyboard.dKey.isPressed)
                        movementNorm += new Vector2(1, 0);

                    break;
                }

            case InputController.ControlsMode.Controller:
                {
                    Vector2 value = Vector2.zero;
                    if (InputController.Instance.playerInput[playerID].gamepad != null)
                        value = InputController.Instance.playerInput[playerID].gamepad.leftStick.ReadValue();

                    movementNorm = value;

                    break;
                }
        }

        if (movementNorm.magnitude < idleStickThreshold)
        {
            movementNorm = Vector2.zero;
        }

        #endregion

        #region LOOK AHEAD

        if (rotate && movementNorm != Vector2.zero)
        {
            float diffAngle = Vector3.SignedAngle(transform.forward.normalized, new Vector3(movementNorm.x, 0, movementNorm.y), Vector3.up);
            transform.Rotate(Vector3.up, diffAngle * rotationSpeed * Time.deltaTime);
        }  

        #endregion

        if (movementNorm != Vector2.zero)
        {
            selector.Select();
        }
    }

    private void FixedUpdate()
    {
        #region APPLY MOVEMENT
        if (movementNorm == Vector2.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            if (move)
                rb.MovePosition(rb.position + new Vector3(movementNorm.x, 0, movementNorm.y) * speed * Time.fixedDeltaTime);
        }
        #endregion
    }
}
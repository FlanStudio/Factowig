using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InmutableData : MonoBehaviour
{
    public static InmutableData Instance = null;

    public InputController.PlayerInput[] playerInput = new InputController.PlayerInput[2] { new InputController.PlayerInput(), new InputController.PlayerInput() };

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);
    }
}
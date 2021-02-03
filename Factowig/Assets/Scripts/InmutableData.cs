using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InmutableData : MonoBehaviour
{
    public static InmutableData Instance = null;

    public InputController.PlayerInput[] playerInput = new InputController.PlayerInput[2] { new InputController.PlayerInput(), new InputController.PlayerInput() };

    private void Awake()
    {
        if(Instance != null)
        {
            if(SceneManager.GetActiveScene().buildIndex == 0)
                Instance.playerInput = new InputController.PlayerInput[2] { new InputController.PlayerInput(), new InputController.PlayerInput() };

            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
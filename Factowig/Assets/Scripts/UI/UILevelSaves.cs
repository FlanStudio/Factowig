using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class UILevelSaves : UILevel
{
    [SerializeField]
    private GameObject escLabel;
    [SerializeField]
    private GameObject bLabel;

    private bool corStarted = false;

    private void Update()
    {
        if (Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic) || Gamepad.current.leftStick.ReadValue().magnitude >= InputController.idleStickThreshold)
        {
            if (!bLabel.activeSelf)
            {
                bLabel.SetActive(true);
                escLabel.SetActive(false);
            }
        }

        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.delta.IsActuated(0.2f))
        {
            if (!escLabel.activeSelf)
            {
                escLabel.SetActive(true);
                bLabel.SetActive(false);
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(OnBackPressed());
        }

        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            StartCoroutine(OnBackPressed());
        }
        
    }

    public void OnPlayPressed()
    {
        UIController.Instance.TransitionFromTo(1, 2);
    }

    private IEnumerator OnBackPressed()
    {
        if(!corStarted)
        {
            corStarted = true;

            StartCoroutine(FadeOut());

            yield return new WaitUntil(() => fadeEnded == true);

            VCamReferencer.Instance.vcam1.gameObject.SetActive(true);
            VCamReferencer.Instance.vcam2.gameObject.SetActive(false);

            yield return new WaitForSeconds(2);

            UIController.Instance.TransitionFromTo(1, 0);

            corStarted = false;
        }
    }
}
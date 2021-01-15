using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        switch(InputController.Instance.playerInput[0].controlMode)
        {
            case InputController.ControlsMode.KeyboardMouse:
                if(!escLabel.activeSelf)
                {
                    escLabel.SetActive(true);
                    bLabel.SetActive(false);
                }

                if(InputController.Instance.playerInput[0].keyboard.escapeKey.wasPressedThisFrame)
                {
                    StartCoroutine(OnBackPressed());
                }

                break;
            case InputController.ControlsMode.Controller:
                if (!bLabel.activeSelf)
                {
                    bLabel.SetActive(true);
                    escLabel.SetActive(false);
                }

                if (InputController.Instance.playerInput[0].gamepad.buttonEast.wasPressedThisFrame)
                {
                    StartCoroutine(OnBackPressed());
                }

                break;
        }
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("Tutorial");
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
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class UILevelSaves : UILevel
{
    [System.Serializable]
    private struct SaveUI
    {
        public TextMeshProUGUI defaultLabel;
        public GameObject savedGameObject;
        public TextMeshProUGUI Date;
        public UILevelScore.Star[] stars;
    }

    [SerializeField]
    private GameObject escLabel;
    [SerializeField]
    private GameObject bLabel;

    private bool corStarted = false;

    [SerializeField]
    private SaveUI[] saveUIs = new SaveUI[3];

    private void Awake()
    {
        for(int i = 0; i < 3; ++i)
        {
            if(SavesManager.Instance.HasSavedData(i))
            {
                saveUIs[i].savedGameObject.SetActive(true);
                saveUIs[i].defaultLabel.gameObject.SetActive(false);
                string date;
                SavesManager.SavedData savedData = SavesManager.Instance.GetSavedData(i, out date);
                saveUIs[i].Date.text = date;
                
                for(int j = 0; j < 3; ++j)
                {
                    if (j + 1 <= savedData.starsRecord)
                    {
                        saveUIs[i].stars[j].filled.gameObject.SetActive(true);
                        saveUIs[i].stars[j].empty.gameObject.SetActive(false);
                    }
                    else
                    {
                        saveUIs[i].stars[j].filled.gameObject.SetActive(false);
                        saveUIs[i].stars[j].empty.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Gamepad.current != null && (Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic) || Gamepad.current.leftStick.ReadValue().magnitude >= InputController.idleStickThreshold))
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

        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            StartCoroutine(OnBackPressed());
        }
        
    }

    public void OnPlayPressed(int index)
    {
        SavesManager.Instance.OnSlotSelected(index);
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
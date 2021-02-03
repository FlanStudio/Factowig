using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevelScore : MonoBehaviour
{
    #region REFERENCES
    [System.Serializable]
    private struct Star
    {
        [SerializeField]
        public Image empty;

        [SerializeField]
        public Image filled;
    }

    [System.Serializable]
    public struct ButtonUI
    {
        [SerializeField]
        private Image controller;

        [SerializeField]
        private Image keyboard;
    }

    [SerializeField]
    private TextMeshProUGUI deliveriesText = null;

    [SerializeField]
    private TextMeshProUGUI bonusText = null;

    [SerializeField]
    private TextMeshProUGUI failsText = null;

    [SerializeField]
    private TextMeshProUGUI totalText = null;

    [SerializeField]
    private Star[] stars = new Star[3];

    [SerializeField]
    private ButtonUI replayButton;

    [SerializeField]
    private ButtonUI backButton;
    #endregion

    private void OnEnable()
    {
        Time.timeScale = 0f;

        deliveriesText.text = GameManager.Instance.deliveredMoney.ToString("");
        bonusText.text = GameManager.Instance.bonusMoney.ToString("0.00");
        failsText.text = GameManager.Instance.failedMoney.ToString("");
        totalText.text = GameManager.Instance.currentMoney.ToString("0.00");

        for(int i = 0; i < 3; ++i)
        {
            if(i+1 <= GameManager.Instance.stars)
            {
                stars[i].empty.gameObject.SetActive(false);
                stars[i].filled.gameObject.SetActive(true);
            }
            else
            {
                stars[i].empty.gameObject.SetActive(true);
                stars[i].filled.gameObject.SetActive(false);
            }
        }
    }
}
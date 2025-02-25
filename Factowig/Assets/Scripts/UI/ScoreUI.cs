﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    [SerializeField]
    private Image[] stars = null;

    [SerializeField]
    private float[] percents = new float[3] {0.5f, 0.75f, 1f};

    [SerializeField]
    private Image progressBarImage = null;
    private RectTransform progressBar = null;

    [SerializeField]
    private TextMeshProUGUI scoreText = null;

    [SerializeField]
    private Sprite starGreen = null;

    public RectTransform mask = null;

    private void Awake()
    {
        Instance = this;
        progressBar = progressBarImage.GetComponent<RectTransform>();
    }

    public void RepositionProgressBar()
    {
        float percent = Mathf.Clamp(GameManager.Instance.currentMoney / GameManager.Instance.finalMoneyGoal, 0f, 1f);
        progressBar.anchoredPosition = new Vector2(0 - mask.rect.width * (1 - percent), progressBar.anchoredPosition.y);

        if (percent > percents[0])
        {
            stars[0].sprite = starGreen;
            GameManager.Instance.stars = 1;

            if(percent > percents[1])
            {
                stars[1].sprite = starGreen;
                GameManager.Instance.stars = 2;

                if(percent >= percents[2])
                {
                    stars[2].sprite = starGreen;
                    GameManager.Instance.stars = 3;
                }
            }
        }

        scoreText.text = GameManager.Instance.currentMoney.ToString("0.00");
    }
}

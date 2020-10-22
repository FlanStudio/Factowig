using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    [SerializeField]
    private Image[] stars;

    [SerializeField]
    private float[] percents = new float[3] {0.5f, 0.75f, 1f};

    [SerializeField]
    private Image progressBarImage;
    private RectTransform progressBar;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Sprite starGreen;

    private RectTransform thisTransform;

    private void Awake()
    {
        Instance = this;

        thisTransform = GetComponent<RectTransform>();
        progressBar = progressBarImage.GetComponent<RectTransform>();
    }

    public void RepositionProgressBar()
    {
        float percent = Mathf.Clamp(GameManager.Instance.currentMoney / GameManager.Instance.finalMoneyGoal, 0f, 1f);
        progressBar.anchoredPosition = new Vector2(0 - thisTransform.rect.width * (1 - percent), progressBar.anchoredPosition.y);

        if (percent > percents[0])
        {
            stars[0].sprite = starGreen;

            if(percent > percents[1])
            {
                stars[1].sprite = starGreen;

                if(percent >= percents[2])
                {
                    stars[2].sprite = starGreen;
                }
            }
        }

        scoreText.text = GameManager.Instance.currentMoney.ToString("0.00");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeRemainingUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private Image progressBar;
    private RectTransform progressBarTransform;

    [SerializeField]
    private RectTransform thisTransform;

    private float timer;

    private void Awake()
    {
        timer = GameManager.Instance.levelDurationSeconds;
        progressBarTransform = progressBar.GetComponent<RectTransform>();
    }

    private void Update()
    {
        int minutes = (int)(timer / 60f);
        int seconds = (int)timer - minutes * 60;

        timeText.text = minutes.ToString("00.") + ":" + seconds.ToString("00.");

        float percent = Mathf.Clamp(timer / GameManager.Instance.levelDurationSeconds, 0f, 1f);

        progressBarTransform.anchoredPosition = new Vector2(-thisTransform.rect.width * (1 - percent), progressBarTransform.anchoredPosition.y);

        timer -= Time.deltaTime;
        if (timer < 0f) timer = 0f;
    }
}
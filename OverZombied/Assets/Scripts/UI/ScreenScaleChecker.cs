using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenScaleChecker : MonoBehaviour
{
    private Vector2 lastResolution;
    private RectTransform rectTransform;

    private void Awake()
    {
        lastResolution = new Vector2(Screen.width, Screen.height);
        rectTransform = GetComponent<RectTransform>();   
    }

    private void Start()
    {
        CheckCanvasScale();
    }

    private void Update()
    {
        if (lastResolution.x != Screen.width || lastResolution.y != Screen.height)
        {
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;

            CheckCanvasScale();
        }
    }

    private void CheckCanvasScale()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);

        foreach(RecipeUI recipeUI in RecipeManager.Instance.recipeBoxes)
        {
            recipeUI.RepositionProgressBar();
        }
    }
}
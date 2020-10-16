using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenScaleChecker : MonoBehaviour
{
    private Vector2 lastResolution;
    private HorizontalLayoutGroup layoutGroup;

    private void Awake()
    {
        lastResolution = new Vector2(Screen.width, Screen.height);
        layoutGroup = GetComponent<HorizontalLayoutGroup>();   
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
        //bool[] lastStates = new bool[transform.childCount];
        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    Transform child = transform.GetChild(i);
        //    lastStates[i] = child.gameObject.activeSelf;
        //    child.gameObject.SetActive(true);

        //    for (int j = 0; j < child.childCount; ++j)
        //    {
        //        child.GetChild(j).gameObject.SetActive(true);
        //    }
        //}

        //layoutGroup.childControlWidth = true;
        //layoutGroup.childControlHeight = true;
        //layoutGroup.childForceExpandWidth = true;
        //layoutGroup.childForceExpandHeight = true;

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    Transform child = transform.GetChild(i);
        //    child.gameObject.SetActive(lastStates[i]);
        //}

        //layoutGroup.childControlWidth = false;
        //layoutGroup.childControlHeight = false;
        //layoutGroup.childForceExpandWidth = false;
        //layoutGroup.childForceExpandHeight = false;

        //for (int i = 0; i < transform.childCount; ++i)
        //{
        //    Transform child = transform.GetChild(i);
        //    RecipeUI recipe = child.GetComponent<RecipeUI>();
        //    recipe.EnableChildsOnRecipe();
        //}
    }
}
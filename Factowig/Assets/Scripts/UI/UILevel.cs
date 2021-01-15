using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UILevel : MonoBehaviour
{
    public int id = 0;

    [SerializeField]
    protected Selectable startSelected = null;

    protected CanvasGroup group = null;

    protected bool fadeEnded = true;

    protected void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    protected virtual void OnEnable()
    {
        if (startSelected)
            startSelected.Select();

        StartCoroutine(FadeIn());
    }

    protected IEnumerator FadeIn()
    {
        fadeEnded = false;

        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime * 1.5f;
            if (group.alpha > 1) group.alpha = 1;
            yield return null;
        }

        fadeEnded = true;
    }

    protected IEnumerator FadeOut()
    {
        fadeEnded = false;

        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * 1.5f;
            if (group.alpha < 0) group.alpha = 0;
            yield return null;
        }

        fadeEnded = true;
    }
}
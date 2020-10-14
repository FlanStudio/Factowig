using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigDispenser : MonoBehaviour
{
    public static WigDispenser Instance;

    [Tooltip("\"-1\" means an infinite amount")]
    public int amountItems = -1;

    [HideInInspector]
    public int itemsSpawned = 0;

    [SerializeField]
    private GameObject wigPrefab = null;

    [SerializeField]
    private Animator trapAnimator;

    [SerializeField]
    private Animator bustAnimator;

    public GameObject[] bustHeadHairs;
    public GameObject bustParent;

    private bool hairReady = true;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObject()
    {
        if (amountItems != -1 && itemsSpawned + 1 > amountItems)
            return null;

        if (!hairReady)
            return null;

        itemsSpawned++;

        GameObject obj = Instantiate(wigPrefab);
        obj.SetActive(false);

        foreach(GameObject gameObject in bustHeadHairs)
        {
            gameObject.SetActive(false);
        }

        hairReady = false;

        StartCoroutine(NewWigCoroutine());

        return obj;
    }

    private IEnumerator NewWigCoroutine()
    {
        trapAnimator.SetTrigger("Down");

        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("Up")) return true; else return false; });
        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return true; else return false; });
        yield return new WaitUntil(() => { if (amountItems != -1 && itemsSpawned + 1 > amountItems) return false; else return true; });

        bustAnimator.SetTrigger("Down");
        for (int i = 0; i < 2; ++i)
        {
            bustHeadHairs[i].SetActive(true);
        }
        bustParent.transform.localPosition = Vector3.zero;

        yield return new WaitUntil(() => { AnimatorStateInfo stateInfo = bustAnimator.GetCurrentAnimatorStateInfo(0); if ((stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime)) >= 0.95f) return true; else return false; });

        hairReady = true;
    }

    //TODO: SINCRONIZE ANIMATIONS, ETC
}

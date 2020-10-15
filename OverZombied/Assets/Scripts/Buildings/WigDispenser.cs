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

    private Rigidbody bustRb;

    private bool hairReady = true;

    private void Awake()
    {
        Instance = this;

        bustRb = bustParent.GetComponent<Rigidbody>();
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
        yield return new WaitUntil(() => { if (amountItems != -1 && itemsSpawned + 1 > amountItems) return false; else return true; });

        trapAnimator.SetTrigger("Down");

        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("opened") && bustRb.velocity == Vector3.zero) return true; else return false; });

        //bustAnimator.SetTrigger("Down");   
        //bustParent.transform.localPosition = Vector3.zero;
        //yield return new WaitUntil(() => { AnimatorStateInfo stateInfo = bustAnimator.GetCurrentAnimatorStateInfo(0); if ((stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime)) >= 0.95f) return true; else return false; });
        
        for (int i = 0; i < 2; ++i)
        {
            bustHeadHairs[i].SetActive(true);
        }

        trapAnimator.SetTrigger("Up");

        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) return true; else return false; });

        hairReady = true;
    }
}

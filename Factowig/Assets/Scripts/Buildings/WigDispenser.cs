using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigDispenser : MonoBehaviour
{
    public static WigDispenser Instance;

    public bool spawnInWaves = false;
    public int waveSize = 0;

    [Tooltip("\"-1\" means an infinite amount")]
    public int amountItems = -1;

    [HideInInspector]
    public int itemsSpawned = 0;

    [SerializeField]
    private GameObject wigPrefab = null;

    [SerializeField]
    private Animator trapAnimator = null;

    [SerializeField]
    private WigBust[] busts = null;

    private int emptyBusts = 0;

    private bool hairReady = true;

    private void Awake()
    {
        Instance = this;
        itemsSpawned = 3;
    }

    public GameObject GetObject()
    {
        if( (!spawnInWaves && amountItems != -1 && itemsSpawned + 1 > amountItems) /*|| (spawnInWaves && itemsSpawned + waveSize < amountItems)*/)
            return null;

        if (!hairReady || emptyBusts == waveSize)
            return null;

        GameObject obj = Instantiate(wigPrefab);
        obj.SetActive(false);

        foreach(WigBust bust in busts)
        {
            if (!bust.hairsHidden)
            {
                bust.HideHairs();
                break;
            }
        }

        emptyBusts++;

        if( (spawnInWaves && emptyBusts == waveSize) || (!spawnInWaves && amountItems != -1 && itemsSpawned + 1 <= amountItems) )
        {
            hairReady = false;
            StartCoroutine(NewWigCoroutine());
        }

        return obj;
    }

    private IEnumerator NewWigCoroutine()
    {
        yield return new WaitUntil(() => { if ( (!spawnInWaves && amountItems != -1 && itemsSpawned + 1 > amountItems) || (spawnInWaves && emptyBusts == waveSize && amountItems - itemsSpawned < waveSize)) return false; else return true; });

        trapAnimator.SetTrigger("Down");

        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("opened") && busts[0].rb.velocity == Vector3.zero) return true; else return false; });
        
        foreach(WigBust bust in busts)
        {
            bust.ShowHairs();
        }

        trapAnimator.SetTrigger("Up");

        yield return new WaitUntil(() => { if (trapAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) return true; else return false; });

        hairReady = true;
        itemsSpawned += waveSize;
        emptyBusts = 0;
    }
}
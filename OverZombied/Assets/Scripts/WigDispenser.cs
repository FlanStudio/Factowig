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

    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetObject()
    {
        itemsSpawned++;

        if (amountItems != -1 && itemsSpawned > amountItems)
            return null;

        GameObject obj = Instantiate(wigPrefab);
        obj.SetActive(false);

        return obj;
    }

    //TODO: SINCRONIZE ANIMATIONS, ETC
}

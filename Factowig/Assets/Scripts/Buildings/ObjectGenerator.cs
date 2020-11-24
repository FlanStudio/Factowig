using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [Tooltip("\"-1\" means an infinite amount")]
    public int amountItems = -1;

    private int itemsSpawned = 0;

    [SerializeField]
    private GameObject prefab = null;

    public Animator animator = null;

    public void Selected()
    {
        if(animator)    
            animator.SetBool("selected", true);       
    }

    public void DeSelected()
    {
        if(animator)
            animator.SetBool("selected", false);
    }

    public GameObject GetObject()
    {
        itemsSpawned++;

        if (amountItems != -1 && itemsSpawned > amountItems)
            return null;

        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);

        return obj;
    }
}
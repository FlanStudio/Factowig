using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WigBust : MonoBehaviour
{
    public Rigidbody rb { get; private set; }
    public HairReferencer hairReferencer { get; private set; }

    public bool hairsHidden { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hairReferencer = GetComponent<HairReferencer>();
    }

    public void HideHairs()
    {
        foreach (MeshRenderer hair in hairReferencer.hairs)
        {
            hair.gameObject.SetActive(false);
        }

        hairsHidden = true;
    }

    public void ShowHairs()
    {
        foreach (MeshRenderer hair in hairReferencer.hairs)
        {
            hair.gameObject.SetActive(true);
        }

        hairsHidden = false;
    }
}
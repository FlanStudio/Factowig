using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public IngredientData data;
    public Rigidbody rb;

    public bool throwable = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "PlaceableSurface")
        {
            PlaceableSurface placeableSurface = other.GetComponent<PlaceableSurface>();
            if(placeableSurface)
            {
                placeableSurface.PlacePickableObject(this);
            }
        }
    }
}

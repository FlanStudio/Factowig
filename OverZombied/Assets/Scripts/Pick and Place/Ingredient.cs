using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public IngredientData data;
    public Rigidbody rb;
    public new MeshRenderer renderer;

    public bool throwable = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if(LayerMask.LayerToName(other.gameObject.layer) == "PlaceableSurface")
        {
            PlaceableSurface placeableSurface = other.collider.GetComponentInParent<PlaceableSurface>();
            if(placeableSurface)
            {
                placeableSurface.PlacePickableObject(this);
            }
        }
    }
}

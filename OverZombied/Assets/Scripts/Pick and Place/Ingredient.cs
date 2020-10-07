using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public IngredientData data;
    public Rigidbody rb;
    public new MeshRenderer renderer;

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
                if(!placeableSurface.pickableObject && !rb.isKinematic)
                    placeableSurface.PlacePickableObject(this);
            }
        }
    }
}
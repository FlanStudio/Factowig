using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    public IngredientData data;
    public Rigidbody rb;
    public MeshRenderer renderer;
    public Collider collider;

    public HairReferencer hairReferencer = null;

    public Image icon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hairReferencer = GetComponent<HairReferencer>();
    }

    public bool HasValidNextStepWith(Ingredient other)
    {
        return other ? RecipeManager.Instance.AnyRecipeHasConsecutive(data, other.data) : false;
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
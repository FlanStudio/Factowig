using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlaceableSurface : MonoBehaviour
{
    public static float selectedColorMultiplier = 0.5f;

    public Ingredient pickableObject;
    public GameObject tableMesh;

    [HideInInspector]
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = tableMesh.GetComponent<MeshRenderer>();
    }

    public void Show()
    {
        foreach(Material material in meshRenderer.materials)        
            material.color *= selectedColorMultiplier;   
    }

    public void Hide()
    {
        foreach (Material material in meshRenderer.materials)
            material.color /= selectedColorMultiplier;
    }

    public void PlacePickableObject(Ingredient obj)
    {
        pickableObject = obj;
        pickableObject.gameObject.SetActive(true);

        pickableObject.transform.rotation = transform.rotation;
        pickableObject.transform.position = transform.position;
        pickableObject.transform.position += new Vector3(0f, meshRenderer.bounds.size.y + pickableObject.renderer.bounds.extents.y, 0f);

        pickableObject.transform.SetParent(null);

        if(obj.rb)
        {
            obj.rb.velocity = obj.rb.angularVelocity = Vector3.zero;
            obj.rb.isKinematic = true;
        }

        if(obj.collider)
        {
            obj.collider.enabled = false;
        }
    }
}
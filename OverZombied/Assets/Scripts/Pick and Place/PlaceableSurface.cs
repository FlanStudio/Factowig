using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlaceableSurface : MonoBehaviour
{
    public static float selectedColorMultiplier = 0.5f;

    public Ingredient pickableObject;
    public GameObject marble;
    public GameObject box;

    private MeshRenderer marbleRenderer;
    private MeshRenderer boxRenderer;

    private void Awake()
    {
        marbleRenderer = marble.GetComponent<MeshRenderer>();
        boxRenderer = box.GetComponent<MeshRenderer>();
    }

    public void Show()
    {
        marbleRenderer.material.color *= selectedColorMultiplier;
        boxRenderer.material.color *= selectedColorMultiplier;
    }

    public void Hide()
    {
        marbleRenderer.material.color /= selectedColorMultiplier;
        boxRenderer.material.color /= selectedColorMultiplier;
    }

    public void PlacePickableObject(Ingredient obj)
    {
        pickableObject = obj;
        pickableObject.gameObject.SetActive(true);

        pickableObject.transform.rotation = marble.transform.rotation;
        pickableObject.transform.position = marble.transform.position;
        pickableObject.transform.position += new Vector3(0f, pickableObject.renderer.bounds.extents.y + 0.04f, 0f);

        if(obj.rb)
        {
            obj.rb.velocity = obj.rb.angularVelocity = Vector3.zero;
            obj.rb.isKinematic = true;
        }
    }
}

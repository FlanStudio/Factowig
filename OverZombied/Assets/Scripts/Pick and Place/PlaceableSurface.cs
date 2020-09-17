using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSurface : MonoBehaviour
{
    public static float selectedColorMultiplier = 1.8f;

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

        pickableObject.transform.position = marble.transform.position;
        pickableObject.transform.position += new Vector3(0f, pickableObject.GetComponent<Renderer>().bounds.extents.y, 0f);
        pickableObject.transform.rotation = marble.transform.rotation;

        if(obj.rb)
        {
            obj.rb.velocity = obj.rb.angularVelocity = Vector3.zero;
        }
    }
}

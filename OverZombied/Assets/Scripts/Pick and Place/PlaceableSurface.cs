using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSurface : MonoBehaviour
{
    public Ingredient pickableObject;
    public MeshRenderer mesh;

    public void Show()
    {
        mesh.enabled = true;
    }

    public void Hide()
    {
        mesh.enabled = false;
    }

    public void PlacePickableObject(Ingredient obj)
    {
        pickableObject = obj;
        pickableObject.gameObject.SetActive(true);

        Transform child = transform.GetChild(0);
        pickableObject.transform.position = child.position;
        pickableObject.transform.position += new Vector3(0f, pickableObject.GetComponent<Renderer>().bounds.extents.y, 0f);
        pickableObject.transform.rotation = child.rotation;

        if(obj.rb)
        {
            obj.rb.velocity = obj.rb.angularVelocity = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSurface : MonoBehaviour
{
    public GameObject pickableObject;
    public MeshRenderer mesh;

    public void Show()
    {
        mesh.enabled = true;
    }

    public void Hide()
    {
        mesh.enabled = false;
    }

    public void PlacePickableObject(GameObject obj)
    {
        pickableObject = obj;
        pickableObject.SetActive(true);

        Transform child = transform.GetChild(0);
        pickableObject.transform.position = child.position;
        pickableObject.transform.position += new Vector3(0f, pickableObject.GetComponent<Renderer>().bounds.extents.y, 0f);
        pickableObject.transform.rotation = child.rotation;
    }
}

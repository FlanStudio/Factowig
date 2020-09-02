using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableSurfaceSelector : MonoBehaviour
{
    public bool drawGizmos = false;
    public float interactRadius = 5f;

    private float height = 1.5f;

    public PlaceableSurface selectedSurface { get; private set; }

    public void SelectPlaceableSurfaces(float height)
    {
        this.height = height;

        if(selectedSurface != null)
            selectedSurface.Hide();
        
        RaycastHit hitInfo;
        if (Physics.Raycast(new Vector3(transform.position.x, height, transform.position.z), transform.forward, out hitInfo, interactRadius, 1 << LayerMask.NameToLayer("PlaceableSurface")))
        {
            selectedSurface = hitInfo.collider.gameObject.GetComponent<PlaceableSurface>();
            selectedSurface.Show();
        }
        else
            selectedSurface = null;
    }

    private void OnDrawGizmos()
    {
        if(drawGizmos)
            Gizmos.DrawLine(new Vector3(transform.position.x, height, transform.position.z), new Vector3(transform.position.x, height, transform.position.z) + transform.forward * interactRadius);
    }
}

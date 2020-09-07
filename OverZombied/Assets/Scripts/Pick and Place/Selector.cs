using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public bool drawGizmos = false;
    public float interactRadius = 5f;
    public float height = 1.5f;

    public PlaceableSurface selectedSurface = null;
    public ObjectGenerator selectedGenerator = null;
    public GameObject groundObject = null;

    public void Select()
    {
        if (selectedSurface != null)
            selectedSurface.Hide();

        RaycastHit hitInfo;

        int layers = 1 << LayerMask.NameToLayer("ObjectGenerator") | 1 << LayerMask.NameToLayer("PlaceableSurface");

        if (Physics.Raycast(new Vector3(transform.position.x, height, transform.position.z), transform.forward, out hitInfo, interactRadius, layers))
        {
            groundObject = null;
            selectedGenerator = hitInfo.collider.gameObject.GetComponent<ObjectGenerator>();
            selectedSurface = hitInfo.collider.gameObject.GetComponent<PlaceableSurface>();
            if (selectedSurface)
                selectedSurface.Show();
        }
        else
        {
            selectedGenerator = null;
            selectedSurface = null;

            //Check for grounded objects
            RaycastHit hitInfoGrounded;

            if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z), transform.forward, out hitInfoGrounded, interactRadius, 1 << LayerMask.NameToLayer("PickableObject")))
            {
                groundObject = hitInfoGrounded.collider.gameObject;
            }
            else
            {
                groundObject = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
            Gizmos.DrawLine(new Vector3(transform.position.x, height, transform.position.z), new Vector3(transform.position.x, height, transform.position.z) + transform.forward * interactRadius);
    }
}

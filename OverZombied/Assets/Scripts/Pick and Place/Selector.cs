using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public bool drawGizmos = false;
    public float interactRadius = 5f;
    public float height = 0.01f;
    public LayerMask layers;

    [Header("Selected stuff")]
    public PlaceableSurface selectedSurface = null;
    public ObjectGenerator selectedGenerator = null;
    public ClientBehavior selectedClient = null;
    public Ingredient groundObject = null;
    public RecipeDeliverer selectedDeliverer = null;

    public void Select()
    {
        if (selectedSurface != null)
            selectedSurface.Hide();

        if (selectedClient != null)
            selectedClient.UnSelectMeshes();

        selectedGenerator = null;
        selectedSurface = null;
        groundObject = null;
        selectedClient = null;
        selectedDeliverer = null;

        RaycastHit hitInfo;

        if (Physics.Raycast(new Vector3(transform.position.x, height, transform.position.z), transform.forward, out hitInfo, interactRadius, layers))
        {
            groundObject = hitInfo.collider.gameObject.GetComponent<Ingredient>();
            if(!groundObject)
            {
                selectedClient = hitInfo.collider.gameObject.GetComponent<ClientBehavior>();
                
                if(selectedClient)
                {
                    selectedClient.SelectMeshes();
                }
                else
                {
                    selectedGenerator = hitInfo.collider.gameObject.GetComponent<ObjectGenerator>();

                    if(!selectedGenerator)
                    {
                        selectedDeliverer = hitInfo.collider.GetComponentInParent<RecipeDeliverer>();

                        if(!selectedDeliverer)
                        {
                            selectedSurface = hitInfo.collider.GetComponentInParent<PlaceableSurface>();
                            if (selectedSurface)
                                selectedSurface.Show();
                        }                    
                    }
                }        
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
            Gizmos.DrawLine(new Vector3(transform.position.x, height, transform.position.z), new Vector3(transform.position.x, height, transform.position.z) + transform.forward * interactRadius);
    }
}

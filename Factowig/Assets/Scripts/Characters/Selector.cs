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
    public PlaceableSurface     selectedSurface        =   null;
    public ObjectGenerator      selectedGenerator      =   null;
    public Chair                selectedChair          =   null;
    public Ingredient           groundObject           =   null;
    public RecipeDeliverer      selectedDeliverer      =   null;
    public WigDispenser         selectedWigDispenser   =   null;

    public void Select()
    {
        if (selectedSurface != null)
            selectedSurface.Hide();

        if (selectedChair != null)
            selectedChair.UnSelectMeshes();

        if (selectedGenerator != null)
            selectedGenerator.DeSelected();

        selectedGenerator = null;
        selectedSurface = null;
        groundObject = null;
        selectedChair = null;
        selectedDeliverer = null;

        RaycastHit hitInfo;

        if (Physics.Raycast(new Vector3(transform.position.x, height, transform.position.z), transform.forward, out hitInfo, interactRadius, layers))
        {
            groundObject = hitInfo.collider.gameObject.GetComponent<Ingredient>();
            if(!groundObject)
            {
                selectedChair = hitInfo.collider.gameObject.GetComponent<Chair>();
                
                if(selectedChair)
                {
                    selectedChair.SelectMeshes();
                }
                else
                {
                    selectedWigDispenser = hitInfo.collider.gameObject.GetComponent<WigDispenser>();

                    if(!selectedWigDispenser)
                    {
                        selectedGenerator = hitInfo.collider.gameObject.GetComponent<ObjectGenerator>();
                        if (selectedGenerator)
                            selectedGenerator.Selected();
                        else
                        {
                            selectedDeliverer = hitInfo.collider.GetComponentInParent<RecipeDeliverer>();

                            if (!selectedDeliverer)
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
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
            Gizmos.DrawLine(new Vector3(transform.position.x, height, transform.position.z), new Vector3(transform.position.x, height, transform.position.z) + transform.forward * interactRadius);
    }
}

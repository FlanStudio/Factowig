using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public Animator animator;

    public MeshRenderer normalHair;
    public MeshRenderer cutHair;
    public MeshRenderer combedHair;
    public MeshRenderer cutcombedHair;

    public MeshRenderer[] chairMeshes;

    private Ingredient wig;

    public void PlaceWig(Ingredient wig)
    {
        switch(wig.data.name)
        {
            case "Normal Hair":
                normalHair.gameObject.SetActive(true);
                break;
            case "Cut Hair":
                cutHair.gameObject.SetActive(true);
                break;
        }

        animator.SetBool("Wig", true);
        this.wig = wig;
    }

    public Ingredient RemoveWig()
    {
        normalHair.gameObject.SetActive(false);
        cutHair.gameObject.SetActive(false);
        combedHair.gameObject.SetActive(false);
        cutcombedHair.gameObject.SetActive(false);
        
        animator.SetBool("Wig", false);

        Ingredient ret = wig;
        wig = null;
        return ret;
    }

    public void SelectMeshes()
    {
        normalHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        cutHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        combedHair.material.color *= PlaceableSurface.selectedColorMultiplier;
        cutcombedHair.material.color *= PlaceableSurface.selectedColorMultiplier;   

        foreach(MeshRenderer renderer in chairMeshes)
        {
            renderer.material.color *= PlaceableSurface.selectedColorMultiplier;
        }
    }

    public void UnSelectMeshes()
    {
        normalHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        cutHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        combedHair.material.color /= PlaceableSurface.selectedColorMultiplier;
        cutcombedHair.material.color /= PlaceableSurface.selectedColorMultiplier;

        foreach (MeshRenderer renderer in chairMeshes)
        {
            renderer.material.color /= PlaceableSurface.selectedColorMultiplier;
        }
    }
}

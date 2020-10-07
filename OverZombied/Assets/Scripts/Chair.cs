using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public Animator animator;

    public MeshRenderer[] hairMeshes;
    public MeshRenderer[] chairMeshes;

    private Ingredient wig;

    public bool PlaceWig(Ingredient wig)
    {
        if (!RecipeManager.Instance.HasMoreSteps(wig))
            return false;

        hairMeshes[wig.data.wigIndex].gameObject.SetActive(true);
      
        animator.SetBool("Wig", true);
        this.wig = wig;

        return true;
    }

    public Ingredient RemoveWig()
    {
        foreach (MeshRenderer renderer in hairMeshes)
            renderer.gameObject.SetActive(false);
        
        animator.SetBool("Wig", false);

        Ingredient ret = wig;
        wig = null;
        return ret;
    }

    public void SelectMeshes()
    {  
        foreach (MeshRenderer meshRenderer in hairMeshes)          
            meshRenderer.material.color *= PlaceableSurface.selectedColorMultiplier;

        foreach (MeshRenderer renderer in chairMeshes)      
            renderer.material.color *= PlaceableSurface.selectedColorMultiplier;
    }

    public void UnSelectMeshes()
    {
        foreach(MeshRenderer meshRenderer in hairMeshes)          
            meshRenderer.material.color /= PlaceableSurface.selectedColorMultiplier;          

        foreach (MeshRenderer renderer in chairMeshes)
            renderer.material.color /= PlaceableSurface.selectedColorMultiplier;
    }
}

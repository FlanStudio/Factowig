using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    public Animator animator;

    public MeshRenderer[] hairMeshes;
    public MeshRenderer[] chairMeshes;

    private Ingredient wig;
    private Ingredient ingredient;

    private bool actionStarted = false;
    private float actionCounter = 0f;
    private bool actionFinished = false;

    [SerializeField]
    private Canvas canvas;
    
    [SerializeField]
    private Image iconIngredient;

    [SerializeField]
    private RectTransform progressBar;

    public bool PlaceWig(Ingredient wig)
    {
        if (!RecipeManager.Instance.HasMoreSteps(wig.data))
            return false;

        hairMeshes[wig.data.wigIndex].gameObject.SetActive(true);
      
        animator.SetBool("Wig", true);
        this.wig = wig;

        return true;
    }

    public Ingredient RemoveWig()
    {
        if (actionStarted)
            return null;

        actionFinished = false;

        foreach (MeshRenderer renderer in hairMeshes)
            renderer.gameObject.SetActive(false);
        
        animator.SetBool("Wig", false);

        canvas.gameObject.SetActive(false);

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

    public bool ApplyIngredient(Ingredient ingredient)
    {
        if (!wig || actionFinished || (!ingredient && !actionStarted) || (ingredient && !wig.HasValidNextStepWith(ingredient)))
            return false;
     
        if(!actionStarted)
        {
            actionStarted = true;
            iconIngredient.sprite = ingredient.data.sprite;
            canvas.gameObject.SetActive(true);
            this.ingredient = ingredient;

            return true;
        }

        actionCounter += Time.deltaTime;
        progressBar.anchoredPosition = new Vector2((1 - (actionCounter / this.ingredient.data.actionPressSeconds)) * -1.3f, progressBar.anchoredPosition.y);

        if(actionCounter >= this.ingredient.data.actionPressSeconds)
        {
            actionStarted = false;
            actionCounter = 0f;
            actionFinished = true;

            iconIngredient.sprite = RecipeManager.Instance.tickSprite;

            IngredientData newIngredientData = RecipeManager.Instance.GetResultingIngredient(wig.data, this.ingredient.data);
            
            hairMeshes[wig.data.wigIndex].gameObject.SetActive(false);
            wig.data = newIngredientData;
            hairMeshes[wig.data.wigIndex].gameObject.SetActive(true);

            Destroy(this.ingredient.gameObject);
        }

        return false;
    }
}

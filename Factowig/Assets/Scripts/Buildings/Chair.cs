using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour
{
    public GameObject head;
    public MeshRenderer[] hairMeshes;
    public MeshRenderer[] chairMeshes;

    private Ingredient wig;
    private Ingredient ingredient;

    private bool actionStarted = false;
    private float actionCounter = 0f;

    [SerializeField]
    private Canvas canvas = null;
    
    [SerializeField]
    private RectTransform progressBar = null;
    [SerializeField]
    private RectTransform mask = null;
    [SerializeField]
    private Image recipeBar = null;
    
    public bool PlaceWig(Ingredient wig)
    {
        if (!RecipeManager.Instance.HasMoreSteps(wig.data))
            return false;

        head.SetActive(true);
        hairMeshes[wig.data.wigIndex].gameObject.SetActive(true);
      
        this.wig = wig;

        return true;
    }

    public Ingredient RemoveWig()
    {
        if (actionStarted)
            return null;

        foreach (MeshRenderer renderer in hairMeshes)
            renderer.gameObject.SetActive(false);

        head.SetActive(false);
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

    public bool ApplyIngredient(Ingredient ingredient, out bool stayHere)
    {
        if (!wig || (!ingredient && !actionStarted) || (ingredient && !wig.HasValidNextStepWith(ingredient)))
        {
            stayHere = false;
            return false;
        }
     
        if(!actionStarted)
        {
            actionStarted = true;
            canvas.gameObject.SetActive(true);
            this.ingredient = ingredient;

            recipeBar.sprite = ingredient.data.progressionBar;

            ingredient.icon.gameObject.SetActive(false);
            ingredient.gameObject.SetActive(true);

            stayHere = true;

            return true;
        }

        actionCounter += Time.deltaTime;

        float percent = Mathf.Clamp(actionCounter / this.ingredient.data.actionPressSeconds, 0f, 1f);
        progressBar.anchoredPosition = new Vector2(-mask.rect.width * (1 - percent), progressBar.anchoredPosition.y);

        if (actionCounter >= this.ingredient.data.actionPressSeconds)
        {
            actionStarted = false;
            actionCounter = 0f;

            IngredientData newIngredientData = RecipeManager.Instance.GetResultingIngredient(wig.data, this.ingredient.data);
            HairReferencer refer = wig.GetComponent<HairReferencer>();

            hairMeshes[wig.data.wigIndex].gameObject.SetActive(false);
            refer.hairs[wig.data.wigIndex].gameObject.SetActive(false);
            wig.data = newIngredientData;
            hairMeshes[wig.data.wigIndex].gameObject.SetActive(true);
            refer.hairs[wig.data.wigIndex].gameObject.SetActive(true);

            Destroy(this.ingredient.gameObject);
            this.ingredient = null;
        }

        stayHere = true;

        return false;
    }
}
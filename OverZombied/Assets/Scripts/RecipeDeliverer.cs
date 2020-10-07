using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDeliverer : MonoBehaviour
{
    public bool Deliver(Ingredient ingredient)
    {
        if (!RecipeManager.Instance.IsFinalIngredient(ingredient))
            return false;

        int index;
        RecipeUI recipeUI = RecipeManager.Instance.GetRecipeUI(ingredient, out index);
        if(!recipeUI)
        {
            //Delivery failed
        }
        else if(index == 0)
        {
            //More points?
            RecipeManager.Instance.RecipeDelivered(recipeUI);
        }
        else
        {
            //Normal points
        }

        Destroy(ingredient.gameObject);

        return true;
    }
}

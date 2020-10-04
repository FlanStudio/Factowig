using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDeliverer : MonoBehaviour
{
    public bool Deliver(Ingredient ingredient)
    {
        RecipeManager.Instance.GetRecipeUI(RecipeManager.Instance.availableRecipes[0]);


        //Destroy(ingredient.gameObject);

        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "FlanStudio/Recipe", order = 2)]
public class Recipe : ScriptableObject
{
    public enum RecipeType { CUT_COMB, CUT, COMB }

    public RecipeType type = RecipeType.CUT_COMB;
    public List<IngredientData> ingredients;
    public float timeLimit = 20f;
    public float moneyInflow = 20f;
    public float moneyPenalty = 10f;

    public IngredientData finalHairState;

    //public bool SameRecipeAs(Recipe recipe)
    //{
    //    if (ingredients.Count == recipe.ingredients.Count)
    //    {
    //        List<IngredientData> myIngredients = new List<IngredientData>();
    //        foreach(IngredientData ingredient in ingredients)
    //        {
    //            myIngredients.Add(ingredient);
    //        }

    //        List<IngredientData> otherIngredients = new List<IngredientData>();
    //        foreach (IngredientData ingredient in recipe.ingredients)
    //        {
    //            otherIngredients.Add(ingredient);
    //        }

    //        while(myIngredients.Count > 0 && otherIngredients.Count > 0)
    //        {
    //            bool found = false;
    //            foreach(IngredientData ingredient in myIngredients)
    //            {
    //                if(ingredient == otherIngredients[0])
    //                {
    //                    found = true;
    //                    myIngredients.Remove(ingredient);
    //                    otherIngredients.RemoveAt(0);
    //                    continue;
    //                }
    //            }

    //            if (found == false)
    //                return false;
    //        }

    //        return true;

    //    }
    //    else
    //        return false;
    //}
}

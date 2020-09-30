using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    private Recipe recipe;

    private Image scissors;
    private Image comb;

    [HideInInspector]
    public bool activated = false;

    public void SetRecipe(Recipe recipe)
    {
        if(!scissors || !comb)
        {
            scissors = transform.GetChild(0).GetComponent<Image>();
            comb = transform.GetChild(1).GetComponent<Image>();
        }

        scissors.gameObject.SetActive(false);
        comb.gameObject.SetActive(false);

        this.recipe = recipe;

        foreach(IngredientData ingredient in recipe.ingredients)
        {
            switch(ingredient.name)
            {
                case "Scissors":
                    scissors.gameObject.SetActive(true);
                    break;
                case "Comb":
                    comb.gameObject.SetActive(true);
                    break;
            }
        }

        gameObject.SetActive(true);
    }
}

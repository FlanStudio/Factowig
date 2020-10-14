using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [HideInInspector]
    public Recipe recipe { get; private set; }

    private Image scissors;
    private Image comb;

    [HideInInspector]
    public bool activated = false;

    public float counter { get; private set; } = 0f;

    private void Update()
    {
        if (recipe)
            counter += Time.deltaTime;
        else
            counter = 0f;
    }

    public void EnableChildsOnRecipe()
    {
        if(recipe != null)
        {
            scissors.gameObject.SetActive(false);
            comb.gameObject.SetActive(false);

            foreach (IngredientData ingredient in recipe.ingredients)
            {
                switch (ingredient.name)
                {
                    case "Scissors":
                        scissors.gameObject.SetActive(true);
                        break;
                    case "Comb":
                        comb.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;

        counter = 0f;

        if (!scissors || !comb)
        {
            scissors = transform.GetChild(0).GetComponent<Image>();
            comb = transform.GetChild(1).GetComponent<Image>();
        }

        EnableChildsOnRecipe();

        gameObject.SetActive(true);
    }
}

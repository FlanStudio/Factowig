using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [HideInInspector]
    public Recipe recipe { get; private set; }

    public Image scissors;
    public Image comb;

    public RectTransform thisTransform;
    public RectTransform progressBar;
    private Image progressBarImage;

    [HideInInspector]
    public bool activated = false;

    public float counter { get; private set; } = 0f;

    private void Update()
    {
        if (recipe)
        {
            counter += Time.deltaTime;
            RepositionProgressBar();
        }
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

    public void RepositionProgressBar()
    {
        if(!progressBarImage)
            progressBarImage = progressBar.GetComponent<Image>();

        float percent = recipe ? Mathf.Clamp(counter / recipe.timeLimit, 0, 1) : 0;
        if(percent < 0.5)
        {
            progressBarImage.color = Color.green;
        }
        else if(percent >= 0.5)
        {
            if (percent > 0.75)         
                progressBarImage.color = Color.red;       
            else
                progressBarImage.color = Color.yellow;
        }

        progressBar.anchoredPosition = new Vector2(0 - thisTransform.rect.width * (percent), progressBar.anchoredPosition.y);
    }
}

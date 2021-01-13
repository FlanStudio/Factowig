using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField]
    public List<Recipe> availableRecipes = null;

    [SerializeField]
    public List<Recipe> totalRecipes = null;

    [SerializeField]
    public List<RecipeUI> recipeBoxes = null;

    private int activeBoxes = 0;
    private bool spawnNewRecipes = true;

    [Header("Game Config")]
    [SerializeField]
    private AnimationCurve activateCurve = null;
    public float recipeRespawnTime = 5f;

    private bool firstLoop = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float value = activateCurve.Evaluate(Time.time / GameManager.Instance.levelDurationSeconds);

        while(spawnNewRecipes && activeBoxes < (int)value)
        {
            foreach(RecipeUI recipeBox in recipeBoxes)
            {
                if(!recipeBox.activated)
                {
                    recipeBox.activated = true;
                    recipeBox.gameObject.SetActive(true);
                    recipeBox.SetRecipe(availableRecipes[Random.Range(0, availableRecipes.Count)]);
                    activeBoxes++;

                    if(firstLoop)
                    {
                        firstLoop = false;
                        AudioManager.Instance.PlaySoundEffect(AudioManager.FX.NEWRECIPE);
                    }

                    break;
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(recipeBoxes[0].transform.parent.GetComponent<RectTransform>());
        }
    }

    public void RecipeDelivered(RecipeUI recipeUI)
    {
        StartCoroutine(ReEnableRecipe(recipeUI, recipeRespawnTime));
    }

    public RecipeUI GetRecipeUI(Ingredient ingredient, out int index)
    {
        for(int i = 0; i < recipeBoxes.Count; ++i)
        {
            RecipeUI recipeUI = recipeBoxes[i];
            if (!recipeUI.gameObject.activeSelf)
                break;
            
            if(recipeUI.recipe.ingredients[recipeUI.recipe.ingredients.Count - 1] == ingredient.data)
            {
                index = i;
                return recipeUI;
            }
        }

        index = -1;
        return null;
    }

    public bool HasMoreSteps(IngredientData ingredient)
    {
        foreach (Recipe recipe in totalRecipes)
        {
            for(int i = 0; i < recipe.ingredients.Count; ++i)
            {
                if(ingredient == recipe.ingredients[i] && i+1 < recipe.ingredients.Count)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool AnyRecipeHasConsecutive(IngredientData a, IngredientData b)
    {
        foreach (Recipe recipe in totalRecipes)
        {
            for (int i = 0; i < recipe.ingredients.Count - 1; ++i)
            {
                if (recipe.ingredients[i] == a && recipe.ingredients[i + 1] == b)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsFinalIngredient(IngredientData ingredient)
    {
        return ingredient.isWig;

        //bool valid = false;

        //foreach (Recipe recipe in availableRecipes)
        //{
        //    if (recipe.ingredients[recipe.ingredients.Count - 1] == ingredient)
        //    {
        //        valid = true;
        //        break;
        //    }
        //}

        //return valid;
    }

    public IngredientData GetResultingIngredient(IngredientData a, IngredientData b)
    {
        foreach(Recipe recipe in totalRecipes)
        {
            for(int i = 0; i < recipe.ingredients.Count - 2; ++i)
            {
                if (recipe.ingredients[i] == a && recipe.ingredients[i + 1] == b)
                    return recipe.ingredients[i + 2];
            }
        }

        return null;
    }

    private IEnumerator ReEnableRecipe(RecipeUI recipeUI, float delay)
    {
        recipeUI.gameObject.SetActive(false);

        float time = Time.time;

        yield return new WaitUntil(() => { if (spawnNewRecipes && Time.time >= time + delay) return true; else return false;  });

        for (int i = 0; i < recipeUI.transform.parent.childCount; ++i)
        {
            Transform child = recipeUI.transform.parent.GetChild(i);
            if (recipeUI.transform != child && !child.gameObject.activeSelf)
            {
                recipeUI.transform.SetSiblingIndex(i-1);
                break;
            }
            else if(i == recipeUI.transform.parent.childCount - 1)
            {
                recipeUI.transform.SetAsLastSibling();
                break;
            }
        }

        recipeUI.SetRecipe(availableRecipes[Random.Range(0, availableRecipes.Count)]);
        recipeUI.gameObject.SetActive(true);

        AudioManager.Instance.PlaySoundEffect(AudioManager.FX.NEWRECIPE);
    }
}
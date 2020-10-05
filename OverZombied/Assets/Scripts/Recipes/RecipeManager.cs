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
    private List<RecipeUI> recipeBoxes = null;

    private int activeBoxes = 0;
    private bool spawnNewRecipes = true;

    [Header("Game Config")]
    [SerializeField]
    private AnimationCurve activateCurve = null;
    public float recipeRespawnTime = 5f;
    public float levelDurationSeconds = 120f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float value = activateCurve.Evaluate(Time.time / levelDurationSeconds);

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
                    break;
                }
            }
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
            
            if(recipeUI.recipe.finalHairState == ingredient.data)
            {
                index = i;
                return recipeUI;
            }
        }

        index = -1;
        return null;
    }

    public bool IsValidRecipe(Ingredient ingredient)
    {
        bool valid = false;

        foreach (Recipe recipe in availableRecipes)
        {
            if (recipe.finalHairState == ingredient.data)
            {
                valid = true;
                break;
            }
        }

        return valid;
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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField]
    private List<Recipe> availableRecipes;

    [SerializeField]
    private List<RecipeUI> recipeBoxes;

    [Header("Game Config")]
    public int startRecipes = 2;
    public float recipeRespawnTime = 5f;

    private void Awake()
    {
        for(int i = 0; i < startRecipes; ++i)
        {
            recipeBoxes[i].SetRecipe(availableRecipes[Random.Range(0, availableRecipes.Count)]);
        }
    }
}

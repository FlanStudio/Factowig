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

    private int activeBoxes = 0;

    [Header("Game Config")]
    [SerializeField]
    private AnimationCurve activateCurve;
    public float recipeRespawnTime = 5f;
    public float levelDurationSeconds = 120f;

    private void Update()
    {
        float value = activateCurve.Evaluate(Time.time / levelDurationSeconds);

        while(activeBoxes < (int)value)
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
}
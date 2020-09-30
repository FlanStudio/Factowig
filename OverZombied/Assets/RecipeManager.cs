using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance;

    [SerializeField]
    private List<Recipe> availableRecipes = null;

    [SerializeField]
    private List<RecipeUI> recipeBoxes = null;

    private int activeBoxes = 0;
    private bool spawnNewRecipes = true;

    [Header("Game Config")]
    [SerializeField]
    private AnimationCurve activateCurve = null;
    public float recipeRespawnTime = 5f;
    public float levelDurationSeconds = 120f;

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
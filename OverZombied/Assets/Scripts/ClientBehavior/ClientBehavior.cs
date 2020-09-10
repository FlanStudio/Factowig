using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
    public Recipe recipe = null;
    public int nextIngredient = 0;

    //Only used for tool actions
    public float usefulCounter = 0f;

    public float angryTime = 0f;

    public float minTimeToRespawn = 3f;
    [Tooltip("Not inclusive")]
    public float maxTimeToRespawn = 9f;

    private void Awake()
    {
        NewRecipe();
    }

    private void Update()
    {
        if(recipe != null)
        {
            angryTime += Time.deltaTime;
            if(angryTime >= recipe.timeLimit)
            {
                RecipeFailed();
            }
        }
    }

    public void NewRecipe()
    {
        if (ClientManager.Instance == null)
            return;

        int rand = UnityEngine.Random.Range(0, ClientManager.Instance.availableRecipes.Count);
        recipe = ClientManager.Instance.availableRecipes[rand];

        nextIngredient = 0;
        angryTime = 0;

        Debug.Log("I want " + recipe.name);
    }

    private void RecipeCompleted()
    {
        Debug.Log("Now im going to the space, i have a wonderful hair");
        ClientManager.Instance.currentMoney += recipe.moneyInflow;
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
        gameObject.SetActive(false);
    }

    private void RecipeFailed()
    {
        Debug.Log("This hair saloon is shit. Im going away.");
        ClientManager.Instance.currentMoney -= recipe.moneyPenalty;
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
        gameObject.SetActive(false);
    }

    public void GiveIngredient(Ingredient ingredient)
    {
        if(nextIngredient < recipe.ingredients.Count)
        {
            if(ingredient.data == recipe.ingredients[nextIngredient])
            {
                nextIngredient++;

                if (nextIngredient < recipe.ingredients.Count)
                    Debug.Log("Correct, now i want " + recipe.ingredients[nextIngredient]);
                else
                {
                    RecipeCompleted();
                }
            }
            else
            {
                RecipeFailed();  
            }
        }
    }

    public void UseTool(Ingredient tool)
    {
        if (nextIngredient < recipe.ingredients.Count)
        {
            if (tool.data == recipe.ingredients[nextIngredient])
            {
                usefulCounter += Time.deltaTime;
                if (usefulCounter > tool.data.actionPressSeconds)
                {
                    usefulCounter = 0f;
                    nextIngredient++;

                    Debug.Log("Action completed");

                    if (nextIngredient < recipe.ingredients.Count)
                    {
                        Debug.Log("I still want " + recipe.ingredients[nextIngredient].name);
                    }
                    else
                    {
                        RecipeCompleted();
                    }
                }
            }
            else
            {
                RecipeFailed();
            }
        }    
    }
}

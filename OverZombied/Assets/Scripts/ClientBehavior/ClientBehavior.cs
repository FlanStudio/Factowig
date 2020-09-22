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

    private PickUpController playerStarted;

    public Canvas canvas;
    public RectTransform foregroundProgressBar;

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

        Debug.Log("I want " + recipe.name + ". " + gameObject.name);

        canvas.gameObject.SetActive(true);
        foregroundProgressBar.anchoredPosition = new Vector2(1.3f, 0f);
    }

    private void RecipeCompleted()
    {
        Debug.Log("Now im going to the space, i have a wonderful hair. " + gameObject.name);
        ClientManager.Instance.currentMoney += recipe.moneyInflow;
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
        recipe = null;
        playerStarted = null;
        canvas.gameObject.SetActive(false);
    }

    private void RecipeFailed()
    {
        Debug.Log("This hair saloon is shit. Im going away. " + gameObject.name);
        ClientManager.Instance.currentMoney -= recipe.moneyPenalty;
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
        recipe = null;
        playerStarted = null;
        canvas.gameObject.SetActive(false);
    }

    public void GiveIngredient(Ingredient ingredient)
    {
        if(recipe != null && nextIngredient < recipe.ingredients.Count)
        {
            if(ingredient.data == recipe.ingredients[nextIngredient])
            {
                nextIngredient++;

                if (nextIngredient < recipe.ingredients.Count)
                    Debug.Log("I still want " + recipe.ingredients[nextIngredient].name + ". " + gameObject.name);
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

    public void UseToolStarted(PickUpController player, Ingredient tool)
    {
        if (recipe == null)
            return;

        if(tool.data == recipe.ingredients[nextIngredient])
        {
            playerStarted = player;
            player.transform.position = transform.position + transform.forward * player.dropDistance;
            player.transform.LookAt(transform.position);
            player.movementController.move = false;
            player.movementController.rotate = false;
        }
        else
        {
            RecipeFailed();
        }    
    }

    public void UseToolFinished()
    {
        if (playerStarted)
        {
            playerStarted.movementController.move = true;
            playerStarted.movementController.rotate = true;
            playerStarted = null;
        }      
    }

    public void UseTool(PickUpController player, Ingredient tool)
    {
        if (playerStarted != null && playerStarted == player && recipe != null && nextIngredient < recipe.ingredients.Count)
        {
            if (tool.data == recipe.ingredients[nextIngredient])
            {
                usefulCounter += Time.deltaTime;

                foregroundProgressBar.anchoredPosition = new Vector2( 1.3f * (1 - usefulCounter/tool.data.actionPressSeconds), 0f);

                if (usefulCounter > tool.data.actionPressSeconds)
                {
                    usefulCounter = 0f;
                    nextIngredient++;

                    Debug.Log("Action completed");

                    if (nextIngredient < recipe.ingredients.Count)
                    {
                        Debug.Log("I still want " + recipe.ingredients[nextIngredient].name + ". " + gameObject.name);
                        foregroundProgressBar.anchoredPosition = new Vector2(1.3f, 0f);
                        UseToolFinished();
                    }
                    else
                    {
                        RecipeCompleted();
                    }
                }
            }
            else
            {
                //Now it should never enter here
                RecipeFailed();
            }
        }    
    }
}

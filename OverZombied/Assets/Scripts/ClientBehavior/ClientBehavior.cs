using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClientBehavior : MonoBehaviour
{
    private Recipe recipe = null;
    private int nextIngredient = 0;

    //Only used for tool actions
    private float currentRecipeTime = 0f;
    private float totalRecipeTime = 0f;
    private float currentIngredientGoal = 0f;

    private float angryTime = 0f;

    public float minTimeToRespawn = 3f;
    [Tooltip("Not inclusive")]
    public float maxTimeToRespawn = 9f;

    private PickUpController playerStarted;

    public Animator animator;

    public Canvas canvas;
    public RectTransform progressBar;
    public List<Image> ingredientBars;

    [SerializeField]
    private List<MeshRenderer> clientMeshes = null;

    private bool startCounting = false;

    [HideInInspector]
    public bool activated = false;

    private void Update()
    {
        if(recipe != null && startCounting)
        {
            angryTime += Time.deltaTime;
            if(angryTime >= recipe.timeLimit)
            {
                StartCoroutine(RecipeFailed());
            }
        }
    }

    private void SetUpHairs()
    {
        for (int i = 0; i < 4; ++i)
        {
                clientMeshes[i].gameObject.SetActive(false);
        }

        switch (recipe.type)
        {
            case Recipe.RecipeType.CUT_COMB:
                {
                    int active = -1;

                    if (nextIngredient == 0)
                        active = 0;
                    else if (nextIngredient == 1)
                        active = 1;
                    else
                        active = 3;

                    for (int i = 0; i < 4; ++i)
                    {
                        if (i == active)
                            clientMeshes[i].gameObject.SetActive(true);
                    }
                    break;
                }
            case Recipe.RecipeType.CUT:
                {
                    int active = -1;

                    if (nextIngredient == 0)
                        active = 0;
                    else
                        active = 1;

                    for (int i = 0; i < 4; ++i)
                    {
                        if (i == active)
                            clientMeshes[i].gameObject.SetActive(true);
                    }
                    break;
                }
            case Recipe.RecipeType.COMB:
                {
                    int active = -1;

                    if (nextIngredient == 0)
                        active = 0;
                    else
                        active = 2;

                    for (int i = 0; i < 4; ++i)
                    {
                        if (i == active)
                            clientMeshes[i].gameObject.SetActive(true);
                    }
                    break;
                }
        }
    }

    public IEnumerator NewRecipe()
    {
        //if (ClientManager.Instance != null)
        //{
        //    int rand = UnityEngine.Random.Range(0, ClientManager.Instance.availableRecipes.Count);
        //    recipe = ClientManager.Instance.availableRecipes[rand];

        //    nextIngredient = 0;
        //    angryTime = 0;
        //    currentRecipeTime = 0f;
        //    currentIngredientGoal = recipe.ingredients[0].actionPressSeconds;
        //    totalRecipeTime = 0f;

        //    foreach (IngredientData ingredient in recipe.ingredients)
        //    {
        //        totalRecipeTime += ingredient.actionPressSeconds;
        //    }

        //    SetUpHairs();

        //    animator.SetTrigger("SpawnClient");

        //    yield return new WaitUntil(() => { return animator.GetCurrentAnimatorStateInfo(0).IsName("ClientChair"); });

        //    Debug.Log("I want " + recipe.name + ". " + gameObject.name);

        //    startCounting = true;

        //    while(ingredientBars.Count != recipe.ingredients.Count)
        //    {
        //        if(ingredientBars.Count < recipe.ingredients.Count)
        //        {
        //            Image ingredientBar = Instantiate(ingredientBars[0].gameObject, ingredientBars[0].transform.parent).GetComponent<Image>();
        //            ingredientBars.Add(ingredientBar);
        //        }
        //        else
        //        {
        //            Destroy(ingredientBars[ingredientBars.Count - 1]);
        //            ingredientBars.RemoveAt(ingredientBars.Count - 1);
        //        }
        //    }

        //    for(int i = 0; i < ingredientBars.Count; ++i)
        //    {
        //        ingredientBars[i].sprite = recipe.ingredients[i].sprite;
        //    }

        //    canvas.gameObject.SetActive(true);
        //    progressBar.anchoredPosition = new Vector2(-1.3f, 0f);

        //    yield return null;
        //}

        yield return null;
    }

    private IEnumerator RecipeCompleted()
    {
        Debug.Log("Now im going to the space, i have a wonderful hair. " + gameObject.name);

        animator.SetTrigger("ClientLeave");

        ClientManager.Instance.currentMoney += recipe.moneyInflow;
        recipe = null;
        playerStarted = null;
        canvas.gameObject.SetActive(false);

        startCounting = false;

        yield return new WaitUntil(() => { return animator.GetCurrentAnimatorStateInfo(0).IsName("OnlyChair"); });
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
    }

    private IEnumerator RecipeFailed()
    {
        Debug.Log("This hair saloon is shit. Im going away. " + gameObject.name);

        animator.SetTrigger("ClientLeave");

        ClientManager.Instance.currentMoney -= recipe.moneyPenalty;
        recipe = null;
        playerStarted = null;
        canvas.gameObject.SetActive(false);

        startCounting = false;

        yield return new WaitUntil(() => { return animator.GetCurrentAnimatorStateInfo(0).IsName("OnlyChair"); });
        ClientManager.Instance.ReEnableClientAfterXSeconds(this, UnityEngine.Random.Range(minTimeToRespawn, maxTimeToRespawn));
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
                    StartCoroutine(RecipeCompleted());
                }
            }
            else
            {
                StartCoroutine(RecipeFailed());  
            }
        }
    }

    public void UseToolStarted(PickUpController player, Ingredient tool)
    {
        if (recipe == null || tool == null)
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
            StartCoroutine(RecipeFailed());
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
                currentRecipeTime += Time.deltaTime;
           
                float percent = currentRecipeTime / totalRecipeTime;
                progressBar.anchoredPosition = new Vector2( -1.3f * (1 - percent), 0f);

                if(currentRecipeTime > currentIngredientGoal)
                {
                    ingredientBars[nextIngredient].sprite = ClientManager.Instance.tickImage;
                    nextIngredient++;
                    SetUpHairs();
                    UseToolFinished();

                    if(nextIngredient < recipe.ingredients.Count)
                        currentIngredientGoal += recipe.ingredients[nextIngredient].actionPressSeconds;
                }

                if (currentRecipeTime > totalRecipeTime)
                {
                    StartCoroutine(RecipeCompleted());                   
                }
            }
            else
            {
                //Now it should never enter here
                StartCoroutine(RecipeFailed());
            }
        }    
    }

    public void SelectMeshes()
    {
        foreach(MeshRenderer meshRenderer in clientMeshes)
        {
            meshRenderer.material.color *= PlaceableSurface.selectedColorMultiplier;
        }
    }

    public void UnSelectMeshes()
    {
        foreach (MeshRenderer meshRenderer in clientMeshes)
        {
            meshRenderer.material.color /= PlaceableSurface.selectedColorMultiplier;
        }
    }
}

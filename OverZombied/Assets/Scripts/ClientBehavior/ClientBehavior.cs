using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public Animator animator;

    public Canvas canvas;
    public RectTransform foregroundProgressBar;
    public TextMeshProUGUI percentText;

    [SerializeField]
    private List<MeshRenderer> clientMeshes;

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
        if (ClientManager.Instance != null)
        {
            int rand = UnityEngine.Random.Range(0, ClientManager.Instance.availableRecipes.Count);
            recipe = ClientManager.Instance.availableRecipes[rand];

            nextIngredient = 0;
            angryTime = 0;
            usefulCounter = 0f;

            SetUpHairs();

            animator.SetTrigger("SpawnClient");

            yield return new WaitUntil(() => { return animator.GetCurrentAnimatorStateInfo(0).IsName("ClientChair"); });

            Debug.Log("I want " + recipe.name + ". " + gameObject.name);

            startCounting = true;

            canvas.gameObject.SetActive(true);
            foregroundProgressBar.anchoredPosition = new Vector2(1.3f, 0f);
            percentText.text = "0%";

            yield return null;
        }

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
                usefulCounter += Time.deltaTime;

                float percent = usefulCounter / tool.data.actionPressSeconds;
                foregroundProgressBar.anchoredPosition = new Vector2( 1.3f * (1 - percent), 0f);
                percentText.text = (percent * 100).ToString("0.00") + "%";

                if (usefulCounter > tool.data.actionPressSeconds)
                {
                    usefulCounter = 0f;
                    foregroundProgressBar.anchoredPosition = new Vector2(1.3f, 0f);
                    percentText.text = "0%";

                    nextIngredient++;

                    SetUpHairs();

                    if (nextIngredient < recipe.ingredients.Count)
                    {
                        Debug.Log("I still want " + recipe.ingredients[nextIngredient].name + ". " + gameObject.name);
                        UseToolFinished();
                    }
                    else
                    {
                        StartCoroutine(RecipeCompleted());
                    }
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

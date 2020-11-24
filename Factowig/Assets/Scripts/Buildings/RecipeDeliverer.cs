using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDeliverer : MonoBehaviour
{
    public static RecipeDeliverer Instance;

    private List<Ingredient> translatedIngredients = new List<Ingredient>();

    public float beltSpeed = 5f;

    [SerializeField]
    private MeshRenderer beltRenderer = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        for(int i = 0; i < translatedIngredients.Count; ++i)
        {
            Ingredient ingredient = translatedIngredients[i];

            ingredient.transform.Translate(Vector3.forward * beltSpeed * Time.deltaTime);

            if(ingredient.transform.position.x > 16)
            {
                translatedIngredients.Remove(ingredient);
                Destroy(ingredient.gameObject);
                i--;
            }
        }
    }

    public bool Deliver(Ingredient ingredient)
    {
        if (!RecipeManager.Instance.IsFinalIngredient(ingredient.data))
            return false;

        int index;
        RecipeUI recipeUI = RecipeManager.Instance.GetRecipeUI(ingredient, out index);
        if(!recipeUI)
        {
            GameManager.Instance.currentMoney -= GameManager.Instance.moneyPenaltyOnFail;
        }      
        else
        {
            RecipeManager.Instance.RecipeDelivered(recipeUI);
            GameManager.Instance.currentMoney += recipeUI.recipe.moneyInflow + recipeUI.recipe.moneyBonus * (1 - Mathf.Clamp(recipeUI.counter / recipeUI.recipe.timeLimit, 0f, 1f));
        }

        WigDispenser.Instance.itemsSpawned -= 1;

        #region ADD TO BELT
        ingredient.transform.rotation = transform.rotation;
        ingredient.transform.position = new Vector3(8.5f, beltRenderer.bounds.size.y + ingredient.renderer.bounds.extents.y, ingredient.transform.position.z);
        ingredient.gameObject.SetActive(true);

        if (ingredient.rb)
        {
            ingredient.rb.velocity = ingredient.rb.angularVelocity = Vector3.zero;
            ingredient.rb.isKinematic = true;
        }

        if (ingredient.collider)
            ingredient.collider.enabled = false;

        translatedIngredients.Add(ingredient);
        #endregion

        ScoreUI.Instance.RepositionProgressBar();

        return true;
    }
}

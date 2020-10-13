using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDeliverer : MonoBehaviour
{
    public static RecipeDeliverer Instance;

    private List<Ingredient> translatedIngredients = new List<Ingredient>();

    public float beltSpeed = 5f;

    [SerializeField]
    private MeshRenderer beltRenderer;

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
            //Delivery failed
        }
        else if(index == 0)
        {
            //More points?
            RecipeManager.Instance.RecipeDelivered(recipeUI);
        }
        else
        {
            //Normal points
        }

        ingredient.transform.position = new Vector3(8.5f, beltRenderer.bounds.size.y + ingredient.renderer.bounds.extents.y, ingredient.transform.position.z);
        ingredient.transform.rotation = transform.rotation;
        ingredient.gameObject.SetActive(true);

        if (ingredient.rb)
        {
            ingredient.rb.velocity = ingredient.rb.angularVelocity = Vector3.zero;
            ingredient.rb.isKinematic = true;
        }

        translatedIngredients.Add(ingredient);

        return true;
    }
}

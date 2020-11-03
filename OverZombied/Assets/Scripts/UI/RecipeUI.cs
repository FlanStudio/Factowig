using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [HideInInspector]
    public Recipe recipe { get; private set; }

    public RectTransform maskTransform;
    public RectTransform progressBar;

    public Image cardImage;
    public Image mainImage;
    private Image progressBarImage;

    [HideInInspector]
    public bool activated = false;

    public float counter { get; private set; } = 0f;

    private void Update()
    {
        if (recipe)
        {
            counter += Time.deltaTime;
            RepositionProgressBar();
        }
        else
            counter = 0f;
    }

    public void SetRecipe(Recipe recipe)
    {
        this.recipe = recipe;

        counter = 0f;

        cardImage.sprite = recipe.UISprite;
        mainImage.sprite = recipe.UISprite;

        gameObject.SetActive(true);

        //LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
    }

    public void RepositionProgressBar()
    {
        if(!progressBarImage)
            progressBarImage = progressBar.GetComponent<Image>();

        float percent = recipe ? Mathf.Clamp(counter / recipe.timeLimit, 0, 1) : 0;
        if(percent < 0.5)
        {
            progressBarImage.color = Color.green /*new Color(0.6f, 0.7607844f, 0.1058824f)*/;
        }
        else if(percent >= 0.5)
        {
            if (percent > 0.75)         
                progressBarImage.color = Color.red;       
            else
                progressBarImage.color = Color.yellow;
        }

        progressBar.anchoredPosition = new Vector2(0 - maskTransform.rect.width * (percent), progressBar.anchoredPosition.y);
    }
}

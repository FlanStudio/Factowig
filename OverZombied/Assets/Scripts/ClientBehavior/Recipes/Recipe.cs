using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "FlanStudio/Recipe", order = 2)]
public class Recipe : ScriptableObject
{
    public List<IngredientData> ingredients;

}

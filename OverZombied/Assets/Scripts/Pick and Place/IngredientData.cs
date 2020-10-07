using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "FlanStudio/Ingredient", order = 1)]
public class IngredientData : ScriptableObject
{
    public bool isWig = false;
    [Header("Only for wigs")]
    public int wigIndex = 0;

    [Space()]
    public bool throwable = true;

    public enum TYPE { TOOL, RESOURCE }
    public TYPE type = TYPE.TOOL;
    public new string name = "Default";

    public Sprite sprite;

    public float actionPressSeconds = 0f;

    public int durability = 1;
}
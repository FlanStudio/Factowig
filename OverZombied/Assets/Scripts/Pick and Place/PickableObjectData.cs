using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickableObjectData", menuName = "FlanStudio/PickableObjectData", order = 1)]
public class PickableObjectData : ScriptableObject
{
    public enum TYPE { TOOL, RESOURCE }
    public TYPE type = TYPE.TOOL;
    public new string name = "Default";

    [Header("Only for tools")]
    public float actionPressSeconds = 5f;
}
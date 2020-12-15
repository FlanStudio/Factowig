using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UILevel : MonoBehaviour
{
    public int id = 0;

    [SerializeField]
    protected Selectable startSelected = null;

    protected virtual void OnEnable()
    {
        if (startSelected)
            startSelected.Select();
    }
}
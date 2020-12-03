using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelPauseMenu : MonoBehaviour
{
    public Selectable startSelected = null;

    private void OnEnable()
    {
        if (startSelected)
            startSelected.Select();
    }
}

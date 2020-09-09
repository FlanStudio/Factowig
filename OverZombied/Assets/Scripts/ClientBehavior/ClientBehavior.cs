using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientBehavior : MonoBehaviour
{
    private Recipe recipe = null;

    private void Awake()
    {
        if (ClientManager.Instance == null)
            return;

        int rand = UnityEngine.Random.Range(0, ClientManager.Instance.availableRecipes.Count);
        recipe = ClientManager.Instance.availableRecipes[rand];

        Debug.Log("I want " + recipe.name);
    }
}

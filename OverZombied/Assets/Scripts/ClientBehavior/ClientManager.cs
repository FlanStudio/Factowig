using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance;

    public uint startEnabled = 2u;
    public float totalTime = 10f;

    public AnimationCurve activateChairsCurve;

    [SerializeField]
    private List<ClientBehavior> clients = new List<ClientBehavior>();

    public List<Recipe> availableRecipes = new List<Recipe>();

    private uint enabledChairs = 0u;

    public float currentMoney = 0f;

    public Sprite tickImage;

    private void Awake()
    {
        Instance = this;

        if(startEnabled <= clients.Count)
        {
            for(int i = 0; i < startEnabled; ++i)
            {
                clients[i].activated = true;
                StartCoroutine(clients[i].NewRecipe());              
            }
            enabledChairs = startEnabled;
        }
    }

    private void Update()
    {
        float chairsPercent = activateChairsCurve.Evaluate(Time.time / totalTime);

        uint amountChairs = (uint)Mathf.FloorToInt(chairsPercent * (clients.Count - startEnabled)) + startEnabled;

        if(amountChairs > enabledChairs)
        {
            foreach(ClientBehavior chair in clients)
            {
                if(!chair.activated)
                {
                    StartCoroutine(chair.NewRecipe());
                    enabledChairs++;
                    chair.activated = true;

                    if (amountChairs <= enabledChairs)
                    {
                        break;
                    }
                }
            }
        }
    }

    public void ReEnableClientAfterXSeconds(ClientBehavior client, float seconds)
    {
        StartCoroutine(ReEnableClientAfterXSecondsCorroutine(client, seconds));
    }

    private IEnumerator ReEnableClientAfterXSecondsCorroutine(ClientBehavior client, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(client.NewRecipe());
    }
}
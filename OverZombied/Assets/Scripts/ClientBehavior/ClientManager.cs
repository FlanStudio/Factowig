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

    private void Awake()
    {
        Instance = this;

        if(startEnabled <= clients.Count)
        {
            for(int i = 0; i < startEnabled; ++i)
            {
                clients[i].gameObject.SetActive(true);
            }
            enabledChairs = startEnabled;
        }
    }

    private void Update()
    {
        float chairsPercent = activateChairsCurve.Evaluate(Time.time / totalTime);

        uint amountChairs = (uint)Mathf.FloorToInt(chairsPercent * (clients.Count - startEnabled)) + startEnabled;

        while(amountChairs > enabledChairs)
        {
            foreach(ClientBehavior chair in clients)
            {
                if(!chair.gameObject.activeSelf)
                {
                    chair.gameObject.SetActive(true);
                    enabledChairs++;
                    break;
                }
            }
        }
    }
}
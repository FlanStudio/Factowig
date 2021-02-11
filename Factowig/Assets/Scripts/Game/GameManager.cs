using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float levelDurationSeconds = 120f;

    [Header("Currency")]
    public float currentMoney = 0f;
    public float moneyPenaltyOnFail = 10f;

    public float finalMoneyGoal = 100f;

    [HideInInspector]
    public float deliveredMoney = 0f;
    [HideInInspector]
    public float bonusMoney = 0f;
    [HideInInspector]
    public float failedMoney = 0f;

    [HideInInspector]
    public int stars = 0;

    private void Awake()
    {
        Instance = this;
    }
}

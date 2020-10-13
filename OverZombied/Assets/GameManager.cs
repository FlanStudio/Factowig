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

    private void Awake()
    {
        Instance = this;
    }
}

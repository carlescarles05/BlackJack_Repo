using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachinesTimeManager : MonoBehaviour
{
    public static SlotMachinesTimeManager Instance { get; private set; }
    public int DefaultYears = 1000; //Default starting years
    public int TotalYears { get; private set; } = 1000; //default starting years

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ResetToDefault()
    {
        TotalYears = DefaultYears;
        Debug.Log($"Time reset to default:{DefaultYears}");
    }
    public void SetYears(int years)
    {
        TotalYears = Mathf.Max(0,years); //used to prevent negative values.
    }
    public void AddYears(int years)
    {
        TotalYears = Mathf.Max(0,TotalYears + years); //Add/Substract years safaely.

    }
}

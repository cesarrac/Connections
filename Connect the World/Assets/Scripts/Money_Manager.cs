using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Money_Manager : MonoBehaviour {

    public static Money_Manager instance;

    decimal totalCapital;
    public decimal startingCapital = 10.00m;

    Dictionary<string, decimal> assetCost_Dict = new Dictionary<string, decimal>();

    public Text currCapitalTxt;

    void Awake()
    {
        instance = this;

        totalCapital = startingCapital;

        InitCosts();
    }

    void InitCosts()
    {
        assetCost_Dict.Add("Pole", 2.0m);
        assetCost_Dict.Add("Basic Connection", 1.0m);
    }

    void Start()
    {
        DisplayCurrentCapital();
    }

    public bool Purchase(string assetName)
    {
        if (assetCost_Dict.ContainsKey(assetName))
        {
            if (totalCapital >= assetCost_Dict[assetName])
            {
                totalCapital -= assetCost_Dict[assetName];
                DisplayCurrentCapital();
                return true;
            }
            else
            {
                Debug.Log("MONEY: No capital left to make that purchase!");
                return false;
            }
            
        }
        else
            return false;
    }

    public void Pay(decimal pay)
    {
        totalCapital += pay;
        DisplayCurrentCapital();
    }

    void DisplayCurrentCapital()
    {
        currCapitalTxt.text = totalCapital.ToString();
    }

}

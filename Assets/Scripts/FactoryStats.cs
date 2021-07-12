using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryStats : MonoBehaviour {

    public static FactoryStats instance;
    public GameObject FactoryStatsWin;

    public Text productivity;
    public Text priceOfParty;
    public Text timeBeforeSell;

    void Awake()
    {
        instance = this;
    }
    public void ShowStats(GameObject factory)
    {
        FactoryStatsWin.SetActive(true);
        productivity.text = factory.GetComponent<ZavodScript>().currentProductivity.ToString();
        priceOfParty.text = factory.GetComponent<ZavodScript>().currentPriceOfParty.ToString();
        timeBeforeSell.text = factory.GetComponent<ZavodScript>().currentTimeBeforeSell.ToString();
    }
    public void HideStats()
    {
        FactoryStatsWin.SetActive(false);
    }
}


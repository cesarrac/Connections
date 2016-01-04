using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class Cities_Manager : MonoBehaviour {

    public static Cities_Manager instance;

    World_Generator world;

    public Text cityName, cityHeal, cityEd, cityEnt, cityEcon, citySpirit, cityTech, cityDef, cityRating, cityPop, cityAgri;

    public GameObject cityStatsPanel;

    public List<CityStats> all_CityStats = new List<CityStats>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        world = World_Generator.instance;

    }

    public void AddCityStat(CityStats newStats)
    {
        all_CityStats.Add(newStats);
    }

    public void UpdateCityInfoPanel(int x)
    {
        if (cityStatsPanel.activeSelf)
        {
            // If the panel is ON we update it
            DisplayCityInfo(x);
        }
    }

    public void DisplayCityInfo(int x)
    {
        City thisCity = world.GetCityFromTilePosX(x);

        if (thisCity != null)
        {
            cityName.text = thisCity.name;
            cityHeal.text = thisCity.cityStats.health.ToString();
            cityEd.text = thisCity.cityStats.education.ToString();
            cityEnt.text = thisCity.cityStats.entertainment.ToString();
            cityEcon.text = thisCity.cityStats.economy.ToString();
            cityDef.text = thisCity.cityStats.defense.ToString();
            cityTech.text = thisCity.cityStats.technology.ToString();
            citySpirit.text = thisCity.cityStats.spirituality.ToString();
            cityRating.text = thisCity.cityStats.average.ToString();
            cityPop.text = thisCity.population.ToString();
            cityAgri.text = thisCity.cityStats.agriculture.ToString();

            if (!cityStatsPanel.activeSelf)
            {
                cityStatsPanel.SetActive(true);
            }

        }
    }

    public void ClosecityStatsPanel()
    {
        cityStatsPanel.SetActive(false);
    }

    public void SortListBy(ConnectionType sortType)
    {
        switch (sortType)
        {
            case ConnectionType.Health:
                SortByHealth();
                break;
            case ConnectionType.Economy:
                SortByEconomy();
                break;
            case ConnectionType.Defense:
                SortByDefense();
                break;
            case ConnectionType.Technology:
                SortByTechnology();
                break;
            default:
                SortByRating();
                break;

        }

    }

    void SortByRating()
    {
        all_CityStats.Sort();

        foreach (CityStats stats in all_CityStats)
        {
            Debug.Log(stats.average);
        }
    }

    void SortByHealth()
    {
        all_CityStats.Sort(delegate (CityStats x, CityStats y)
        {
            if (x.health == 0 && y.health == 0) return 0;
            else if (x.health == 0) return -1;
            else if (y.health == 0) return 1;
            else return x.health.CompareTo(y.health);
        });

    }

    void SortByEconomy()
    {
        all_CityStats.Sort(delegate (CityStats x, CityStats y)
        {
            if (x.economy == 0 && y.economy == 0) return 0;
            else if (x.economy == 0) return -1;
            else if (y.economy == 0) return 1;
            else return x.economy.CompareTo(y.economy);
        });

    }

    void SortByTechnology()
    {
        all_CityStats.Sort(delegate (CityStats x, CityStats y)
        {
            if (x.technology == 0 && y.technology == 0) return 0;
            else if (x.technology == 0) return -1;
            else if (y.technology == 0) return 1;
            else return x.technology.CompareTo(y.technology);
        });

    }

    void SortByDefense()
    {
        all_CityStats.Sort(delegate (CityStats x, CityStats y)
        {
            if (x.defense == 0 && y.defense == 0) return 0;
            else if (x.defense == 0) return -1;
            else if (y.defense == 0) return 1;
            else return x.defense.CompareTo(y.defense);
        });

    }

    public void SortIndividualCity(City city)
    {
        Dictionary<ConnectionType, int> thisCityStats = new Dictionary<ConnectionType, int> {
            {ConnectionType.Health, city.cityStats.health },
            {ConnectionType.Education, city.cityStats.education },
            {ConnectionType.Spirituality, city.cityStats.spirituality },
            {ConnectionType.Technology, city.cityStats.technology },
            {ConnectionType.Economy, city.cityStats.economy },
            {ConnectionType.Defense, city.cityStats.defense },
            {ConnectionType.Entertainment, city.cityStats.entertainment}
        };


        List<KeyValuePair<ConnectionType, int>> myList = thisCityStats.ToList();

        myList.Sort((firstPair, nextPair) =>
        {
            return firstPair.Value.CompareTo(nextPair.Value);
        }
        );

        // Assign the highest this city has
        city.highestStatType = myList[myList.Count - 1].Key;
        Debug.Log("CITIES MAN: " + city.name + " highest stat is " + city.highestStatType);
    }

}

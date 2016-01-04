using UnityEngine;
using System.Collections;
using System;


public class CityStats : IComparable<CityStats>
{
    public int myCity_XPos;
    public int health;
    public int education;
    public int technology;
    public int economy;
    public int defense;
    public int spirituality;
    public int entertainment;
    public int agriculture;
    public int[] statsArray;
    public decimal average;

    string seed;

    public CityStats(string _seed, int cityX)
    {
        seed = _seed;
        myCity_XPos = cityX;
        InitStats();
    }

    void InitStats()
    {
        System.Random pseudoRandom = new System.Random (seed.GetHashCode ());

        health = pseudoRandom.Next(0, 100);
        education = pseudoRandom.Next(0, 100);
        technology = pseudoRandom.Next(0, 100);
        economy = pseudoRandom.Next(0, 100);
        defense = pseudoRandom.Next(0, 100);
        spirituality = pseudoRandom.Next(0, 100);
        entertainment = pseudoRandom.Next(0, 100);
        agriculture = pseudoRandom.Next(0, 100);

        statsArray = new int[] { health, education, technology, economy, defense, spirituality, entertainment, agriculture };

        average = Average();

    }

    int Sum()
    {
        int result = 0;

        for (int i = 0; i < statsArray.Length; i++)
        {
            result += statsArray[i];
        }

        return result;
    }

    decimal Average()
    {
        int sum = Sum();
        decimal result = sum / statsArray.Length;
        return result;
    }

    public void CalculateAverage()
    {
        average = Average();
    }

    public int CompareTo(CityStats compareStats)
    {
        return this.average.CompareTo(compareStats.average);
    }
}

[System.Serializable]
public class City  {

    public string name;

    public int population;

    public int gridWidth, gridHeight;

    public enum Status
    {
        AT_WAR,
        AT_PEACE
    }

    public Status cityStatus = Status.AT_PEACE;

    public CityStats cityStats;

    // Where cable connections would hook up to.
    public Vector3 connectorPosition;

    public int worldX;

    public ConnectionType highestStatType;


    public City(string _name, int pop, int x, int width, int height, Vector3 _connectorPos)
    {
        name = _name;
        population = pop;
        worldX = x;

        gridHeight = height;
        gridWidth = width;

        connectorPosition = _connectorPos;

        // Right now we are using the name of the city to generate its random starting stats
        cityStats = new CityStats(name, worldX);
    }

    public int GetStat(ConnectionType connectType)
    {
        int stat = 0;

        switch (connectType)
        {
            case ConnectionType.Health:
                stat = cityStats.health;
                break;
            case ConnectionType.Education:
                stat = cityStats.education;
                break;
            case ConnectionType.Technology:
                stat = cityStats.technology;
                break;
            case ConnectionType.Spirituality:
                stat = cityStats.spirituality;
                break;
            case ConnectionType.Entertainment:
                stat = cityStats.entertainment;
                break;

            default:
                stat = 0;
                break;
        }

        return stat;
    }

    public void ChangeStat(ConnectionType statType, int ammnt)
    {
        switch (statType)
        {
            case ConnectionType.Health:
                cityStats.health += ammnt;
                break;
            case ConnectionType.Education:
                cityStats.education += ammnt;
                break;
            case ConnectionType.Technology:
                cityStats.technology += ammnt;
                break;
            case ConnectionType.Spirituality:
                cityStats.spirituality += ammnt;
                break;
            case ConnectionType.Entertainment:
                cityStats.entertainment += ammnt;
                break;
            default:
                // no change
                break;
        }

        // After changing a stat (+/-) the City needs to recalculate its stats average
        cityStats.CalculateAverage();
    }

    public void ChangePopulation(int change)
    {
        population += change;
    }
}

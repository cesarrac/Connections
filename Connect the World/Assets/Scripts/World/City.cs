using UnityEngine;
using System.Collections;
using System;


[System.Serializable]
public class CityStats
{
    public int health;
    public int education;
    public int technology;
    public int economy;
    public int defense;
    public int spirituality;

    string seed;

    public CityStats(string _seed)
    {
        seed = _seed;

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

    public float worldX;


    public City(string _name, int pop, float x, int width, int height, Vector3 _connectorPos)
    {
        name = _name;
        population = pop;
        worldX = x;

        gridHeight = height;
        gridWidth = width;

        connectorPosition = _connectorPos;

        // Right now we are using the name of the city to generate its random starting stats
        cityStats = new CityStats(name);
    }

}

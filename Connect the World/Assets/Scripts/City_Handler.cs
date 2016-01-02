using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class City_Handler : MonoBehaviour {

    public City myCity;

    World_Generator world;

    int gridPosX;

    Dictionary<Guid, Connection> cityConnections = new Dictionary<Guid, Connection>();

    void OnEnable()
    {
        gridPosX = Mathf.RoundToInt(transform.position.x);
        cityConnections.Clear();
    }

    void Start()
    {
       
        world = World_Generator.instance;
        myCity = world.GetCityAtLocationX(gridPosX);

    }

    public void AddNewConnection(Guid id, Connection connection)
    {
        cityConnections.Add(id, connection);
        Debug.Log(myCity.name + " added a new " + connection.connectionMode + " of type " + connection.connectionType);
    }

 

    //void DebugCityStats(int x)
    //{
    //    City thisCity = world.GetCityAtLocationX(x);
    //    if (thisCity != null)
    //    {
    //        Debug.Log("Name: " + thisCity.name + " Pop: " + thisCity.population + " Stats: ");
    //        Debug.Log("Health: " + thisCity.cityStats.health + " Education: " + thisCity.cityStats.education);
    //    }
    //    else
    //        Debug.Log("MOUSE cant find the city you are looking for! Maybe you're not clicking the base tile of that city... ");
    //}
}

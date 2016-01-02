using UnityEngine;
using System.Collections;

public class City_Handler : MonoBehaviour {

    public City myCity;

    World_Generator world;

    int gridPosX;

 

    void Start()
    {
        gridPosX = Mathf.RoundToInt(transform.position.x);
        world = World_Generator.instance;
        myCity = world.GetCityAtLocationX(gridPosX);

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

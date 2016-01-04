using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class City_Handler : MonoBehaviour {

    public City myCity;

    World_Generator world;

    int gridPosX;

    Dictionary<Guid, Connection> cityConnections = new Dictionary<Guid, Connection>();

    float startingYScale = 1.0f;
    float _curYScale;
    float curYScale { get { return _curYScale; } set { _curYScale = Mathf.Clamp(value, 0.1f, 1.0f); } }

    int cityPopulation, currPopulation;

    SpriteRenderer sr;

    public bool isPayingSubscription { get; protected set; }

    void OnEnable()
    {
        curYScale = startingYScale;

        gridPosX = Mathf.RoundToInt(transform.position.x);
        cityConnections.Clear();

        isPayingSubscription = false;
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        world = World_Generator.instance;
        myCity = world.GetCityAtLocationX(gridPosX);

        cityPopulation = myCity.population;

        StartCoroutine("CheckScale");
    }

    public void AddNewConnection(Guid id, Connection connection)
    {
        cityConnections.Add(id, connection);
        Debug.Log(myCity.name + " added a new " + connection.connectionMode + " of type " + connection.connectionType);

        if (!isPayingSubscription)
        {
            isPayingSubscription = true;
            StartCoroutine("PayingSub");
        }
    }

    IEnumerator CheckScale()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (myCity.population == 0)
            {
                KillCity();
                yield break;
            }

            if (myCity.population < cityPopulation)
            {
                DecreaseScale();
                
            }
            else if (myCity.population > cityPopulation)
            {
                IncreaseScale();
            }

            cityPopulation = myCity.population;
        }
    }

    void DecreaseScale()
    {
        if (curYScale > 0.1f)
        {
            transform.localScale = new Vector3(1.0f, curYScale - 0.1f, 1.0f);
            curYScale = transform.localScale.y;
        }
    }

    void IncreaseScale()
    {
        if (curYScale < 1.0f)
        {
            transform.localScale = new Vector3(1.0f, curYScale + 0.1f, 1.0f);
            curYScale = transform.localScale.y;
        }
    }

    void KillCity()
    {
        // Change the name to name + dead
        myCity.name = myCity.name + "_DEAD";

        // Change the sprite's color to black. This can later be changed to something that changes the sprite entirely to a visual that represents the motive of death (e.g. Epidemic, War, etc.)
        sr.color = Color.black;
    }

    IEnumerator PayingSub()
    {
        while (true)
        {
            // Pay subscription (once this city actually has connections) once a month
            if (cityConnections.Count > 0)
            {
                yield return new WaitForSeconds(World_Events_Manager.instance.monthInSeconds);
                PaySubscription();
            }
            else
            {
                isPayingSubscription = false;
                yield break;
            }
             
        }
    }

    void PaySubscription()
    {
        // Total number of connections this city has, total population
        Money_Manager.instance.ChargeSubscriptionFee(cityConnections.Count, myCity.population);
        Debug.Log(myCity.name + " is paying subscription!");
    }
}

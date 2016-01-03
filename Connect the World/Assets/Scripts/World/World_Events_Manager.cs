using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World_Events_Manager : MonoBehaviour {

    public static World_Events_Manager instance { get; protected set; }

    public float monthInSeconds = 60f;

    WorldEvent curWorldEvent;

    List<WorldEvent> pastEvents = new List<WorldEvent>();

    int populationPenalty = -100;
    int statPenalty = -5;
    int statBonus = 2;

    Cities_Manager city_manager;

    City lowestForPopChange_1, lowestForPopChange_2, lowestForHighStat_1, lowestForHighStat_2, lowestEconomy_1, lowestEconomy_2, lowestDef_1, lowestDef_2, lowestTech_1, lowestTech2;

    bool isGettingNewEvent = true;

    WorldEvent[] eventArray;

    WorldEvent epidemic = new WorldEvent(WorldEventType.EPIDEMIC, 2); // <-------- affects low health cities, subtracts population
    WorldEvent terrorism = new WorldEvent(WorldEventType.TERRORISM, 1); // <-------- affects low defense, subtracts population + lowers highest stat + rises defense (unless their highest stat is already defense)
    WorldEvent disaster = new WorldEvent(WorldEventType.NATURAL_DISASTER, 1); // <-------- affects low technology, subtracts population
    WorldEvent market_crash = new WorldEvent(WorldEventType.MARKET_CRASH, 6); // <-------- affects low economy, subtracts highest stat

    bool isCurrentlyOnEvent = false;

    void Awake()
    {
        instance = this;
    }

    public void StartGeneratingEvents()
    {
        city_manager = Cities_Manager.instance;

        eventArray = new WorldEvent [] { epidemic, terrorism, disaster, market_crash };

        isGettingNewEvent = true;
        StartCoroutine("CheckForNewEvents");
    }

    void Update()
    {
        if (curWorldEvent.isPastEvent && !isGettingNewEvent)
        {
            isGettingNewEvent = true;
            StartCoroutine("CheckForNewEvents");
        }

        if (!curWorldEvent.isPastEvent && !isCurrentlyOnEvent)
        {
            isCurrentlyOnEvent = true;
            PlayNewEvent();
        }

    }

    IEnumerator CheckForNewEvents()
    {
        while (true)
        {
            // Right now this will grab an event that is stored on this script, but later it might read it from an external database
            WorldEvent newEvent = GetNewEvent(Random.Range(0, eventArray.Length));

            if (curWorldEvent != null)
            {
                if (newEvent.eventType != curWorldEvent.eventType)
                {
                    isGettingNewEvent = false;
                    curWorldEvent = newEvent;
                    yield break;
                }
            }
            else
            {
                isGettingNewEvent = false;
                curWorldEvent = newEvent;
                yield break;
            }
           

            // If we dont get a new random event that is of a type different to the current one, we wait until next month!
            yield return new WaitForSeconds(monthInSeconds);

           
        }
    }

    WorldEvent GetNewEvent(int i)
    {
        return eventArray[i];
    }


    void PlayNewEvent()
    {
        switch (curWorldEvent.eventType)
        {
            case WorldEventType.EPIDEMIC:
                DecreasePopulationEvent(ConnectionType.Health);
                break;
            case WorldEventType.MARKET_CRASH:
                DecreaseHighestStatEvent(ConnectionType.Economy);
                break;
            case WorldEventType.NATURAL_DISASTER:
                DecreasePopulationEvent(ConnectionType.Technology);
                break;
            case WorldEventType.TERRORISM:
                DecreasePopulationEvent(ConnectionType.Defense);
                DecreaseHighestStatEvent(ConnectionType.Defense);
                //RaiseStatEvent(ConnectionType.Defense);
                break;
            default:
                break;
        }
    }



    public void DecreasePopulationEvent(ConnectionType sortType)
    {
        // Population Decrease affects the first and second cities with the lowest selected sortType
        city_manager.SortListBy(sortType);

        // Ask the cities manager for the 2 cities with the least Health and decrease their population by the penalty
        lowestForPopChange_1 = World_Generator.instance.GetCityAtLocationX(city_manager.all_CityStats[0].myCity_XPos);
        lowestForPopChange_2 = World_Generator.instance.GetCityAtLocationX(city_manager.all_CityStats[1].myCity_XPos);

        StartCoroutine("DecreasePopulation");
    }

    public void DecreaseHighestStatEvent(ConnectionType sortType)
    {
        city_manager.SortListBy(sortType);

        lowestForHighStat_1 = World_Generator.instance.GetCityAtLocationX(city_manager.all_CityStats[0].myCity_XPos);
        lowestForHighStat_2 = World_Generator.instance.GetCityAtLocationX(city_manager.all_CityStats[1].myCity_XPos);


        // Sort the stats of these two cities to assign their highest stats
        city_manager.SortIndividualCity(lowestForHighStat_1);
        city_manager.SortIndividualCity(lowestForHighStat_2);

        StartCoroutine("DecreaseHighestStat");

    }



    IEnumerator DecreasePopulation()
    {
        while (true)
        {
            if (curWorldEvent.duration > 0)
            {
                curWorldEvent.duration--;

                lowestForPopChange_1.ChangePopulation(populationPenalty);
                lowestForPopChange_2.ChangePopulation(populationPenalty);

                // After making a change, update the city info panel
                city_manager.UpdateCityInfoPanel(lowestForPopChange_1.worldX);
                city_manager.UpdateCityInfoPanel(lowestForPopChange_2.worldX);

                Debug.Log(lowestForPopChange_1.name + " and " + lowestForPopChange_2.name + "'s citizens are dying from " + curWorldEvent.eventType);
                
            }
            else
            {
                // Now that the event has passed, flag it and store it
                curWorldEvent.isPastEvent = true;
                isCurrentlyOnEvent = false;
                pastEvents.Add(curWorldEvent);

                yield break;
            }

            yield return new WaitForSeconds(monthInSeconds);


          
        }
    }

    IEnumerator DecreaseHighestStat()
    {
        while (true)
        {
            if (curWorldEvent.duration > 0)
            {
                curWorldEvent.duration--;

                lowestForHighStat_1.ChangeStat(lowestForHighStat_1.highestStatType, statPenalty);
                lowestForHighStat_2.ChangeStat(lowestForHighStat_2.highestStatType, statPenalty);

                // After making a change, update the city info panel
                city_manager.UpdateCityInfoPanel(lowestForHighStat_1.worldX);
                city_manager.UpdateCityInfoPanel(lowestForHighStat_2.worldX);

                Debug.Log(lowestForHighStat_1.name + "'s " + lowestForHighStat_1.highestStatType +" industry has been hit hard by the recession.");
                Debug.Log(lowestForHighStat_2.name + "'s " + lowestForHighStat_2.highestStatType + " industry has been hit hard by the recession.");
            }
            else
            {
                // Now that the event has passed, flag it and store it
                curWorldEvent.isPastEvent = true;
                isCurrentlyOnEvent = false;
                pastEvents.Add(curWorldEvent);

                yield break;
            }

            yield return new WaitForSeconds(monthInSeconds);



        }
    }
}

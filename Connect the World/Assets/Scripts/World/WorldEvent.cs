using UnityEngine;
using System.Collections;

public enum WorldEventType
{
    NATURAL_DISASTER,
    EPIDEMIC,
    TERRORISM,
    MARKET_CRASH
}
public class WorldEvent {

    public WorldEventType eventType;
    public int duration;
    public bool isPastEvent; // < ------- flag to allow Manager to set a new current event and store this one for an event log

    // The Manager for world events will take care of telling the Cities manager to affect a city (e.g. Decrease Population, Decrease a stat, etc)
    // for the duration of this world event (in months). (i.e. Decrease Population takes x # of citizen's lives every month for (duration) months.)
    // The length of a month (in seconds) is set in the Manager and could be set by the player at the start of the game (clamped to something like 60f to 300f);
    // There can ONLY be one world event of the same type happening per month.

    public WorldEvent(WorldEventType _type, int _duration)
    {
        eventType = _type;
        duration = _duration;
    }
	
}

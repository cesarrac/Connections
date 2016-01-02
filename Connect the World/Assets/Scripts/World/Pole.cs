using UnityEngine;
using System.Collections;

public class Pole {

    public int posX { get; protected set; }

    public int maxConnections;

    public int currConnections;

    public City connectionA;
    public City connectionB;

    public Pole(int x, int maxConnects)
    {
        posX = x;
        maxConnections = maxConnects;
    }
}

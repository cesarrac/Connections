using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cities_Manager : MonoBehaviour {

    public static Cities_Manager instance;

    World_Generator world;

    List<int> cityXkeys = new List<int>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        world = World_Generator.instance;
    }

    public void AddKey(int x)
    {
        cityXkeys.Add(x);
    }

}

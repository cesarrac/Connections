using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Connection_Manager : MonoBehaviour {

    public static Connection_Manager instance;
    Dictionary<int, Pole_Handler> pole_handlers = new Dictionary<int, Pole_Handler>();


    void Awake()
    {
        instance = this;

    }

    public void AddNewPoleHandler(int x, Pole_Handler poleHandler)
    {
        pole_handlers.Add(x, poleHandler);
    }

    public Pole_Handler GetPoleHandler(int key)
    {
        if (pole_handlers.ContainsKey(key))
            return pole_handlers[key];
        else
            return null;
    }

    public Connection FindConnectionOnPoleHandler(int poleHandlerKey, Guid id)
    {
        if (pole_handlers.ContainsKey(poleHandlerKey))
        {
            return pole_handlers[poleHandlerKey].connectionOnThisPole[id];
        }
        else
            return null;
    }


}

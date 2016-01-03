using UnityEngine;
using System.Collections;
using System;

public class Connection_Handler : MonoBehaviour {

    Guid connectionID;

    Connection myConnection;

    World_Generator world;

    Pole_Handler originPole_handler;

    void Start()
    {
        
        world = World_Generator.instance;


    }

    public void Init(Guid id, Connection newConnection)
    {
        connectionID = id;
        myConnection = newConnection;

        Debug.Log("CONNECTION: Initialized connection!");

        originPole_handler = world.GameObjectFromTileXCoord(Mathf.RoundToInt(transform.parent.position.x)).GetComponent<Pole_Handler>();

        // As soon as the connection has been initialized, it can start Transferring Data ONLY if it's an INPUT connection
        if (newConnection.connectionMode == ConnectionMode.INPUT)
            StartCoroutine("TransferData");

    }


    // Input connections Receive Data from their connector
    void ReceiveData()
    {
        Debug.Log("CONNECTION: Receiving Data!");

        // This is a POLE to CITY connection. This means we are Inputting from the connector CITY.

        // First make sure that my pole has an output connection of my connection type
        if (originPole_handler == null)
            originPole_handler = world.GameObjectFromTileXCoord(Mathf.RoundToInt(transform.parent.position.x)).GetComponent<Pole_Handler>();

        if (originPole_handler.outputConnections.ContainsKey(myConnection.connectionType))
        {
            // Make sure that the connector City has a stat of this connectionType that is HIGHER than the receiver
            City myCity = world.GetCityFromTilePosX(myConnection.end_x_position);

            City receiverCity = originPole_handler.outputConnections[myConnection.connectionType].outputCity;
            if (receiverCity == null)
            {
                // if this is null it is probably because the Player set added an output connection to a pole that had no output connections
                // get the output city of my output connection's connector
                receiverCity = world.GameObjectFromTileXCoord(originPole_handler.outputConnections[myConnection.connectionType].
                    end_x_position).GetComponent<Pole_Handler>().outputConnections[myConnection.connectionType].outputCity;
                if (receiverCity == null)
                    Debug.LogError("CONNECTION: Could not find the output city!");
            }

            if (CompareCityStats(myCity, receiverCity, myConnection.connectionType))
            {
                // This connection can receive data of its type.
                // Since this Connection is connected directly to a city we can just add to its stat here
                SendData(receiverCity, myConnection.connectionType, myConnection.dataPacketSize);

                // If the city stats panel is on, update its information through the Cities Manager
                Cities_Manager.instance.UpdateCityInfoPanel(receiverCity.worldX);
            }
        }
        else
        {
            Debug.Log("CONNECTION: Could not find an Output connection in my origin pole handler! Did you forget to add one? Is the pole handler returning null?");
        }


        //else
        //{
        //    //// This is a POLE to POLE connection. This means we are Inputting from another Input connection on the origin pole

        //    //// Check that my origin pole has Output connections of this type
        //    //if (originPole_handler == null)
        //    //    originPole_handler = world.GameObjectFromTileXCoord(Mathf.RoundToInt(transform.parent.position.x)).GetComponent<Pole_Handler>();

        //    //// If the origin pole has inputConnections of this type then we have no receiver to send data to
        //    //if (originPole_handler.outputConnections.ContainsKey(myConnection.connectionType))
        //    //{
        //    //    // Check if this connector Pole has output connections of this type through my connector's Pole handler
        //    //    Pole_Handler pole_handler = world.GameObjectFromTileXCoord(myConnection.end_x_position).GetComponent<Pole_Handler>();

        //    //    if (pole_handler.outputConnections.ContainsKey(myConnection.connectionType))
        //    //    {
        //    //        // it contains a connection of this type

        //    //        // Since this is a POLE to POLE connection my city will be found through my origin Pole's input connections
        //    //        City myCity = myConnection.sourceCity;

        //    //        City receiverCity = world.GetCityFromTilePosX(pole_handler.outputConnections[myConnection.connectionType].end_x_position);

        //    //        // Make sure that the connector City has a stat of this connectionType that is HIGHER than the receiver
        //    //        if (CompareCityStats(myCity, receiverCity, myConnection.connectionType))
        //    //        {
        //    //            // This connection can receive data of its type.
        //    //            // Since this is connected to a pole we have to transfer stat data through the output connection to the city its connected to
        //    //            SendData(receiverCity, myConnection.connectionType, myConnection.dataPacketSize);
        //    //        }

        //    //    }
        //    //}


        //}
    }

    bool CompareCityStats(City a, City b, ConnectionType statToCompare)
    {
        if (a != null && b != null)
        {
            if (a.GetStat(statToCompare) > b.GetStat(statToCompare))
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            Debug.Log("CONNECTION: Can't find city to compare! City a = " + a + " City b = " + b);
            return false;
        }
         
    }



    void SendData(City recieverCity, ConnectionType c_type, int ammnt)
    {
        if (recieverCity != null)
        {
            recieverCity.ChangeStat(c_type, ammnt);
            Debug.Log("CONNECTION: Sending " + c_type + " data to " + recieverCity.name);
        }
   
    }



    IEnumerator TransferData()
    {
        while (true)
        {
            yield return new WaitForSeconds(myConnection.dataTransferSpeed);


            ReceiveData();

            yield return null;
        
          
        }
    }
}

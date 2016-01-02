using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Pole_Handler : MonoBehaviour {

    public Pole myPole;

    World_Generator world;

    int gridPosX;

    int maxConnections, curNumOfConnections = 0;

    LineRenderer curActiveLineR;

    public float connectorYPosition = 3.5f;

    bool isConnecting = false;

    Mouse_Controller mouse;

    ConnectionType curChoseConnectionType;
    ConnectionMode curChosenConnectionMode;

    Dictionary<Guid, Connection> connectionOnThisPole = new Dictionary<Guid, Connection>();
    public Dictionary<ConnectionType, Connection> inputConnections = new Dictionary<ConnectionType, Connection>();
    public Dictionary<ConnectionType, Connection> outputConnections = new Dictionary<ConnectionType, Connection>();

    GameObject connectionGObj;

    void OnEnable()
    {
        gridPosX = Mathf.RoundToInt(transform.position.x);
        connectionOnThisPole.Clear();
        curNumOfConnections = 0;
    }

    void Start()
    {
        //lineR = GetComponentInChildren<LineRenderer>();
        //lineR.gameObject.SetActive(false);

        curNumOfConnections = 0;

        world = World_Generator.instance;

        mouse = Mouse_Controller.instance;

        myPole = world.GetPoleAtLocationX(gridPosX);
        if (myPole == null)
            Debug.LogError("Couldnt find my Pole!");

        if (myPole != null)
            maxConnections = myPole.maxConnections;

    }

    void Update()
    {
        if (isConnecting)
            LineEndFollowMouse(curActiveLineR);
    }

    public void AddNewConnection(ConnectionMode connectMode, ConnectionType connectType)
    {
        curChoseConnectionType = connectType;
        curChosenConnectionMode = connectMode;

        if (curNumOfConnections < maxConnections)
        {
            // Make sure that this Pole does not already have an input or output connection of this type
            if (connectMode == ConnectionMode.INPUT)
            {
                if (inputConnections.ContainsKey(connectType))
                {
                    Debug.Log("POLE: Already have an INPUT connection of type " + connectType);
                    return;
                }
            }
            else
            {
                if (outputConnections.ContainsKey(connectType))
                {
                    Debug.Log("POLE: Already have an OUTPUT connection of type " + connectType);
                    return;
                }
            }
  
            // Spawn the connection
            connectionGObj = ObjectPool.instance.GetObjectForType("Connection", true, new Vector3(transform.position.x, connectorYPosition, 0));

            connectionGObj.transform.SetParent(transform, true);

            curNumOfConnections++;

            Activate(connectionGObj.GetComponent<LineRenderer>());
        }
    }

    void Activate(LineRenderer lineR)
    {
        Debug.Log("POLE: Activating!");
        if (curNumOfConnections < maxConnections && !isConnecting)
        {
            if (!lineR.gameObject.activeSelf)
            {
                lineR.gameObject.SetActive(true);
            }

            // Set the first position
            lineR.SetPosition(0, new Vector3(transform.position.x, connectorYPosition, 0));

            curActiveLineR = lineR;
            isConnecting = true;

        }
    }

    void LineEndFollowMouse(LineRenderer lineR)
    {
        // Set the second position to the mouse
        lineR.SetPosition(1, Mouse_Controller.instance.mousePosition);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // This cancels a connection by turning off the line renderer
            isConnecting = false;
            lineR.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Tile curTile = mouse.GetTileUnderMouse();
            if (curTile != null)
            {
                if (curTile.tileType == TileType.CITY)
                {
                    // Connecting to a City Behavior:
                    Connection newConnect = new Connection(Mathf.RoundToInt(mouse.mousePosition.x), curChoseConnectionType, 
                                            curChosenConnectionMode, Connector.City, myPole, 1, 30f);

                    // All input connections should know their source! and all Output connections should know their input's source!
                    if (newConnect.connectionMode == ConnectionMode.INPUT)
                    {
                        newConnect.sourceCity = world.GetCityFromTilePosX(curTile.posX);
                        Debug.Log("POLE: Source city set to " + newConnect.sourceCity.name);
                    }
                    else
                    {
                        newConnect.outputCity = world.GetCityFromTilePosX(curTile.posX);
                        Debug.Log("POLE: Output city set to " + newConnect.outputCity.name);
                    }

                    Debug.Log("POLE: Made a new " + newConnect.connectionMode + " connection of type " + newConnect.connectionType);
                    

                    isConnecting = false;

                    // Here we have to tell the city handler at the end of the connection that it's receiving a new connnection of this type from this pole
                    Guid id = Guid.NewGuid();

                    Debug.Log("New ID = " + id);

                    world.GameObjectFromTileXCoord(curTile.posX).GetComponent<City_Handler>().AddNewConnection(id, newConnect);

                    // Register the connection with this pole to keep track of its current connections
                    RegisterConnectionFromMe(id, newConnect);

                    // Set this new connection's Connection Handler
                    connectionGObj.GetComponent<Connection_Handler>().Init(id, newConnect);

                    /*
                    // THIS LINE BELOW, will cause the connection to stick to a position in the center of the city sprite. 
                    // Could FIX THIS to make the connections look tidier but they can't all connect to the same Vector3, if they do they look like one 
                    // continous line instead of multiple connections.

                    lineR.SetPosition(1, world.GetCityFromTilePosX(curTile.posX).connectorPosition);
                    */
                }
                else if (curTile.tileType == TileType.POLE)
                {
                    // Connecting to a Pole Behavior:
                    Connection newConnect = new Connection(Mathf.RoundToInt(mouse.mousePosition.x), curChoseConnectionType, 
                                            curChosenConnectionMode, Connector.Pole, myPole, 1, 30f);

                    // All input connections need its source. In this case, with a new Pole to Pole connection, find the corresponding input connection on this Pole
                    if (newConnect.connectionMode == ConnectionMode.INPUT)
                    {
                        if (inputConnections.ContainsKey(newConnect.connectionType))
                        {
                            newConnect.sourceCity = inputConnections[newConnect.connectionType].sourceCity;
                        }
                    }
                    else
                    {
                        // check this pole for outputs. There's a big chance that the player hasn't set any outputs yet of this type so this will need to be check again!
                        // Check the connector poles outputs
                        if (world.GameObjectFromTileXCoord(curTile.posX).GetComponent<Pole_Handler>().outputConnections.
                            ContainsKey(newConnect.connectionType))
                        {
                            newConnect.outputCity = world.GameObjectFromTileXCoord(curTile.posX).GetComponent<Pole_Handler>().
                                outputConnections[newConnect.connectionType].outputCity;
                        }
                        //else
                        //{
                        //    // CAN'T connect to a pole with an OUTPUT connection if that pole doesn not contain OUTPUTS of that type
                        //    isConnecting = false;
                        //    lineR.gameObject.SetActive(false);
                        //    Debug.Log("POLE: Can't place OUTPUT connection on a pole with no outputs!");

                        //    return;
                           

                        //}
                    }

                    Debug.Log("POLE: Made a new " + newConnect.connectionMode + " connection of type " + newConnect.connectionType);

                    isConnecting = false;

                    Guid id = Guid.NewGuid();


                    // Register the connection with this pole to keep track of its current connections
                    RegisterConnectionFromMe(id, newConnect);

                    // Set this new connection's Connection Handler
                    connectionGObj.GetComponent<Connection_Handler>().Init(id, newConnect);

                    // Register the connection on the pole on the end of this connection (reverse the connection mode!)
                    world.GameObjectFromTileXCoord(curTile.posX).GetComponent<Pole_Handler>().RegisterConnectionToMe(id, newConnect);

                 
                }
            }
           
         
        }
    }

    void RegisterConnectionFromMe(Guid id, Connection newConnection)
    {
        if (!connectionOnThisPole.ContainsKey(id))
            connectionOnThisPole.Add(id, newConnection);

        // add it to output or input connections
        if (newConnection.connectionMode == ConnectionMode.INPUT)
        {
            inputConnections.Add(newConnection.connectionType, newConnection);
        }
        else
        {
            outputConnections.Add(newConnection.connectionType, newConnection);
        }
    }

    public void RegisterConnectionToMe(Guid id, Connection newConnection)
    {

        // add it to output or input connections
        if (newConnection.connectionMode == ConnectionMode.INPUT)
        {
            // Since this is connecting to me as an input, that means it is MY output
            newConnection.connectionMode = ConnectionMode.OUTPUT;

            outputConnections.Add(newConnection.connectionType, newConnection);

            connectionOnThisPole.Add(id, newConnection);
        }
        else
        {
            // Since this is connecting to me as an output, that means it is MY input
            newConnection.connectionMode = ConnectionMode.INPUT;

            inputConnections.Add(newConnection.connectionType, newConnection);

            connectionOnThisPole.Add(id, newConnection);
        }

        //if (!connectionOnThisPole.ContainsKey(id))
        //    connectionOnThisPole.Add(id, newConnection);
    }
}

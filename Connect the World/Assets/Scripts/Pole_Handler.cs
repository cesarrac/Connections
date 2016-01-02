using UnityEngine;
using System.Collections;

public class Pole_Handler : MonoBehaviour {

    public Pole myPole;

    World_Generator world;

    int gridPosX;

    int maxConnections, curNumOfConnections = 0;

    LineRenderer curActiveLineR;

    public float connectorYPosition = 3.5f;

    bool isConnecting = false;

    Mouse_Controller mouse;

    void OnEnable()
    {
        gridPosX = Mathf.RoundToInt(transform.position.x);

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

    public void AddNewConnection()
    {
        if (curNumOfConnections < maxConnections)
        {
            // Spawn the connection
            GameObject lineRenderer = ObjectPool.instance.GetObjectForType("Connection", true, new Vector3(transform.position.x, connectorYPosition, 0));
            lineRenderer.transform.SetParent(transform, true);

            Activate(lineRenderer.GetComponent<LineRenderer>());
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
                if (curTile.tileType == TileType.CITY || curTile.tileType == TileType.POLE)
                {
                    isConnecting = false;
                    //lineR.SetPosition(1, world.GetCityFromTilePosX(curTile.posX).connectorPosition);
                }
            }
           
         
        }
    }
}

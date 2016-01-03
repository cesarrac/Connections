using UnityEngine;
using System.Collections;


public enum BuildMode { BUILD_POLE, DESTROY, NONE }
public class Mouse_Controller : MonoBehaviour {

    public static Mouse_Controller instance { get; protected set; }
    Vector3 curMousePosition;

    public Vector3 mousePosition { get; protected set; }

    Vector3 lastFramePosition = Vector3.zero;

    //float constantY = 4.3f;

    //float constantYBuildPosition = 1.2f;

    int gridWidth, gridHeight;

    World_Generator world;

   
    ConnectionMode connectionMode = ConnectionMode.INPUT;

    ConnectionType connectionType;

    BuildMode buildMode = BuildMode.NONE;

    public GameObject connectionModePanel, connectionTypePanel, mainMenuPanel;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        world = World_Generator.instance;

        gridWidth = world.gridWidth;
        gridHeight = world.gridHeight;

    }

    void Update()
    {
        TrackMousePosition();

        TrackMiddleClickDrag();

        lastFramePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lastFramePosition.z = 0;

        if (Input.GetMouseButtonDown(1))
        {
            Tile curTile = GetTileUnderMouse();

            if (curTile != null)
            {
                if (buildMode == BuildMode.BUILD_POLE)
                {
                    if (curTile.tileType == TileType.EMPTY)
                    {
                        PlacePole();
                    }
                    else if (curTile.tileType == TileType.POLE)
                    {
                        ActivatePole(curTile.posX);
                    }
                }
                else if (buildMode == BuildMode.NONE)
                {
                   if (curTile.tileType == TileType.CITY)
                    {
                        Cities_Manager.instance.DisplayCityInfo(curTile.posX);
                        // Center the camera on this city that we are displaying info for
                        //Vector3 center = lastFramePosition - curMousePosition;
                        //center.y = 0;
                        //Camera.main.transform.Translate(center);
                    }

                }
                else
                {
                    // DESTROY Poles: ( By pooling them )
                    if (curTile.tileType == TileType.POLE)
                    {
                     
                        if (world.GameObjectFromTileXCoord(curTile.posX) != null)
                            ObjectPool.instance.PoolObject(world.GameObjectFromTileXCoord(curTile.posX));

                        // Remove from tiles grid
                        world.RemoveFromGrid(curTile.posX);

                    }

                    // Always after attempting any Destruction, reset the build mode to None
                    buildMode = BuildMode.NONE;
                }

            }
            else
            {
                Debug.Log("MOUSE: Can't place a pole! There's NO TILE! Are you checking a negative coordinate?");
            }

        }
       

        mousePosition = curMousePosition;
  
    }

    void TrackMousePosition()
    {
        curMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        curMousePosition.z = 0;

    }

    void TrackMiddleClickDrag()
    {
 
        if (Input.GetMouseButton(2) && lastFramePosition != Vector3.zero)
        {
            Vector3 diff = lastFramePosition - curMousePosition;
            diff.y = 0;
            Camera.main.transform.Translate(diff);
        }
    }

    void PlacePole()
    {
        // Check with the Money Manager to see if this purchase can be made
        if (Money_Manager.instance.Purchase("Pole"))
        {
            int x = Mathf.RoundToInt(curMousePosition.x);
            world.CreatePole(x);
            // POLE HAS BEEN PLACED!
        }
    



    }


    public Tile GetTileUnderMouse()
    {
        int x = Mathf.RoundToInt(curMousePosition.x);
        int y = Mathf.RoundToInt(curMousePosition.y);
        if (x < gridWidth && x > 0 && y < gridHeight && y > 0)
        {
            Tile currTile = world.TileFromWorldPoint(curMousePosition);
            Debug.Log("MOUSE: Tile under mouse is of type " + currTile.tileType + " its coords are: " + currTile.posX + " " + currTile.posY);
            Debug.Log("MOUSE: the current mouse position is " + curMousePosition);

            return currTile;
        }

        return null;
    }

    // There are TWO BUILD MODES: Build Pole, Destroy or None (for not building)
    public void ChangeToBuildMode(string mode)
    {
        switch (mode)
        {
            case "Build Pole":
                buildMode = BuildMode.BUILD_POLE;
                DisplayConnectionModeOptions();
                break;
            case "Destroy":
                buildMode = BuildMode.DESTROY;
                break;
            case "None":
                buildMode = BuildMode.NONE;
                DisplayMainMenu();
                break;
            default:
                buildMode = BuildMode.NONE;
                break;
        }
        

    }


    // There are TWO CONNECTION MODES. Input and Output
    public void ChangeConnectionMode(string mode)
    {
        if (mode == "Input")
        {
            connectionMode = ConnectionMode.INPUT;
            DisplayConnectionTypeOptions();
        }
        else if (mode == "Output")
        {
            connectionMode = ConnectionMode.OUTPUT;
            DisplayConnectionTypeOptions();
        }
    }
   

    void DisplayMainMenu()
    {
        connectionTypePanel.SetActive(false);
        connectionModePanel.SetActive(false);

        mainMenuPanel.SetActive(true);
    }

    void DisplayConnectionTypeOptions()
    {
        connectionModePanel.SetActive(false);
        mainMenuPanel.SetActive(false);

        connectionTypePanel.SetActive(true);
    }

    void DisplayConnectionModeOptions()
    {
        connectionTypePanel.SetActive(false);
        mainMenuPanel.SetActive(false);

        connectionModePanel.SetActive(true);
    }

    public void ChangeConnectionType(string type)
    {
        switch (type)
        {
            case "Health":
                connectionType = ConnectionType.Health;
                break;
            case "Education":
                connectionType = ConnectionType.Education;
                break;
            case "Spirituality":
                connectionType = ConnectionType.Spirituality;
                break;
            case "Entertainment":
                connectionType = ConnectionType.Entertainment;
                break;
            case "Technology":
                connectionType = ConnectionType.Technology;
                break;
            default:
                Debug.Log("MOUSE: Can't find that type of connection! Did you pass in the wrong name in the button parameter?");
                break;

        }

        // After a type selection has been made, go back to displaying the connection mode options
        DisplayConnectionModeOptions();
    }

    // By activating the Pole it is starting a connection
    void ActivatePole(int x)
    {
        if (world.GameObjectFromTileXCoord(x).GetComponent<Pole_Handler>() != null)
        {
            world.GameObjectFromTileXCoord(x).GetComponent<Pole_Handler>().AddNewConnection(connectionMode, connectionType);
        }
           

    }


}

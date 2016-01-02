using UnityEngine;
using System.Collections;

public class Mouse_Controller : MonoBehaviour {

    public static Mouse_Controller instance { get; protected set; }
    Vector3 curMousePosition;

    public Vector3 mousePosition { get; protected set; }

    Vector3 lastFramePosition = Vector3.zero;

    float constantY = 4.3f;

    float constantYBuildPosition = 1.2f;

    int gridWidth, gridHeight;

    World_Generator world;

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
                if (curTile.tileType == TileType.EMPTY)
                {
                    PlacePole();
                }
                else if (curTile.tileType == TileType.POLE)
                {
                    ActivatePole(curTile.posX);
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
        int x = Mathf.RoundToInt(curMousePosition.x);
        world.CreatePole(x);

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


    void ActivatePole(int x)
    {
        if (world.GameObjectFromTileXCoord(x).GetComponent<Pole_Handler>() != null)
        {
            world.GameObjectFromTileXCoord(x).GetComponent<Pole_Handler>().AddNewConnection();
        }
           

    }


}

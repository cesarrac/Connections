using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World_Generator : MonoBehaviour {

    public int totalOfCities = 10;

    public float buildingYPosition = 0f;
    public float poleBuildingPos = -0.5f;

    public GameObject cityPreFab, polePrefab;

    public static World_Generator instance;

    Tile[,] grid;
    public int gridWidth = 50, gridHeight = 50;

    Dictionary<int, GameObject> worldGameObjects = new Dictionary<int, GameObject>();

    Dictionary<int, Pole> poles = new Dictionary<int, Pole>();

    Dictionary<int, City> cities = new Dictionary<int, City>();

    public Transform cityHolder, poleHolder;

    ObjectPool objPool;

    void Awake()
    {
        instance = this;

        InitEmptyGrid();
    }

    void InitEmptyGrid()
    {
        grid = new Tile[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new Tile(x, y, 1, 1, TileType.EMPTY);
            }
        }

        Debug.Log("WORLD: Grid initialized. Total tiles = " + (gridWidth * gridHeight));
    }

    void Start()
    {
        objPool = ObjectPool.instance;

        CreateCities();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
    }


    void CreateCities()
    {

        //int spriteWidth = Mathf.RoundToInt( cityPreFab.GetComponent<SpriteRenderer>().sprite.bounds.size.x );
        //int spriteHeight = Mathf.RoundToInt(cityPreFab.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

        // For now im hardcoding the initial city sprite width and height to be width  = 2 and height = 4 ( actual sprite size is 128 x 256, but the world right now is assuming one unit is 64 pixels)
        int spriteWidth = 2;
        int spriteHeight = 4;

        for (int i = 0, x = 5; i < totalOfCities; i++, x+=5)
        {
            City newCity = new City("City " + i, 500, i + x, spriteWidth, spriteHeight, new Vector3(i + x, buildingYPosition + 3.5f, 0));

            //GameObject cityGObj = Instantiate(cityPreFab, new Vector3(newCity.worldX, buildingYPosition, 0), Quaternion.identity) as GameObject;

            GameObject cityGObj = objPool.GetObjectForType("city", true, new Vector3(newCity.worldX, buildingYPosition, 0));

            if (cityGObj == null)
            {
                Debug.LogError("City GameObject not found in pool! Did you forget to add it to pool? Does it not have enough prefab city preloaded??");
                return;
            }

            cityGObj.transform.SetParent(cityHolder, true);

            AddToGrid(i + x, spriteWidth, spriteHeight, TileType.CITY, cityGObj);

            // Add city to the cities dictionary, pointing to this instance of the City class with this X ( this X represents the base tile of this city, when a city is spawned its city handler script will ask to get the city using its X as the key)
            cities.Add(i + x, newCity);
            

            Debug.Log("Created a city called " + newCity.name + " with a population of " + newCity.population);
        }
    }

    void AddToGrid(int _x, int width, int height, TileType tileType, GameObject gObj)
    {
        for (int w = 0; w < width; w++)
        {
            // Add to dictionary to point all these x coords to the same GameObject
            if (!worldGameObjects.ContainsKey(_x + w))
            {
                worldGameObjects.Add(_x + w, gObj);
            }
           

            for (int h = 0; h < height; h++)
            {
                grid[_x + w, h] = new Tile(_x + w, h, width, height, tileType);
                
                Debug.Log("Added " + tileType + " to grid. At: " + (_x + w) + " " + h);
            }
        }
    }



    public void CreatePole(int x)
    {
        if (!poles.ContainsKey(x))
        {
            Pole newPole = new Pole(x, 5);

            //GameObject pole = Instantiate(polePrefab, new Vector3(newPole.posX, poleBuildingPos, 0), Quaternion.identity) as GameObject;

            GameObject pole = objPool.GetObjectForType("Pole", true, new Vector3(newPole.posX, poleBuildingPos, 0));

            if (pole == null)
            {
                Debug.LogError("Could not find Pole gameObject in the Pool! Did you forget to preload them?? Are we running out??");
                return;
            }

            pole.transform.SetParent(poleHolder, true);

            AddToGrid(x, 1, 5, TileType.POLE, pole);

            poles.Add(x, newPole);

        }
        else
        {
            Debug.Log("WORLD: There's a Pole already placed at that X coord!");
        }

    }


    public Tile TileFromWorldPoint(Vector3 worldPos)
    {
        //Vector3 worldBottomLeft = transform.position - Vector3.right * gridWidth / 2 - Vector3.up * gridHeight / 2;
        //Vector3 worldPoint = worldBottomLeft + Vector3.right * worldPos.x + Vector3.up * worldPos.y;

        int x = Mathf.RoundToInt(worldPos.x);
        int y = Mathf.RoundToInt(worldPos.y);

        return grid[x, y];
    }

    public GameObject GameObjectFromTileXCoord (int x)
    {
        if (worldGameObjects.ContainsKey(x))
        {
            return worldGameObjects[x];
        }
        else
            return null;
  
    }


    public Pole GetPoleAtLocationX(int x)
    {
        if (poles.ContainsKey(x))
        {
            return poles[x];
        }
        else
            return null;
    }

    public Pole GetPoleFromTilePosX(int tilePosX)
    {
        int x = Mathf.RoundToInt(GameObjectFromTileXCoord(tilePosX).transform.position.x);

        return GetPoleAtLocationX(x);
    }

    public City GetCityAtLocationX (int x)
    {
        if (cities.ContainsKey(x))
        {
            return cities[x];
        }
        else
            return null;
    }

    public City GetCityFromTilePosX(int tilePosX)
    {
        int x = Mathf.RoundToInt(GameObjectFromTileXCoord(tilePosX).transform.position.x);

        return GetCityAtLocationX(x);
    }

}

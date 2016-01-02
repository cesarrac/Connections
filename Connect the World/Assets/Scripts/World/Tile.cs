using UnityEngine;
using System.Collections;

public enum TileType
{
    EMPTY,
    POLE,
    CITY
}

public class Tile {

    public int posX, posY;

    int width, height;

    public TileType tileType = TileType.EMPTY;

    public Tile(int x, int y, int _width, int _height, TileType _type)
    {
        posX = x;
        posY = y;
        width = _width;
        height = _height;
        tileType = _type;
    }


}

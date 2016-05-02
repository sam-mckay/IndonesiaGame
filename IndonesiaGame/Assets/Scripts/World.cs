using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public HashSet<Coord> roomTiles;

    public void setRoomTiles(HashSet<Coord> _roomTiles)
    {
        roomTiles = _roomTiles;
    }

    
}

public struct Coord
{
    public int tileX, tileY;
    public Coord(int x, int y)
    {
            tileX = x;
            tileY = y;
    }

    public float Distance(Coord a, Coord b)
    {
        return Mathf.Sqrt(Mathf.Pow(b.tileX - a.tileX,2) + Mathf.Pow(b.tileY - a.tileY, 2)); 
    }


}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public HashSet<Coord> roomTiles;
    public List<Room> rooms;
    public int objectCount;
    public List<Coord> objectPositions;
    
    public GameObject[] objectList;
    public Queue<GameObject> objects;


    public void setRoomTiles(HashSet<Coord> _roomTiles, List<Room> validRooms)
    {
        roomTiles = _roomTiles;
        rooms = validRooms;
        objects = new Queue<GameObject>();

        foreach (GameObject obj in objectList)
        {
            objects.Enqueue(obj);
        }
        if (objectCount > 0)
        {
            placeObjects();
        }
        
    }

    void placeObjects()
    {
        foreach(Room room in rooms)
        {
            int randCoord = UnityEngine.Random.Range(0, rooms.Count);
            Coord currentCoord = room.tiles[randCoord];
            createNewObject(currentCoord);
            objectCount--;
            if(objectCount == 0)
            {
                return;
            }
        }
        if(objectCount != 0)
        {
            placeObjects();
        }
    }

    void createNewObject(Coord position)
    {
        GameObject newObject = (GameObject)Instantiate(objects.Dequeue(),new Vector3(position.tileY, -4, position.tileY), Quaternion.identity);
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
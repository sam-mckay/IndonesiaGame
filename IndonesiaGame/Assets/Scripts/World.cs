using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public HashSet<Coord> roomTiles;
    public List<Room> rooms;
    public int objectCount;
    public LinkedList<Coord> objectPositions;
    
    public GameObject[] objectList;
    public GameObject container;
    public Queue<GameObject> objects;
    public List<string> objectSaveNameList;

    public Vector3 PlayerStartPos;
    

    Vector3 FindStartTile()
    {
        Vector3 startTile = Vector3.zero;

        Room mainRoom = new Room();

        // get a random start pos
        for(int i = 0; i < rooms.Count; ++i)
        {
            if(rooms[i].isMainRoom)
            {
                mainRoom = rooms[i];
                break;
            }
        }

        Coord randCoord = GetRandomCoordInRoom(mainRoom);

        startTile = new Vector3(randCoord.tileX, 0, randCoord.tileY);

        return startTile;
    }

    public void setRoomTiles(HashSet<Coord> _roomTiles, List<Room> validRooms)
    {
        objectSaveNameList = new List<string>();
        roomTiles = _roomTiles;
        rooms = validRooms;
        objectPositions = new LinkedList<Coord>();
        objects = new Queue<GameObject>();

        //DEBUG FEATURE
        /*
        int[,] map = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().map;
        foreach (Room room in rooms)
        {
            foreach (Coord tile in room.tiles)
            {
                Debug.Log("ADDED: " + tile.tileX + "," + tile.tileY + " MAP: " + map[tile.tileX, tile.tileY]);
            }
        }
        */
        //
            foreach (GameObject obj in objectList)
        {
            objects.Enqueue(obj);
            objectCount++;
        }
        if (objectCount > 0)
        {
            placeObjects();
        }

        PlayerStartPos = FindStartTile();

        PlayerStartPos.y -= 3.5f;

        GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
        player.transform.position = PlayerStartPos;

        Vector3 oldCamPos = Camera.main.transform.position;

        

        Camera.main.transform.position = new Vector3(PlayerStartPos.x + 10, oldCamPos.y, PlayerStartPos.z + 10);
        Camera.main.transform.LookAt(new Vector3(PlayerStartPos.x + 2, 0, PlayerStartPos.z + 2));
    }

    Coord GetRandomCoordInRoom(Room room)
    {
        int randCoord = UnityEngine.Random.Range(0, rooms.Count);
        return room.tiles[randCoord];
    }

    void placeObjects()
    {
        foreach(Room room in rooms)
        {
            Coord currentCoord = GetRandomCoordInRoom(room);
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
        GameObject newObject = (GameObject)Instantiate(objects.Dequeue(),new Vector3(position.tileX, -4, position.tileY), Quaternion.identity);
        GameObject newObjectContainer = (GameObject)Instantiate(container, new Vector3(position.tileX, -4, position.tileY), Quaternion.identity);
        newObject.transform.parent = newObjectContainer.transform;
        objectPositions.AddLast(position);
        int[,] map = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().map;
        //Debug.Log("ADDED: " + position.tileX + "," + position.tileY+ " MAP: "+map[position.tileX, position.tileY]);
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
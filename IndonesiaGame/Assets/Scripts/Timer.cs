using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
    bool timerInitialised = false;
    public Text timerText;
    float timeRemaining;
	// Use this for initialization
	public void initTimer ()
    {
        World world = GameObject.FindGameObjectWithTag(Tags.mainCam).GetComponent<World>();
        LinkedList<Coord> objectCoords = world.objectPositions;
        if (objectCoords.First != null)
        {
            LinkedListNode<Coord> currentCoord = objectCoords.First;
            int mapWidth = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().width;
            int mapHeight = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().height;
            int[,] map = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().map;
            PathFinder pathing = new PathFinder();
            float totalDistance = 0.0f;

            while (currentCoord.Next != null)
            {
                totalDistance += pathing.GridPathfind(getCoordVec2(currentCoord.Value), getCoordVec2(currentCoord.Next.Value), mapWidth, mapHeight, map);
                currentCoord = currentCoord.Next;
            }

            timeRemaining = totalDistance + 30.0f;
            timerInitialised = true;
        }
    }

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(timerInitialised)
        {
            timeRemaining -= Time.deltaTime;
            if(timeRemaining < 0)
            {
                //GAME OVER CODE

            }
            timerText.text = timeRemaining.ToString();
        }
	}

    Vector2 getCoordVec2(Coord coord)
    {
        return new Vector2(coord.tileX, coord.tileY);
    }
}

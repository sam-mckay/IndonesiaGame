using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalPowerUpManager : MonoBehaviour
{
    public GameObject PowerUp;
    public GameObject roomSquare;

    public int minimumDistance;
    public int totalPowerUps;
    public float bestObjectDistance;

    HashSet<Coord> roomTiles;
    HashSet<Coord> powerUpLocations;
    HashSet<int> usedColumns = new HashSet<int>();
    HashSet<int> usedRows = new HashSet<int>();

    float width, height;

    // Use this for initialization
    void Start ()
    {
        
        width = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().width;
        height = GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().height;
        roomTiles = GameObject.FindGameObjectWithTag(Tags.mainCam).GetComponent<World>().roomTiles;
        //blockPlace();
        bestObjectDistance = getBestObjectDistance();
        powerUpLocations = new HashSet<Coord>();
        usedColumns = new HashSet<int>();
        usedRows = new HashSet<int>();

        randomGen();
	}

    void randomGen()
    {
        int powerUpsPlaced = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                powerUpsPlaced = powerUpLocations.Count;
                Coord potentialPos = new Coord(x, y);
                if (roomTiles.Contains(potentialPos) && powerUpsPlaced != totalPowerUps)
                {
                    if (powerUpsPlaced > 0 && !usedRows.Contains(x) && !usedColumns.Contains(y) && isGoodProximity(potentialPos))
                    {
                        placePowerUp(potentialPos);   
                    }
                    else if (powerUpsPlaced == 0)
                    {
                        placePowerUp(potentialPos);
                    }
                }
            }
        }        
    }

    void placePowerUp(Coord newLocation)
    {
        GameObject newPowerUp = (GameObject)Instantiate(PowerUp, new Vector3(newLocation.tileX, -2.7f, newLocation.tileY), Quaternion.identity);
        
        int powerUpType = Random.Range(0, 5);

        switch (powerUpType)
        {
            default:
                break;
            case 0:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Distance(newPowerUp.transform.position, 10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
            case 1:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Width(newPowerUp.transform.position, 10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
            case 2:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Brightness(newPowerUp.transform.position, 10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
            case 3:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Distance(newPowerUp.transform.position, -10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
            case 4:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Width(newPowerUp.transform.position, -10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
            case 5:
                newPowerUp.GetComponent<PowerUp_Controller>().powerUp = new PU_Brightness(newPowerUp.transform.position, -10.0f, newPowerUp.GetComponent<PowerUp_Controller>());
                break;
        }
        
        powerUpLocations.Add(newLocation);
        usedRows.Add(newLocation.tileX);
        usedColumns.Add(newLocation.tileY);
        Debug.Log("ADDED: " + newLocation.tileX + "," + newLocation.tileY);
    }

    float getBestObjectDistance()
    {
        

        float surfaceArea = (width*height ) * GameObject.FindGameObjectWithTag(Tags.mapGen).GetComponent<MapGenerator>().randomFillPercent;

        return surfaceArea - 15;
    }

    bool isValidLocation(GameObject powerUp)
    {
        //check power up location is not in a wall
        Vector3 elevatedTransformPos = powerUp.transform.position;
        elevatedTransformPos.y += 20.0f;
        RaycastHit hit = new RaycastHit();
        Ray ray= new Ray(elevatedTransformPos, -powerUp.transform.up);
        
        if ((Physics.Raycast(ray, out hit, 100.0f)))
        {
            //Debug.Log("HIT SOMETHING: " + hit.collider.name);
            if (hit.collider.tag == Tags.caveRoof)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.red, 10.0f);
                return false;
            }
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.blue, 10.0f);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green, 10.0f);
        }
        return true;
    }

    bool isGoodProximity(Coord suggestedPosition)
    {
        foreach (Coord otherCoord in powerUpLocations)
        {
            float distance = suggestedPosition.Distance(suggestedPosition, otherCoord);
            if(distance < minimumDistance)
            {
                return false;
            }
        }
        return true;
    }

    //debug
    void blockPlace()
    {
        foreach(Coord coord in roomTiles)
        {
            Instantiate(roomSquare, new Vector3(coord.tileX, -3, coord.tileY), Quaternion.identity);
        }
    }
}

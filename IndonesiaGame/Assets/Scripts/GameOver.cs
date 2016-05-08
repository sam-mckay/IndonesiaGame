using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        if (PlayerPrefs.GetInt(SaveManager.gameWon) == 1)
        {
            this.GetComponent<Text>().text = "Congratulations! You Win!";
        }
        else
        {
            this.GetComponent<Text>().text = "Game Over! You Failed to Find all the Objects in Time";
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

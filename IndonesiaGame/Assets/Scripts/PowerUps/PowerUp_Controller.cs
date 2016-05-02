using UnityEngine;
using System.Collections;

public class PowerUp_Controller : MonoBehaviour
{
    public PowerUp powerUp;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        powerUp.OnTriggerEnter(other);
    }

    public void removePowerUp()
    {
        Destroy(this.gameObject); 
    }
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Camera gameCamera;
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);
        gameCamera = GetComponent<Camera>();

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(this.transform.position, player.transform.position) > 10)
        {
            //transform.Translate(player.transform.position / 10);
            moveCamera(true);
        }
        else if(Vector3.Distance(this.transform.position, player.transform.position) < 5)
        {
            //transform.Translate(- (player.transform.position / 10));
            moveCamera(false);
        }

        gameCamera.transform.LookAt(player.transform);

        if (Input.GetKey(KeyCode.A))
        {
            rotateAround(true, Vector3.up);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotateAround(false, Vector3.up);
        }

    }

    void moveCamera(bool towards)
    {
        if(towards)
        {
            Vector3 newPosition = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            transform.position += newPosition / 10;
        }
        else if (!towards)
        {
            Vector3 newPosition = new Vector3(this.transform.forward.x, 0, this.transform.forward.z);
            transform.position -= newPosition / 10;
            
        }
    }

    void rotateAround(bool clockwise, Vector3 axis)
    {
        if (clockwise)
        {
            this.transform.RotateAround(player.transform.position, axis, 3.0f);
        }
        else if (!clockwise)
        {
            this.transform.RotateAround(player.transform.position, axis, -3.0f);
        }
    }
}

using UnityEngine;
using System.Collections;

public class ObjectFoundZoom : MonoBehaviour {

    public bool objectFound = false;
    private bool objectFoundZoom = false;
    private bool objectFoundRotate = false;

    Vector3 startPos;

    private float aps = 1.0f;


    private Vector3 LerpToPosition(Vector3 pos, float speed, float time)
    {
        return Vector3.Lerp(transform.position, pos, speed * time);
    }

    IEnumerator RotateToAngle()
    {
        for(float i = 0.0f; i < 360.0f; i+= aps)
        {
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), aps);
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        string saveName = SaveManager.pickUpObject + "_" + this.name;
        GameObject.FindGameObjectWithTag(Tags.mainCam).GetComponent<World>().objectSaveNameList.Add(saveName);
        PlayerPrefs.SetInt(saveName, 1);
        DestroyImmediate(transform.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        startPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(objectFound)
        {
            if (!objectFoundZoom)
            {
                if (Vector3.Distance(transform.position, Camera.main.transform.position + 2.0f * Camera.main.transform.forward) > 2.0f)
                    transform.position = LerpToPosition(Camera.main.transform.position + 2.0f * Camera.main.transform.forward, 1.0f, Time.deltaTime);
                else
                    objectFoundZoom = true;
            }
            else if (objectFoundZoom)
            {
                if(!objectFoundRotate)
                {
                    StartCoroutine("RotateToAngle");
                    objectFoundRotate = true;
                }
            }
        }
        

                
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTERED");
        if(other.tag == Tags.player)
        {
            Debug.Log("PLAYER ENTERED");
            objectFound = true;
        }
    }
}

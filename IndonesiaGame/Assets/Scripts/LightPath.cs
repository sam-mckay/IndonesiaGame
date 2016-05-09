using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightPath : MonoBehaviour
{
    public static List<Vector3> PathMousePositions = new List<Vector3>();
    public GameObject groundPlane;
    Vector3 oldMousePos;
    bool getNewPosition = true;
    public Material mat;
    bool isFirstVertex = true;

    public static Dictionary<Vector3, GameObject> Spotlights = new Dictionary<Vector3, GameObject>();

    // FPS
    float DeltaTime = 0;
    int SecondCounter = 0;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        World world = GameObject.FindGameObjectWithTag(Tags.mainCam).GetComponent<World>();

        if(!world.HasGameStarted)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            PathMousePositions.Clear();
        }

        if (Input.GetMouseButton(0))
        {
            // FPS calculations----------------------------------------------
            DeltaTime += (Time.deltaTime - DeltaTime) * 0.1f;

            float FPS = 1f / DeltaTime;

            // frequency of sampling the mouse position
            if (SecondCounter > 0.01f * FPS) // 10 milliseconds passed
            {
                getNewPosition = true;

                SecondCounter = 0;
            }
            else
            {
                getNewPosition = false;
            }

            SecondCounter++;

            if (getNewPosition)
            {
                Vector3 newPos = Vector3.zero;
                Vector3 normalAtHit = Vector3.zero;

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    newPos = hit.point;
                    normalAtHit = hit.normal;
                }

                if (oldMousePos != newPos)
                {
                    // if we're not trying to trace on a roof
                    bool isHittingRoof = newPos.y > 0.1f && normalAtHit == Vector3.up;

                    if (!isHittingRoof)
                    {
                        // if we're hitting the side of the wall
                        if(newPos.y > 0.1f)
                        {
                            // move the point a small distance away from the wall in the direction of the normal
                            // so the path does not cause the character to move through the wall
                            newPos += normalAtHit;
                        }

                        oldMousePos = newPos;
                        newPos.y = groundPlane.transform.position.y + 0.05f;
                        PathMousePositions.Add(newPos);

                        // don't double the first vertex, otherwise we're not drawing anything (it will draw from its position to itself)
                        if (isFirstVertex)
                        {
                            isFirstVertex = false;
                        }
                        else
                        {
                            // double up the vertex to create a continuous line
                            PathMousePositions.Add(newPos);
                        }
                    }
                    else
                    {
                        Debug.Log("Hit roof!");
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // remove last one so we don't draw the next in continuation
            PathMousePositions.RemoveAt(PathMousePositions.Count - 1);
            isFirstVertex = true;

            PathMovement.HasReachedTarget = false;
            PathMovement.AllowedToMove = true;

            PathMovement.PathfinderIndices = 0;

            if (PathMovement.PathfinderIndices < PathMousePositions.Count - 1)
            {
                Vector3 adjustedTarget = PathMousePositions[PathMovement.PathfinderIndices];
                // this value is supposed to be half the height of the character
                adjustedTarget.y = -3.5f;
                PathMovement.TargetLocation = adjustedTarget;
            }

            // add light

            // spotlight version 1
            //GameObject playerLight = PlayerCharacter.transform.GetChild(0).gameObject;
            //playerLight.SetActive(true);

            //Vector3 newLightPos = PlayerCharacter.transform.position;
            //newLightPos.y = 2.5f;
            //playerLight.transform.position = newLightPos;

            //playerLight.transform.LookAt(PathMousePositions[PathMousePositions.Count - 1]);
            //------------------------

            // spotlight version 2
            for(int i = 0; i < 5; ++i)
            {
                if(PathMousePositions.Count < 2)
                {
                    return;
                }

                Vector3 key = PathMousePositions[PathMousePositions.Count / 5 * i];
                key.y = -3.5f;

                if (!Spotlights.ContainsKey(key))
                {
                    GameObject lightGameObject = new GameObject(string.Format("Spotlight{0}", i));
                    Light lightComp = lightGameObject.AddComponent<Light>();
                    lightComp.type = LightType.Spot;
                    lightComp.color = Color.white;
                    //lightComp.intensity = 3;
                    lightComp.range = 10f;

                    Vector3 pos = PathMousePositions[PathMousePositions.Count / 5 * i];
                    pos.y = 0;
                    lightGameObject.transform.position = pos;
                    lightGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

                    Spotlights.Add(key, lightGameObject);
                }
            }

            // add the last one at the end
            //GameObject lastLightGameObject = new GameObject("Spotlight5");
            //Light lastLightComp = lastLightGameObject.AddComponent<Light>();
            //lastLightComp.type = LightType.Spot;
            //lastLightComp.color = Color.white;
            ////lastLightComp.intensity = 3;
            //lastLightComp.range = 10f;
            //Vector3 lastKey = PathMousePositions[PathMousePositions.Count - 2];
            //lastKey.y = 1;
            //Vector3 lastPos = PathMousePositions[PathMousePositions.Count - 2];
            //lastPos.y = 4;
            //lastLightGameObject.transform.position = lastPos;
            //lastLightGameObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            //Spotlights.Add(lastKey, lastLightGameObject);
            //-----------------------
        }

    }

    void OnPostRender()
    {
        // now draw the lines using the positions above
        if (PathMousePositions.Count > 1)
        {
            GL.Begin(GL.LINES);
            mat.SetPass(0);
            GL.Color(Color.red);
            for (int i = 0; i < PathMousePositions.Count; ++i)
            {
                GL.Vertex(PathMousePositions[i]);
            }
            GL.End();
        }
    }
}

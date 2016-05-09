using UnityEngine;
using System.Collections;

public class PathMovement : MonoBehaviour
{
    public static Vector3 TargetLocation;
    float MovementSpeed = 3f;
    public static bool HasReachedTarget = true;

    public static int PathfinderIndices;
    public static bool AllowedToMove = false;

    // Use this for initialization
    void Start ()
    {
	
	}

    public void SetTargetLocation(Vector3 target)
    {
        if (target == TargetLocation)
        {
            return;
        }

        HasReachedTarget = false;

        PathfinderIndices = 0;

        if (PathfinderIndices < LightPath.PathMousePositions.Count - 1)
        {
            SetAdjustedTargetLocation(LightPath.PathMousePositions[PathfinderIndices]);
        }
    }

    void SetAdjustedTargetLocation(Vector3 location)
    {
        Vector3 adjustedTarget = location;
        // this value is supposed to be half the height of the character
        adjustedTarget.y = -3.5f;
        TargetLocation = adjustedTarget;
    }

    void SetIntermediateTarget(Vector3 intermediateTarget)
    {
        SetAdjustedTargetLocation(intermediateTarget);
    }

    // Update is called once per frame
    void Update ()
    {
	    if(LightPath.PathMousePositions.Count > 0 && AllowedToMove)
        {
            GameObject playerLight = transform.GetChild(0).gameObject;

            if (PathfinderIndices == LightPath.PathMousePositions.Count)
            {
                //playerLight.SetActive(false);
                LightPath.PathMousePositions.Clear();
                AllowedToMove = false;
                PathfinderIndices = 0;
                return;
            }

            if (HasReachedTarget)
            {
                if (LightPath.Spotlights.ContainsKey(TargetLocation))
                {
                    LightPath.Spotlights[TargetLocation].SetActive(false);
                }

                if (PathfinderIndices < LightPath.PathMousePositions.Count)
                {

                    SetIntermediateTarget(LightPath.PathMousePositions[PathfinderIndices]);
                    HasReachedTarget = false;
                }
            }

            float distToTarget = Vector3.Distance(TargetLocation, transform.position);
            float movementSpeedThisFrame = MovementSpeed * Time.deltaTime;
            // if distance to target < speed, m_pos = target
            if (distToTarget > movementSpeedThisFrame)
            {
                Vector3 dirToTarget = (TargetLocation - transform.position).normalized;
                transform.position += dirToTarget * movementSpeedThisFrame;
                // printf("moving to: %f, %f \n", XMVectorGetX(m_Position), XMVectorGetY(m_Position));
            }
            else
            {
                transform.position = TargetLocation;
                HasReachedTarget = true;
                PathfinderIndices++;
            }

            //SM: added player rotation to face direction of travel
            this.transform.LookAt(TargetLocation);

            // spotlights version 1
            //playerLight.SetActive(true);

            //Vector3 newLightPos = transform.position;
            //newLightPos.y = 2.5f;
            //playerLight.transform.position = newLightPos;

            //playerLight.transform.LookAt(LightPath.PathMousePositions[LightPath.PathMousePositions.Count - 1]);
            // ---------------------

            // spotlights version 2

        }
	}
}

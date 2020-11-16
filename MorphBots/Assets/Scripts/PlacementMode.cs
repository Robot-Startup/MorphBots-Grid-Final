using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementMode : MonoBehaviour
{
    public float maxRaycastDistance;

    public GameObject morphBotRef;

    public LayerMask gameLayers;

    Ray morphBotRay;
    RaycastHit morphBotRayHit;

    Vector3 mousePosition;
    Vector3 truePosition;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            morphBotRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(morphBotRay, out morphBotRayHit, maxRaycastDistance, gameLayers))
            {
                mousePosition = morphBotRayHit.point - morphBotRayHit.transform.position;

                if (mousePosition.x > 0)
                {
                    truePosition.x = Mathf.Floor(mousePosition.x + 0.5f);
                }

                else if (mousePosition.x < 0)
                {
                    truePosition.x = Mathf.Floor(mousePosition.x * -1 + 0.5f) * -1;
                }

                if (mousePosition.y > 0)
                {
                    truePosition.y = Mathf.Floor(mousePosition.y + 0.5f);
                }

                else if (mousePosition.y < 0)
                {
                    truePosition.y = Mathf.Floor(mousePosition.y * -1 + 0.5f) * -1;
                }
                if (mousePosition.z > 0)
                {
                    truePosition.z = Mathf.Floor(mousePosition.z + 0.5f);
                }

                else if (mousePosition.z < 0)
                {
                    truePosition.z = Mathf.Floor(mousePosition.z * -1 + 0.5f) * -1;
                }

                Instantiate(morphBotRef, truePosition + morphBotRayHit.transform.position, Quaternion.identity);
            }
        }
    }
}
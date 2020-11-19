using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    // the maximum distance the proceeding Raycasts will utilize
    public float maxRaycastDistance;

    // a reference to the Highlighter prefab
    public GameObject highlighterRef;

    // a mixed LayerMask composing of the MorphBot layer and Platform layer
    public LayerMask gameLayers;
    // a single LayerMask composed of only the Platform layer
    public LayerMask platformLayer;

    // a spawned instance of the Highlighter prefab
    GameObject highlighter;

    // a Ray between the main camera and where the mouse is pointing
    Ray highlightRay;
    // stores information from a Raycast if collision is detected
    RaycastHit highlightRayHit;

    // a modified value of the location that the mouse hit in-game
    Vector3 hitPoint;
    // similar to hitPoint, but has been further altered by CalculateTruePos
    Vector3 truePosition;

    // void function that alters the status of highlighter
    public void UpdateVisibility(bool isVisible)
    {
        // activates or deactivates highlighter based on isVisible
        highlighter.SetActive(isVisible);
    }

    // Vector3 function that updates and returns truePosition
    public Vector3 CalculateTruePos(Vector3 position)
    {
        // runs if the x value of position is greater than 0
        if (position.x > 0)
        {
            /* snaps position.x to a grid and sets the value to truePosition.x
               for example, 0.5 --> 1.0 and 0.3 --> 0 */
            truePosition.x = Mathf.Floor(position.x + 0.5f);
        }

        // if x value of position was not greater than 0, it checks to see if it was less than 0 instead
        else if (position.x < 0)
        {
            /* snaps position.x to a grid and sets the value to truePosition.x
               for example, -0.5 --> -1.0 and -2.4 --> -2.0 */
            truePosition.x = Mathf.Floor(position.x * -1 + 0.5f) * -1;
        }

        // repeats the same steps as above, but for position.y instead
        if (position.y > 0)
        {
            truePosition.y = Mathf.Floor(position.y + 0.5f);
        }

        else if (position.y < 0)
        {
            truePosition.y = Mathf.Floor(position.y * -1 + 0.5f) * -1;
        }

        // repeats the same steps as above, but for position.z instead
        if (position.z > 0)
        {
            truePosition.z = Mathf.Floor(position.z + 0.5f);
        }

        else if (position.z < 0)
        {
            truePosition.z = Mathf.Floor(position.z * -1 + 0.5f) * -1;
        }

        // returns truePosition
        return truePosition;
    }
    
    // runs at the beginning of the game
    private void Start()
    {
        /* spawns a GameObject that references the highlighterRef prefab that is placed at the origin and has a rotation of 0 (Quaternion.identity)
           highlighter is then set to this GameObject so that it can be accessed later during the game */
        highlighter = Instantiate(highlighterRef, new Vector3(0, 0, 0), Quaternion.identity);
        // deactivates highlighter by calling the UpdateVisibility function
        UpdateVisibility(false);
    }
    
    // runs every frame
    private void Update()
    {
        highlightRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(highlightRay, out highlightRayHit, maxRaycastDistance, gameLayers))
        {
            if (highlighter.activeSelf != true)
            {
                UpdateVisibility(true);
            }

            if (highlightRayHit.transform.gameObject.layer == 8)
            {
                hitPoint = CalculateTruePos(highlightRayHit.point - highlightRayHit.transform.position);

                if (hitPoint.x == 1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) - new Vector3(0.499f, 0, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, -90);
                }

                else if (hitPoint.x == -1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) + new Vector3(0.499f, 0, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 90);
                }

                if (hitPoint.y == 1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) - new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 0);
                }

                else if (hitPoint.y == -1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) + new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 180);
                }

                if (hitPoint.z == 1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) - new Vector3(0, 0, 0.499f);
                    highlighter.transform.eulerAngles = new Vector3(90, 0, 0);
                }

                else if (hitPoint.z == -1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) + new Vector3(0, 0, 0.499f);
                    highlighter.transform.eulerAngles = new Vector3(-90, 0, 0);
                }
            }

            else if (highlightRayHit.transform.gameObject.layer == 9)
            {
                if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.down), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition - new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 0);
                }

                else if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.up), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition + new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 180);
                }

                else if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.left), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition - new Vector3(0.499f, 0, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, -90);
                }

                else if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.right), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition + new Vector3(0.499f, 0, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 90);
                }

                else if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.back), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition - new Vector3(0, 0, 0.499f);
                    highlighter.transform.eulerAngles = new Vector3(90, 0, 0);
                }

                else if (Physics.Raycast(new Ray(CalculateTruePos(highlightRayHit.point), Vector3.forward), out highlightRayHit, maxRaycastDistance, platformLayer))
                {
                    highlighter.transform.position = truePosition + new Vector3(0, 0, 0.499f);
                    highlighter.transform.eulerAngles = new Vector3(-90, 0, 0);
                }
            }
        }
        
        else
        {
            if (highlighter.activeSelf != false)
            {
                UpdateVisibility(false);
            }
        }
    }
}
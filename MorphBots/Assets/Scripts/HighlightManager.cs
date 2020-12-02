using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    // The maximum distance the proceeding Raycasts will utilize
    public float maxRaycastDistance;

    // A reference to the Highlighter prefab
    public GameObject highlighterRef;

    // A mixed LayerMask composing of the MorphBot layer and Platform layer
    public LayerMask gameLayers;
    // A single LayerMask composed of only the Platform layer
    public LayerMask platformLayer;

    // A spawned instance of the Highlighter prefab
    GameObject highlighter;

    // A Ray between the main camera and where the mouse is pointing
    Ray highlightRay;
    // Stores information from a Raycast if collision is detected
    RaycastHit highlightRayHit;

    // A modified value of the location that the mouse hit in-game
    Vector3 hitPoint;
    // Similar to hitPoint, but has been further altered by CalculateTruePos
    Vector3 truePosition;

    // Void function that alters the status of highlighter
    public void UpdateVisibility(bool isVisible)
    {
        // Activates or deactivates highlighter based on isVisible
        highlighter.SetActive(isVisible);
    }

    // Vector3 function that updates and returns truePosition
    public Vector3 CalculateTruePos(Vector3 position)
    {
        // Runs if the x value of position is greater than 0
        if (position.x > 0)
        {
            /* Snaps position.x to a grid and sets the value to truePosition.x
               For example, 0.5 --> 1.0 and 0.3 --> 0 */
            truePosition.x = Mathf.Floor(position.x + 0.5f);
        }

        // If x value of position was not greater than 0, it checks to see if it was less than 0 instead
        else if (position.x < 0)
        {
            /* Snaps position.x to a grid and sets the value to truePosition.x
               For example, -0.5 --> -1.0 and -2.4 --> -2.0 */
            truePosition.x = Mathf.Floor(position.x * -1 + 0.5f) * -1;
        }

        // Repeats the same steps as above, but for position.y instead
        if (position.y > 0)
        {
            truePosition.y = Mathf.Floor(position.y + 0.5f);
        }

        else if (position.y < 0)
        {
            truePosition.y = Mathf.Floor(position.y * -1 + 0.5f) * -1;
        }

        // Repeats the same steps as above, but for position.z instead
        if (position.z > 0)
        {
            truePosition.z = Mathf.Floor(position.z + 0.5f);
        }

        else if (position.z < 0)
        {
            truePosition.z = Mathf.Floor(position.z * -1 + 0.5f) * -1;
        }

        // Returns truePosition
        return truePosition;
    }
    
    // Runs at the beginning of the game
    private void Start()
    {
        /* Spawns a GameObject that references the highlighterRef prefab that is placed at the origin and has a rotation of 0 (Quaternion.identity)
           The highlighter is then set to this GameObject so that it can be accessed later during the game */
        highlighter = Instantiate(highlighterRef, new Vector3(0, 0, 0), Quaternion.identity);
        // Deactivates highlighter by calling the UpdateVisibility function
        UpdateVisibility(false);
    }
    
    // Runs every frame
    private void Update()
    {
        // Creates a line from the main camera to the position of the mouse and assigns the value to highlightRay
        highlightRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        /* Returns a boolean depending on whether highlightRay hit something with either layer from gameLayers, returning hit information
           to highlightRayHit if true */
        if (Physics.Raycast(highlightRay, out highlightRayHit, maxRaycastDistance, gameLayers))
        {
            // Runs if the highlighter GameObject is not active
            if (highlighter.activeSelf != true)
            {
                // References the UpdateVisibility script to activate highlighter
                UpdateVisibility(true);
            }

            // Asks destructionRayHit if the object that was hit by destructionRay has a layer of 8 (which is the layer that MorphBots use)
            if (highlightRayHit.transform.gameObject.layer == 8)
            {
                // References the CalculateTruePos function with the point of where the Ray hit subtracted by the position of the MorphBot that the ray hit
                hitPoint = CalculateTruePos(highlightRayHit.point - highlightRayHit.transform.position);

                /* Explanation of what this does: 
                   So far, if your camera interacts with a MorphBot that was placed during the game, you will subtract where your mouse is by the center position of the MorphBot.
                   This value is when modified by a bunch of floor functions in CalculateTruePos which snaps the value to a grid. Once this value is snapped, only one of
                   three axes (x, y, z) will have a value that is NOT zero. this value can either be 1 or -1, and the second we detect it, we know that any other of the 
                   six possibilities are redundant. The position of highlighter is then determined by adding the new hitPoint value to the position of the MorphBot we hit,
                   alongside a new rotation that I got by tinkering with the system, and subtracting or adding by 0.499f in an axis to make highlighter "attach" to the MorphBot. */

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

                else if (hitPoint.y == 1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) - new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 0);
                }

                else if (hitPoint.y == -1)
                {
                    highlighter.transform.position = (hitPoint + highlightRayHit.transform.position) + new Vector3(0, 0.499f, 0);
                    highlighter.transform.eulerAngles = new Vector3(0, 0, 180);
                }

                else if (hitPoint.z == 1)
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

            // If destructionRayHit did not interact with layer 8 (a MorphBot), the script checks to see if it interacted with layer 9 (the platform)
            else if (highlightRayHit.transform.gameObject.layer == 9)
            {

                /* Explanation of what this does:
                   Depending on whether you hit a MorphBot or the platform, the script will use a different system of finding out what position and rotation highlighter
                   will need. This script runs when we know we did not hit a MorphBot, and that we are hitting the platform. When this is confirmed, the script will use
                   the CalculateTruePos function to snap where your mouse is pointing to a grid. Once this is done, a ray is created from the snapped mouse point to all
                   6 directions by 1 unit. The script does this to find out in what direction the platform is relative to your mouse. There is only one possible direction
                   that the platform is facing, and once it is found out, a series of calculations are run to rotate and move highlighter. */

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
        
        // Runs if the script did not detect any collision against a MorphBot or the platform.
        else
        {   // Runs if highlighter is active
            if (highlighter.activeSelf != false)
            {
                // Deactivates highlighter
                UpdateVisibility(false);
            }
        }
    }
}
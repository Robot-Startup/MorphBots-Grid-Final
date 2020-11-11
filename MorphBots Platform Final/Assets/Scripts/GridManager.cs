using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    // Initialize variables whose values will be assigned in-game
        
        // A reference of dummyBotRef that will be placed in-game
        GameObject dummyBot;
        // A line which will be created every frame from the main camera to where the mouse is pointing
        Ray physicsRay;
        // Stores values about the point of hit if physicsRay interacted with something
        RaycastHit physicsRayHit;
        // Part of the calculation of where dummyBot should be if mouse cursor is on a morphBotRef
        Vector3 blockDifference;
        // The unchanged value of where the mouse is pointing at in a 3D space
        Vector3 mousePosition;
        // The altered value of mousePosition after running through a series of statements
        Vector3 truePosition;
        Ray physicsRay2;
        RaycastHit physicsRayHit2;

    // Initialize variables whose values will be assigned in the editor

    // Reference to the dummyBot prefab which will later be spawned in-game
    public GameObject dummyBotRef;
        // Reference to the morphBot prefab which will later be spawned in-game
        public GameObject morphBotRef;
        // The layer assigned to the platform and morphBot prefab
        public LayerMask gameLayer;
        public LayerMask platformLayer;
        // Where the dummyBot will initially spawn when the game is started
        public Vector3 startingPosition;
        // How far the ray between the main camera and mouse cursor will be able to go
        public float rayMaxDistance;

    private void Start()
    {
        // Spawn a reference of dummyBotRef at startingPosition with a rotation of (0, 0, 0) and assign the GameObject to dummyBot to be accessed in-game
        dummyBot = Instantiate(dummyBotRef, startingPosition, Quaternion.identity);
        dummyBot.SetActive(false);
    }

    private void Update()
    {
        // Creates a line, or ray from the main camera to where the mouse is pointing
        physicsRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Checks if physicsRay collides with anything in the world with the blockLayer tag(s), and returns a boolean value
        if (Physics.Raycast(physicsRay, out physicsRayHit, rayMaxDistance, gameLayer))
        {
            // Checks to see if dummyBot is not active
            if (dummyBot.activeSelf == false)
            {
                // Activates dummyBot (making it visible)
                dummyBot.SetActive(true);
            }

            /* Subtracts physicsRayHit.point (the 3D coordinate of the mouse) by the position of the object that it hit (the morphBot with the blockLayer tag)
            and assigns it to blockDifference */
            blockDifference = physicsRayHit.point - physicsRayHit.transform.position;

        // Runs a series of floor functions to snap each axis of blockDifference to a grid of 1

            // In this case, if the x value of blockDifference is positive, it will add 0.5 to the value and run a floor function (i.e 0.4 ---> 0 and 0.5 ---> 1)
            if (blockDifference.x > 0)
            {
                truePosition.x = Mathf.Floor(blockDifference.x + 0.5f);
            }
            /* In this case, if the x value of blockDifference is negative, it will make the value positive, add 0.5f, run a floor function, then return
            the value to negative (i.e -1.4 ---> -1 and -0.6 ---> -1) */
            else if (blockDifference.x < 0)
            {
                truePosition.x = Mathf.Floor(blockDifference.x * -1 + 0.5f) * -1;
            }
            // Repeats the same steps as above for a positive y value
            if (blockDifference.y > 0)
            {
                truePosition.y = Mathf.Floor(blockDifference.y + 0.5f);
            }
            // Repeats the same steps as above for a negative y value
            else if (blockDifference.y < 0)
            {
                truePosition.y = Mathf.Floor(blockDifference.y * -1 + 0.5f) * -1;
            }
            // Repeats the same steps as above for a positive z value
            if (blockDifference.z > 0)
            {
                truePosition.z = Mathf.Floor(blockDifference.z + 0.5f);
            }
            // Repeats the same steps as above for a negative z value
            else if (blockDifference.z < 0)
            {
                truePosition.z = Mathf.Floor(blockDifference.z * -1 + 0.5f) * -1;
            }

            // Adds the truePosition, derived from the above statements, to the position of the block the ray collided with, and sets this value to the position of dummyBot
            /*dummyBot.transform.position = physicsRayHit.transform.position + truePosition;
            dummyBot.transform.eulerAngles = truePosition;*/
            if (physicsRayHit.transform.gameObject.layer == 8)
            {
                if (truePosition.x == 1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(-0.499f, 0, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else if (truePosition.x == -1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(0.499f, 0, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                if (truePosition.y == 1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(0, -0.499f, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (truePosition.y == -1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(0, 0.499f, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                if (truePosition.z == 1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(0, 0, -0.499f);
                    dummyBot.transform.eulerAngles = new Vector3(90, 0, 0);
                }
                else if (truePosition.z == -1)
                {
                    dummyBot.transform.position = (physicsRayHit.transform.position + truePosition) + new Vector3(0, 0, 0.499f);
                    dummyBot.transform.eulerAngles = new Vector3(-90, 0, 0);
                }
            }
            else if (physicsRayHit.transform.gameObject.layer == 9)
            {
                if (Physics.Raycast(new Ray(truePosition, Vector3.down), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition - new Vector3(0, 0.499f, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else if (Physics.Raycast(new Ray(truePosition, Vector3.up), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition + new Vector3(0, 0.499f, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 180);
                }
                else if (Physics.Raycast(new Ray(truePosition, Vector3.right), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition + new Vector3(0.499f, 0, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, 90);
                }
                else if (Physics.Raycast(new Ray(truePosition, Vector3.left), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition - new Vector3(0.499f, 0, 0);
                    dummyBot.transform.eulerAngles = new Vector3(0, 0, -90);
                }
                else if (Physics.Raycast(new Ray(truePosition, Vector3.forward), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition + new Vector3(0, 0, 0.499f);
                    dummyBot.transform.eulerAngles = new Vector3(-90, 0, 0);
                }
                else if (Physics.Raycast(new Ray(truePosition, Vector3.back), out physicsRayHit2, 1, platformLayer))
                {
                    dummyBot.transform.position = truePosition - new Vector3(0, 0, 0.499f);
                    dummyBot.transform.eulerAngles = new Vector3(90, 0, 0);
                }
            }
            

            // Checks to see if the right mouse button was pressed
            if (Input.GetMouseButtonDown(1))
            {
                // Spawns a reference of morphBotRef at the current position of dummyBot with a rotation of (0, 0, 0)
                Instantiate(morphBotRef, physicsRayHit.transform.position + truePosition, Quaternion.identity);
            }
        }

        // Runs statement if no collision was detected against objects with the gameLayer mask(s)
        else
        {
            if (dummyBot.activeSelf == true)
            {
                dummyBot.SetActive(false);
            }
        }
    }
}
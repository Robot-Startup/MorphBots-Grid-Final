using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    // The maximum distance the proceeding Raycasts will utilize
    public float maxRaycastDistance;

    // A mixed LayerMask composing of the MorphBot layer and Platform layer
    public LayerMask gameLayers;

    // Determines if currentMorphBot can and should be destroyed
    bool canDestroy;

    // The current MorphBot that the script has detected
    GameObject currentMorphBot;

    // A Ray from the main camera to where the mouse is
    Ray destructionRay;
    // Stores information from a Raycast if collision against specified LayerMask is detected
    RaycastHit destructionRayHit;

    // Runs every frame
    private void Update()
    {
        // Creates a line from the main camera to the position of the mouse and assigns the value to destructionRay
        destructionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        /* Returns a boolean depending on whether destructionRay hit something with either layer from gameLayers, returning hit information
        to destructionRayHit if true */
        if (Physics.Raycast(destructionRay, out destructionRayHit, maxRaycastDistance, gameLayers))
        {
            // Asks destructionRayHit if the object that was hit by destructionRay has a layer of 8 (which is the layer that MorphBots use)
            if (destructionRayHit.transform.gameObject.layer == 8)
            {
                // Sets canDestroy to true (meaning that the hit MorphBot can be destroyed)
                canDestroy = true;
                // Sets currentMorphBot to the object that destructionRayHit stored
                currentMorphBot = destructionRayHit.transform.gameObject;
            }

            // Runs if the layer in gameLayer was not 8 (a MorphBot)
            else
            {
                // Runs if canDestroy is true
                if (canDestroy == true)
                {
                    // Sets canDestroy to false, telling the script that nothing can be destroyed
                    canDestroy = false;
                }
            }
        }

        // Runs if no collision between destructionRay and objects with gameLayers was detected
        else
        {
            // Runs if canDestroy is true
            if (canDestroy == true)
            {
                // Sets canDestroy to false, telling the script that nothing can be destroyed
                canDestroy = false;
            }
        }

        // Runs if (a) right mouse button is held down and (b) canDestroy is true
        if (Input.GetMouseButtonDown(1) && canDestroy == true)
        {
            // Destroys the GameObject associated with currentMorphBot
            Destroy(currentMorphBot);
        }
    }
}
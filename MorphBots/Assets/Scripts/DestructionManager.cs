using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    // the maximum distance the proceeding raycasts will use
    public float maxRaycastDistance;

    // the layers the proceeding raycasts will use (everything else they will ignore)
    public LayerMask gameLayers;

    // determines if currentMorphBot can and should be destroyed
    bool canDestroy;

    // the current MorphBot that the script has detected
    GameObject currentMorphBot;

    // a ray from the main camera to where the mouse is
    Ray destructionRay;
    // holds any information about the proceeding raycasts such as the object that was hit, location, etc.
    RaycastHit destructionRayHit;

    // runs every frame
    private void Update()
    {
        // creates a line from the main camera to the position of the mouse and assigns the value to destructionRay
        destructionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        /* returns a boolean depending on whether destructionRay hit something with a gameLayers layer, returning hit information
        to destructionRayHit */
        if (Physics.Raycast(destructionRay, out destructionRayHit, maxRaycastDistance, gameLayers))
        {
            // asks destructionRayHit if the object that was hit by destructionRay has a layer of 8 (which is the layer that MorphBots use)
            if (destructionRayHit.transform.gameObject.layer == 8)
            {
                // sets canDestroy to true (meaning that the hit MorphBot can be destroyed)
                canDestroy = true;
                // sets currentMorphBot to the object that destructionRayHit stored
                currentMorphBot = destructionRayHit.transform.gameObject;
            }

            // runs if the layer in gameLayer was not 8 (a MorphBot)
            else
            {
                // runs if canDestroy is true
                if (canDestroy == true)
                {
                    // sets canDestroy to false, telling the script that nothing can be destroyed
                    canDestroy = false;
                }
            }
        }

        // runs if no collision between destructionRay and objects with gameLayers was detected
        else
        {
            // runs if canDestroy is true
            if (canDestroy == true)
            {
                // sets canDestroy to false, telling the script that nothing can be destroyed
                canDestroy = false;
            }
        }

        // runs if right mouse button is held down and canDestroy is true
        if (Input.GetMouseButtonDown(1) && canDestroy == true)
        {
            // destroys the GameObject associated with currentMorphBot
            Destroy(currentMorphBot);
        }
    }
}
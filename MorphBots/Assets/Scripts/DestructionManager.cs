using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionManager : MonoBehaviour
{
    public float maxRaycastDistance;

    public LayerMask gameLayers;

    bool canDestroy;

    GameObject currentMorphBot;

    Ray destructionRay;
    RaycastHit destructionRayHit;

    private void Update()
    {
        destructionRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(destructionRay, out destructionRayHit, maxRaycastDistance, gameLayers))
        {
            if (destructionRayHit.transform.gameObject.layer == 8)
            {
                canDestroy = true;
                currentMorphBot = destructionRayHit.transform.gameObject;
            }

            else
            {
                canDestroy = false;
            }
        }

        if (Input.GetMouseButtonDown(1) && canDestroy == true)
        {
            Destroy(currentMorphBot);
        }
    }
}
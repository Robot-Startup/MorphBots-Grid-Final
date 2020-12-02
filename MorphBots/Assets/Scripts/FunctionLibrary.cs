using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionLibrary : MonoBehaviour
{
    public Vector3 GetGridPos(RaycastHit raycastHit)
    {
        Vector3 gridPos = new Vector3(0, 0, 0);
        Vector3 rawInput = raycastHit.point - raycastHit.transform.position;

        if (rawInput.x > 0)
        {
            gridPos.x = Mathf.Floor(rawInput.x + 0.5f);
        }

        else if (rawInput.x < 0)
        {
            gridPos.x = Mathf.Floor(rawInput.x * -1 + 0.5f) * -1;
        }

        if (rawInput.y > 0)
        {
            gridPos.y = Mathf.Floor(rawInput.y + 0.5f);
        }

        else if (rawInput.y < 0)
        {
            gridPos.y = Mathf.Floor(rawInput.y * -1 + 0.5f) * -1;
        }

        if (rawInput.z > 0)
        {
            gridPos.z = Mathf.Floor(rawInput.z + 0.5f);
        }

        else if (rawInput.z < 0)
        {
            gridPos.z = Mathf.Floor(rawInput.z * -1 + 0.5f) * -1;
        }

        gridPos += raycastHit.transform.position;
        return gridPos;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public float maxRaycastDistance;

    public GameObject highlighterRef;

    public LayerMask gameLayers;
    public LayerMask platformLayer;

    GameObject highlighter;

    Ray highlightRay;
    RaycastHit highlightRayHit;

    Vector3 hitPoint;
    Vector3 truePosition;

    public void UpdateVisibility(bool isVisible)
    {
        highlighter.SetActive(isVisible);
    }

    public Vector3 CalculateTruePos(Vector3 position)
    {
        if (position.x > 0)
        {
            truePosition.x = Mathf.Floor(position.x + 0.5f);
        }

        else if (position.x < 0)
        {
            truePosition.x = Mathf.Floor(position.x * -1 + 0.5f) * -1;
        }

        if (position.y > 0)
        {
            truePosition.y = Mathf.Floor(position.y + 0.5f);
        }

        else if (position.y < 0)
        {
            truePosition.y = Mathf.Floor(position.y * -1 + 0.5f) * -1;
        }

        if (position.z > 0)
        {
            truePosition.z = Mathf.Floor(position.z + 0.5f);
        }

        else if (position.z < 0)
        {
            truePosition.z = Mathf.Floor(position.z * -1 + 0.5f) * -1;
        }

        return truePosition;
    }

    private void Start()
    {
        highlighter = Instantiate(highlighterRef, new Vector3(0, 0, 0), Quaternion.identity);
        UpdateVisibility(false);
    }

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
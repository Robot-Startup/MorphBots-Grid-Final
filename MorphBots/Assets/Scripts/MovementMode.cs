using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMode : MonoBehaviour
{
    HighlightManager highlightManager;

    public GameObject selectedMorphBot;
    public Material selectedMaterial;
    public Material defaultMaterial;
    public KeyCode moveKey;

    Vector3 initialLocation;
    Vector3 endingLocation;
    Ray morphBotRay;
    RaycastHit morphBotRayHit;
    public float maxRaycastDistance;
    public LayerMask gameLayers;
    Vector3 mousePosition;
    Vector3 truePosition;
    Vector3 amountToMove;
    const float lerpTimerInitial = 1;
    float lerpTimerCurrent = lerpTimerInitial;
    float lerpPercentage;
    public bool canMove = true;

    private void Start()
    {
        highlightManager = GetComponent<HighlightManager>();
    }
    public void SelectMorphBot(GameObject morphBotRef)
    {
        if (morphBotRef != selectedMorphBot)
        {   if (selectedMorphBot != null)
            {
                selectedMorphBot.GetComponent<MeshRenderer>().material = defaultMaterial;
            }
            selectedMorphBot = morphBotRef;
            selectedMorphBot.GetComponent<MeshRenderer>().material = selectedMaterial;
        }

        else if (morphBotRef == selectedMorphBot)
        {
            selectedMorphBot.GetComponent<MeshRenderer>().material = defaultMaterial;
            selectedMorphBot = null;
        }
    }

    public void InitiateMovementProcess()
    {
        initialLocation = selectedMorphBot.transform.position;
        GetTransformLocation();
        endingLocation = morphBotRayHit.transform.position + truePosition;
        amountToMove = endingLocation - initialLocation;
        Timer();
    }

    public void GetTransformLocation()
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
    }

    public void Timer()
    {
        lerpTimerCurrent -= Time.deltaTime;
        lerpTimerCurrent = Mathf.Clamp(lerpTimerCurrent, 0, lerpTimerInitial);
        lerpPercentage = Mathf.Clamp(((lerpTimerInitial - lerpTimerCurrent) / lerpTimerInitial), 0, lerpTimerInitial);
        if (selectedMorphBot.transform.position != endingLocation)
        {
            Lerp();
            Invoke("Timer", Time.deltaTime);
            // note for tomorrow: make sure highlightblock is disabled when moving and also you cannot switch modes when moving.
        }
        else
        {
            lerpTimerCurrent = lerpTimerInitial;
            highlightManager.enabled = true;
            canMove = true;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(moveKey))
        {
            if (selectedMorphBot != null)
            {
                morphBotRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(morphBotRay, out morphBotRayHit, maxRaycastDistance, gameLayers))
                {
                    if (canMove)
                    {
                        canMove = false;
                        InitiateMovementProcess();
                        highlightManager.UpdateVisibility(false);
                        highlightManager.enabled = false;
                    }
                }
            }
        }
    }

    public void Lerp()
    {
        selectedMorphBot.transform.position = Vector3.Lerp(initialLocation, endingLocation, lerpPercentage);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementMode : MonoBehaviour
{
    HighlightManager highlightManager;

    public GameObject selectedMorphBot;
    public Material selectedMaterial;
    public Material defaultMaterial;
    public bool canMove = true;

    float timePercent;
    public float initialTime;
    float currentTime;

    public KeyCode transformKey;

    Ray screenRay;
    RaycastHit screenRayHit;
    public LayerMask gameLayers;
    public float maxRaycastDistance;

    Vector3 initialLocation;
    Vector3 destinationLocation;
    Vector3 amountToMove;
    Vector3 currentLocation;
    int movesRemaining;
    int movementOffset;

    FunctionLibrary functionLibrary;


    private void Awake()
    {
        highlightManager = GetComponent<HighlightManager>();
        functionLibrary = GetComponent<FunctionLibrary>();
        selectedMorphBot = null;
        currentTime = initialTime;
    }

    public void SelectMorphBot(GameObject morphBotRef)
    {
        if (morphBotRef != selectedMorphBot)
        {
            if (selectedMorphBot != null)
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

    private void BeginMovement()
    {
        initialLocation = selectedMorphBot.transform.position;
        destinationLocation = functionLibrary.GetGridPos(screenRayHit);
        amountToMove = destinationLocation - initialLocation;

        movesRemaining = Convert.ToInt32(amountToMove.x);
        currentLocation = initialLocation;

        if (movesRemaining > 0)
        {
            movementOffset = 1;
        }

        else
        {
            movementOffset = -1;
        }

        MoveInX();

    }

    private void ResetTimer()
    {
        currentTime = initialTime;
    }

    private bool UpdateTimer()
    {
        currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0, initialTime);

        timePercent = 1 - (currentTime / initialTime);

        if (currentTime > 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (canMove)
        {
            if (selectedMorphBot != null)
            {
                if (Input.GetKeyDown(transformKey))
                {
                    screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(screenRay, out screenRayHit, maxRaycastDistance, gameLayers))
                    {
                        if (screenRayHit.transform.gameObject.layer != 10)
                        {
                            canMove = false;
                            highlightManager.UpdateVisibility(false);
                            highlightManager.enabled = false;
                            BeginMovement();
                        }
                    }
                }
            }
        }
    }

    private void MoveInX()
    {
        if (currentLocation.x != destinationLocation.x)
        {
            if (UpdateTimer())
            {
                currentLocation = selectedMorphBot.transform.position;
                currentLocation.x = Mathf.Lerp(initialLocation.x, initialLocation.x + movementOffset, timePercent);
                selectedMorphBot.transform.position = currentLocation;
                Invoke("MoveInX", Time.deltaTime);
            }

            else
            {
                currentLocation.x = initialLocation.x + movementOffset;
                selectedMorphBot.transform.position = currentLocation;
                initialLocation.x += movementOffset;
                ResetTimer();
                Invoke("MoveInX", Time.deltaTime);
            }
        }

        else
        {
            movesRemaining = Convert.ToInt32(amountToMove.y);

            if (movesRemaining > 0)
            {
                movementOffset = 1;
            }

            else
            {
                movementOffset = -1;
            }

            MoveInY();
        }
    }

    private void MoveInY()
    {
        if (currentLocation.y != destinationLocation.y)
        {
            if (UpdateTimer())
            {
                currentLocation = selectedMorphBot.transform.position;
                currentLocation.y = Mathf.Lerp(initialLocation.y, initialLocation.y + movementOffset, timePercent);
                selectedMorphBot.transform.position = currentLocation;
                Invoke("MoveInY", Time.deltaTime);
            }

            else
            {
                currentLocation.y = initialLocation.y + movementOffset;
                selectedMorphBot.transform.position = currentLocation;
                initialLocation.y += movementOffset;
                ResetTimer();
                Invoke("MoveInY", Time.deltaTime);
            }
        }

        else
        {
            movesRemaining = Convert.ToInt32(amountToMove.z);

            if (movesRemaining > 0)
            {
                movementOffset = 1;
            }

            else
            {
                movementOffset = -1;
            }

            MoveInZ();
        }
    }

    private void MoveInZ()
    {
        if (currentLocation.z != destinationLocation.z)
        {
            if (UpdateTimer())
            {
                currentLocation = selectedMorphBot.transform.position;
                currentLocation.z = Mathf.Lerp(initialLocation.z, initialLocation.z + movementOffset, timePercent);
                selectedMorphBot.transform.position = currentLocation;
                Invoke("MoveInZ", Time.deltaTime);
            }

            else
            {
                currentLocation.z = initialLocation.z + movementOffset;
                selectedMorphBot.transform.position = currentLocation;
                initialLocation.z += movementOffset;
                ResetTimer();
                Invoke("MoveInZ", Time.deltaTime);
            }
        }

        else
        {
            canMove = true;
            highlightManager.enabled = true;
        }
    }
}
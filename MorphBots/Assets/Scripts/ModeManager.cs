using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    PlacementMode placementMode;
    MovementMode movementMode;
    DestructionManager destructionManager;
    public KeyCode stateSwitch;
    public enum gameState { placement, movement };
    public Text textRef;

    public gameState currentGameState;

    public void Awake()
    {
        placementMode = GetComponent<PlacementMode>();
        movementMode = GetComponent<MovementMode>();
        destructionManager = GetComponent<DestructionManager>();
    }

    public void SwitchGameState()
    {
        if (currentGameState == gameState.placement)
        {
            currentGameState = gameState.movement;
        }
        else if (currentGameState == gameState.movement)
        {
            if (movementMode.canMove == true)
            {
                currentGameState = gameState.placement;
            }
        }
    }

    public void UpdateGameState()
    {
        if (currentGameState == gameState.placement)
        {
            if (movementMode.selectedMorphBot != null)
            {
                movementMode.selectedMorphBot.GetComponent<MeshRenderer>().material = movementMode.defaultMaterial;
                movementMode.selectedMorphBot = null;
            }

            movementMode.enabled = false;
            placementMode.enabled = true;
            destructionManager.enabled = true;
            textRef.text = "Placement Mode";
        }

        else if (currentGameState == gameState.movement)
        {
            placementMode.enabled = false;
            destructionManager.enabled = false;
            movementMode.enabled = true;
            textRef.text = "Movement Mode";
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(stateSwitch))
        {
            SwitchGameState();
            UpdateGameState();
        }
    }
}
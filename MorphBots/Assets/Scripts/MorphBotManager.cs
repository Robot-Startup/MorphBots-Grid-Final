using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphBotManager : MonoBehaviour
{
    MovementMode movementMode;
    ModeManager modeManager;
    private void Awake()
    {
        movementMode = FindObjectOfType<MovementMode>();
        modeManager = FindObjectOfType<ModeManager>();
    }
    private void OnMouseDown()
    {
        if (modeManager.currentGameState == ModeManager.gameState.movement && movementMode.canMove == true)
        {
            movementMode.SelectMorphBot(this.gameObject);
        }
    }
}

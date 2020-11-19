using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    ModeManager modeManager;

    public void Start()
    {
        modeManager = GetComponent<ModeManager>();
        modeManager.UpdateGameState();
    }
}
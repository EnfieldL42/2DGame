using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PlayerInputTest : MonoBehaviour
{
    public int playerID;
    public MultiplayerInput inputManager;
    InputControls inputControls;
    // Start is called before the first frame update
    void Start()
    {
        inputManager.onPlayerJoined += AssignInputs;
    }

    // Update is called once per frame
    void AssignInputs(int ID)
    {
        if(playerID == ID)
        {
            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
        }
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        
    }
}

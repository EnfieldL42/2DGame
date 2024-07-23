using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[System.Serializable]
public class individualPlayerControls
{
    public int playerID;

    public InputDevice inputDevice;
    InputUser inputUser;

    public InputControl playerControls;


    public void SetupPlayer(InputAction.CallbackContext obj, int ID)
    {
        playerID = ID;
        inputDevice = obj.control.device;

       // playerControls = new InputControlls();
        inputUser = InputUser.PerformPairingWithDevice(inputDevice);
       // inputUser.AssociateActionsWithUser(playerControls);

        //playerControls.Enable();

    }

}

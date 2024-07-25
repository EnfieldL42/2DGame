using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[System.Serializable]
public class PlayerControls
{
    public int playerID;
    public InputDevice inputDevice;
    InputUser inputUser;

    public InputControls playerControls;
    public ControllerType controllerType;

    public enum ControllerType
    {
        Keyboard,
        Playstation,
        Xbox,
        Switch
    }

    public void SetupPlayer(InputAction.CallbackContext context, int ID)
    {
        playerID = ID;
        inputDevice = context.control.device;
        playerControls = new InputControls();

        inputUser = InputUser.PerformPairingWithDevice(inputDevice);
        inputUser.AssociateActionsWithUser(playerControls);
        SetControllerType();

        playerControls.Enable();
    }

    public void DisableControls()
    {
        playerControls.Disable();
    }

    private void SetControllerType()
    {
        if (inputDevice is UnityEngine.InputSystem.XInput.XInputControllerWindows)
        {
            controllerType = ControllerType.Xbox;
        }
        if (inputDevice is UnityEngine.InputSystem.Switch.SwitchProControllerHID)
        {
            controllerType = ControllerType.Switch;
        }
        if (inputDevice is UnityEngine.InputSystem.Keyboard)
        {
            controllerType = ControllerType.Keyboard;
        }
        if (inputDevice is UnityEngine.InputSystem.DualShock.DualShock4GamepadHID)
        {
            controllerType = ControllerType.Playstation;
        }
    }
}

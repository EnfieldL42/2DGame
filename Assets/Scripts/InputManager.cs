using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering; //Use Unity Input System

public class InputManager : MonoBehaviour
{
    public Vector2 movement;
    public static InputControls inputControls; //Referencing input actions

    // Start is called before the first frame update
    void Start()
    {
        //Creates inputs
        inputControls = new InputControls();
        inputControls.MasterControls.Jump.performed += JumpPerformed;
        inputControls.MasterControls.Jump.canceled += JumpCanceled;
        inputControls.MasterControls.Attack.performed += AttackPerformed;
        inputControls.MasterControls.Attack.canceled += AttackCanceled;
        inputControls.MasterControls.Movement.performed += MovementPerformed;
        inputControls.MasterControls.Movement.canceled += MovementCanceled;
        inputControls.Enable();
    }

    private void AttackCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("attackcamcelled");
        throw new NotImplementedException();
    }

    private void JumpCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("jumpcamcelled");
        throw new NotImplementedException();
        
    }

    private void MovementCanceled(InputAction.CallbackContext context)
    {
        movement = Vector2.zero;
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        movement = (context.ReadValue<Vector2>());
    }

    private void AttackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("attack");
        Debug.Log(context.control.device.displayName);
    }

    //Function when button is pressed
    private void JumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("jomp");
        Debug.Log(context.control.device.displayName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

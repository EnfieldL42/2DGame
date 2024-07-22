using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class inputManagesr : MonoBehaviour
{

    public static InputControlls inputControls;

    public Vector2 movement;

    // Start is called before the first frame update
    void Awake()
    {
        inputControls = new InputControlls();

        inputControls.Generic.Jump.performed += JumpPerformed;
        inputControls.Generic.Attack.performed += Attack_performed;
        inputControls.Generic.Movement.performed += Movement_performed;
        inputControls.Generic.Movement.canceled += Movement_canceled;
        inputControls.Enable();
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        //movement = Vector2 zero;
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        movement = obj.ReadValue<Vector2>();
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        print("attacked");
    }

    private void JumpPerformed(InputAction.CallbackContext obj)
    {
        print("jumped");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

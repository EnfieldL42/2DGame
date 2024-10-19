using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject firstButton;
    bool isSelected = false;
    PlayerControls playerControls;


    private void Start()
    {
        AudioManager.instance.PlayMusic("5");
        isSelected = false;
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            isSelected = false;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
        {
            if (!isSelected)
            {
                SetFirstSelectedButton();

            }
        }

        float leftStickY = Gamepad.current.leftStick.ReadValue().y;

        if (leftStickY > 0.3f || leftStickY < -0.3f || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame)
        {
            if (!isSelected)
            {
                SetFirstSelectedButton();

            }
        }

    }

    private void SetFirstSelectedButton()
    {
        isSelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}

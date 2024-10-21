using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject firstSettingsButton;
    public bool isSelected = false;
    PlayerControls playerControls;

    public SettingsMenu settingsMenu;
    public GameObject controlsScreen;
    public GameObject controlsButton;

    public Button[] uiButtons = new Button[4];

    private void Start()
    {
        AudioManager.instance.PlayMusic("5");
        isSelected = false;

        settingsMenu.SetMusicVolume();
        settingsMenu.SetSFXVolume();

    }

    private void Update()
    {


        if (!settingsMenu.gameObject.activeSelf)
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
        else if(settingsMenu.gameObject.activeSelf)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                isSelected = false;
            }

            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            {
                if (!isSelected)
                {
                    SetSelectedButtonSettings();

                }
            }

            float leftStickY = Gamepad.current.leftStick.ReadValue().y;

            if (leftStickY > 0.3f || leftStickY < -0.3f || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame)
            {
                if (!isSelected)
                {
                    SelectControlsButtons();

                }
            }
        }
        else if (controlsScreen.gameObject.activeSelf)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                isSelected = false;
            }

            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame)
            {
                if (!isSelected)
                {
                    SelectControlsButtons();

                }
            }

            float leftStickY = Gamepad.current.leftStick.ReadValue().y;

            if (leftStickY > 0.3f || leftStickY < -0.3f || Gamepad.current.buttonSouth.wasPressedThisFrame || Gamepad.current.dpad.up.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame)
            {
                if (!isSelected)
                {
                    SelectControlsButtons();

                }
            }
        }


    }

    public void UnselectCurrentButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        isSelected = false;
    }

    private void SetFirstSelectedButton()
    {
        isSelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void SetSelectedButtonSettings()
    {
        isSelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSettingsButton);
    }

    private void SelectControlsButtons()
    {
        isSelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(controlsButton);
    }



    public void OpenControls()
    {

        if (controlsScreen.gameObject.activeSelf == true)
        {
            EnableUIButtons();
            isSelected = false;
            controlsScreen.gameObject.SetActive(false);
            AudioManager.instance.PlaySFX("Click Button", 4);
        }
        else
        {
            DisableUIButtons();
            SelectControlsButtons();
            controlsScreen.gameObject.SetActive(true);
        }

    }


    public void DisableUIButtons()
    {
        foreach (Button button in uiButtons)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    public void EnableUIButtons()
    {
        foreach (Button button in uiButtons)
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }
    }
}

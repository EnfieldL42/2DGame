using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public class ItemStation : MonoBehaviour
{
    public int itemID;
    private HashSet<int> collectedByPlayers = new HashSet<int>();


    public float requiredStayTime; 
    public float stayTimer = 0f;
    public bool isPlayerInStation = false;

    public Animator stationPressE;
    public TextMeshProUGUI interactMessage;


    public bool TryCollectItem(int playerID, PlayerInventory playerInventory)
    {
        if (playerInventory.uniqueItem == -1)
        {
            if (collectedByPlayers.Contains(playerID))
            {
                Debug.Log($"Player {playerID} has already collected from this station.");
                return false;
            }

            if (playerInventory.CollectItem(itemID)) // Tries to collect the item only if the player can hold more items

            {
                collectedByPlayers.Add(playerID); // Mark this player as having collected from the station
                return true;
            }
        }
        return false;
    }

    public void ResetCollectionStatus(int playerID)
    {
        if (collectedByPlayers.Contains(playerID))
        {
            collectedByPlayers.Remove(playerID);
            Debug.Log($"Player {playerID} can now collect from this station again.");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInStation = true;
            stayTimer = 0f;

            PlayerControls playerControls = MultiplayerInputManager.instance.players[0]; // Assuming player 1 for now
            UpdateInteractMessage(playerControls.inputDevice);
        }
    }

    private void Update()
    {
        if (isPlayerInStation)
        {
            stayTimer += Time.deltaTime;

        }
        if (stayTimer >= requiredStayTime)
        {
            PlayStationAnimation();

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInStation = false;
            stayTimer = 0f;

            stationPressE.SetBool("TurnOnOff", false);
        }
    }

    private void PlayStationAnimation()
    {

        stationPressE.SetBool("TurnOnOff", true);

    }

    public void ResetTimer()
    {
        stayTimer = 0f;
        stationPressE.SetBool("TurnOnOff", false);

    }

    private void UpdateInteractMessage(InputDevice inputDevice)
    {
        string message = "";

        //if (inputDevice is Keyboard)
        //{
        //    message = "Press E to interact";
        //}
        //else if (inputDevice is Gamepad)
        //{
        //    var gamepad = inputDevice as Gamepad;

        //    if (gamepad is DualShockGamepad) 
        //    {
        //        message = "Press X to interact";
        //    }
        //    else if (gamepad is XInputController) 
        //    {
        //        message = "Press A to interact";
        //    }
        //    else
        //    {
        //        message = "Press a button to interact";
        //    }
        //}

        message = "Press interact button";

        interactMessage.text = message;
    }
}

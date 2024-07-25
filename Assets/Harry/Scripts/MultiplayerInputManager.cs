using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MultiplayerInputManager : MonoBehaviour
{
    public List<PlayerControls> players = new List<PlayerControls>();
    int maxPlayers = 2;

    public InputControls inputControls;

    public delegate void OnPlayerJoined(int playerID);
    public OnPlayerJoined onPlayerJoined;

    private void Awake()
    {
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        inputControls = new InputControls();
        inputControls.MasterControls.JoinButton.performed += JoinButtonPerformed;
        inputControls.Enable();
    }

    private void JoinButtonPerformed(InputAction.CallbackContext context)
    {
        if(players.Count >= maxPlayers)
        {
            return;
        }

        foreach(PlayerControls player in players)
        {
           if (player.inputDevice  == context.control.device)
            {
                return;
            }
        }
        PlayerControls newPlayer = new PlayerControls();
        newPlayer.SetupPlayer(context, players.Count);
        players.Add(newPlayer);

        if(onPlayerJoined != null)
        {
            onPlayerJoined.Invoke(newPlayer.playerID);
        }
    }
}

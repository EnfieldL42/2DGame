using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;


public class MultiplayerInputManager : MonoBehaviour
{
    public static MultiplayerInputManager instance;

    public List<PlayerControls> players = new List<PlayerControls>();
    public int maxPlayers = 4;

    public InputControls inputControls;

    public delegate void OnPlayerJoined(int playerID);
    public OnPlayerJoined onPlayerJoined;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInputs();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInputs()
    {
        inputControls = new InputControls();
        inputControls.MasterControls.JoinButton.performed += JoinButtonPerformed;
        inputControls.Enable();
    }

    private void JoinButtonPerformed(InputAction.CallbackContext context)
    {
        if (players.Count >= maxPlayers)
        {
            return;
        }

        foreach (PlayerControls player in players)
        {
            if (player.inputDevice == context.control.device)
            {
                return;
            }
        }
        PlayerControls newPlayer = new PlayerControls();
        newPlayer.SetupPlayer(context, players.Count);
        players.Add(newPlayer);

        if (onPlayerJoined != null)
        {
            onPlayerJoined.Invoke(newPlayer.playerID);
        }
    }


    public void DisableInputs()
    {
        inputControls.Disable();
        foreach (PlayerControls players in players)
        {
            players.playerControls.Disable();
        }
    }
}

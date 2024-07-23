using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInput : MonoBehaviour
{
    public List<IndividualControls> players = new List<IndividualControls>();
    int maxPlayers = 4;

    public static InputControls inputControls;

    public delegate void OnPlayerJoined(int playerID);
    public OnPlayerJoined onPlayerJoined;

    private void Awake()
    {
        InitializeInputs();
    }
    void InitializeInputs()
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

        foreach(IndividualControls player in players)
        {
            if (player.inputDevice == context.control.device)
            {
                return;
            }
        }

        IndividualControls newPlayer = new IndividualControls();
        newPlayer.SetupPlayer(context, players.Count);
        players.Add(newPlayer);

    }
}

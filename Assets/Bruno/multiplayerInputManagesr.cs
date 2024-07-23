using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class multiplayerInputManagesr : MonoBehaviour
{

    public List<individualPlayerControls> players = new List<individualPlayerControls>();

    int maxPlayers = 4;

    public InputControlls inputControls;

    // Start is called before the first frame update
    public void Awake()
    {
        InitializeInputs();

    }


    void InitializeInputs()
    {
        inputControls = new InputControlls();
        inputControls.Generic.Join.performed += Join_performed;
        inputControls.Enable();

    }

    private void Join_performed(InputAction.CallbackContext obj)
    {
        if(players.Count >= maxPlayers) 
        {
            return;
        }

        individualPlayerControls newPlayer = new individualPlayerControls();
        newPlayer.SetupPlayer(obj, players.Count);
        players.Add(newPlayer);

    }


}

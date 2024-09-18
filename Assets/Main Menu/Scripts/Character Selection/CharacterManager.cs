using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase characterDB;
    public TextMeshProUGUI nameText;
    public Image artworkSprite;

    public int[] selectedOption;
    public int playerID;

    public Vector2 input;
    private InputControls inputControls;

    private bool canChangeOption = true; 

    private void Start()
    {

        selectedOption = new int[4];


        if (MultiplayerInputManager.instance.players.Count > playerID)
        {
            AssignInputs(playerID);
        }
        else
        {
            MultiplayerInputManager.instance.onPlayerJoined += AssignInputs;
        }


        UpdateCharacter(selectedOption[playerID]);

    }

    void AssignInputs(int ID)
    {
        if (playerID == ID)
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;

            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed -= MovementPerformed;
        }
        else
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;
            inputManager.onPlayerJoined -= AssignInputs;
        }
    }

    private void MovementPerformed(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        if (canChangeOption && input.x > 0.3f)
        {
            NextOption();
            canChangeOption = false;
        }
        else if (canChangeOption && input.x < -0.3f)
        {
            BackOption();
            canChangeOption = false;
        }
        else if (input.x > -0.3f && input.x < 0.3f)
        {
            canChangeOption = true;
        }
    }

    public void NextOption()
    {
        selectedOption[playerID]++;

        if (selectedOption[playerID] >= characterDB.characterCount)
        {
            selectedOption[playerID] = 0;
        }

        UpdateCharacter(selectedOption[playerID]);
    }

    public void BackOption()
    {
        selectedOption[playerID]--;

        if (selectedOption[playerID] < 0)
        {
            selectedOption[playerID] = characterDB.characterCount - 1;
        }

        UpdateCharacter(selectedOption[playerID]);

    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);

        artworkSprite.sprite = character.characterSprite;
        nameText.text = character.characterName;

        Save();
    }

    private void Load()
    {
        for (int i = 0; i < selectedOption.Length; i++)
        {
            if (PlayerPrefs.HasKey("SelectedOption_Player" + i))
            {
                selectedOption[i] = PlayerPrefs.GetInt("SelectedOption_Player" + i);
            }
            else
            {
                selectedOption[i] = 0; 
            }
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt("SelectedOption_Player" + playerID, selectedOption[playerID]);
        PlayerPrefs.Save();
    }
}

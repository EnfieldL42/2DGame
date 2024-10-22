using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public bool hasConfirmed = false;

    public GameObject firstButton;
    bool isSelected = false;

    public MainMenuUIManager mainMenuManager;
    bool canReturntoMenu = true;

    public Button returnButton;

    [SerializeField] Dictionary<int, bool> playerConfirmed = new Dictionary<int, bool>();

    private void Start()
    {
        ResetPlayerPrefs(playerID);

        playerConfirmed[playerID] = false;

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
            CharacterDataManager.instance.TurnOnPlayer(ID);

            inputManager.onPlayerJoined -= AssignInputs;
            inputControls = inputManager.players[playerID].playerControls;
            inputControls.MasterControls.Movement.performed += MovementPerformed;
            inputControls.MasterControls.Attack.performed += InteractionPerformed;
            inputControls.MasterControls.Pause.performed += EscapePerformed;
        }
    }

    private void OnDisable()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed -= MovementPerformed;
            inputControls.MasterControls.Attack.performed -= InteractionPerformed;
            inputControls.MasterControls.Pause.performed -= EscapePerformed;


        }
        else
        {
            MultiplayerInputManager inputManager = MultiplayerInputManager.instance;
            inputManager.onPlayerJoined -= AssignInputs;
        }
    }

    private void disableInputs()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed -= MovementPerformed;
            inputControls.MasterControls.Attack.performed -= InteractionPerformed;
            //inputControls.MasterControls.Pause.performed -= EscapePerformed;
        }

    }

    private void enableInputs()
    {
        if (inputControls != null)
        {
            inputControls.MasterControls.Movement.performed += MovementPerformed;
            inputControls.MasterControls.Attack.performed += InteractionPerformed;
            inputControls.MasterControls.Pause.performed += EscapePerformed;
        }
    }



    private void MovementPerformed(InputAction.CallbackContext context)
    {
        if (!playerConfirmed[playerID])
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
        if (playerID == 0)
        {
            if (playerConfirmed[playerID])
            {
                if (!isSelected && playerID == 0)
                {
                    SetFirstSelectedButton();
                }

            }
        }

    }

    private void SetFirstSelectedButton()
    {
        isSelected = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    private void UnselectButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void InteractionPerformed(InputAction.CallbackContext context)
    {
        if (!playerConfirmed[playerID])
        {
            mainMenuManager.confirmselection();
            playerConfirmed[playerID] = true;
        }

        mainMenuManager.selectUINoArrow[playerID].SetActive(true);
        mainMenuManager.selectUI[playerID].SetActive(false);
        canReturntoMenu = false;    
        AudioManager.instance.PlaySFX("Select Button", 4);
        //hasConfirmed = true;
        mainMenuManager.turnOnConfirmedBackground(playerID);
        if (playerID > 0)
        {
            disableInputs();
        }


    }

    private void EscapePerformed(InputAction.CallbackContext context)
    {
        if (playerConfirmed[playerID])
        {
            mainMenuManager.confirmselection();
            playerConfirmed[playerID] = false;

        }


        if (canReturntoMenu)
        {
            if (returnButton != null)
            {
                returnButton.onClick.Invoke();
            }
        }
        else
        {
            mainMenuManager.unconfirmSelection();
            mainMenuManager.selectUINoArrow[playerID].SetActive(false);
            mainMenuManager.selectUI[playerID].SetActive(true);

            isSelected = true;
            AudioManager.instance.PlaySFX("Select Button", 4);
            //hasConfirmed = false;
            mainMenuManager.turnOffConfirmedBackground(playerID);

            if (playerID == 0)
            {
                UnselectButton();
            }

            if (playerID > 0)
            {
                enableInputs();
            }
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
        CharacterDataManager.instance.SetPlayer(playerID, selectedOption[playerID]);
        PlayerPrefs.SetInt("SelectedOption_Player" + playerID, selectedOption[playerID]);
        PlayerPrefs.Save();

        //int winnerSprite = CharacterDataManager.instance.GetCharacterSprite(playerID);
    }

    private void ResetPlayerPrefs(int playerID)
    {
        CharacterDataManager.instance.ResetActivePlayers();

        PlayerPrefs.DeleteKey("SelectedOption_Player" + playerID);

    }

}

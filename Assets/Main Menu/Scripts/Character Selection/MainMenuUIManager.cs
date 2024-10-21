using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainMenuUIManager : MonoBehaviour
{
    public List<TextMeshProUGUI> playerInfoTexts = new List<TextMeshProUGUI>();
    public List<GameObject> pressToStartImages = new List<GameObject>();
    public List<GameObject> playerSelection = new List<GameObject>();
    public List<GameObject> confirmedImages = new List<GameObject>();
    public List<GameObject> selectUI = new List<GameObject>();
    public List<GameObject> selectUINoArrow = new List<GameObject>();


    public GameObject startButton; 
    private MultiplayerInputManager inputManager;
    public CharacterManager mainmenumang;
    public int maxPlayers;

    public Button[] uiButtons;

    public EventSystem eventSystem;

    public int confirmedNumber = 0;

    private void Awake()
    {
        DisableUIButtons();
    }
    private void Start()
    {
        inputManager = MultiplayerInputManager.instance;
        inputManager.onPlayerJoined += UpdatePlayerInfo;

    }


    private void UpdatePlayerInfo(int playerID)
    {
        if (playerID < playerInfoTexts.Count)
        {
            playerInfoTexts[playerID].text = "Player " + (playerID + 1) + " joined";
        }

        if (playerID < pressToStartImages.Count)
        {

            pressToStartImages[playerID].SetActive(false);
            playerSelection[playerID].SetActive(true);
            selectUI[playerID].SetActive(true);
        }

        //if (inputManager.players.Count >= maxPlayers && startButton != null)
        //{
        //    startButton.SetActive(true);
        //}

 
    }

    public void confirmselection()
    {
        confirmedNumber++;
    }

    public void unconfirmSelection()
    {
        confirmedNumber--;
    }

    public void turnOnConfirmedBackground(int playerID)
    {
        if (playerID < pressToStartImages.Count)
        {
            confirmedImages[playerID].SetActive(true);
        }

    }
    public void turnOffConfirmedBackground(int playerID)
    {
        if (playerID < pressToStartImages.Count)
        {
            confirmedImages[playerID].SetActive(false);
        }

    }

    private void Update()
    {
        //
        //
        //
        if (inputManager.players.Count >= maxPlayers && confirmedNumber >= maxPlayers)
        {
            EnableUIButtons();
        }
        if (inputManager.players.Count < maxPlayers && confirmedNumber < maxPlayers)
        {
            DisableUIButtons();
        }
    }

    private void OnDestroy()
    {
        inputManager.onPlayerJoined -= UpdatePlayerInfo;
    }

    public void DisableUIButtons()
    {
        if (uiButtons != null)
        {
            foreach (Button button in uiButtons)
            {
                if (button != null) 
                {
                    button.interactable = false;
                }
            }
        }

    }

    public void EnableUIButtons()
    {
        if (uiButtons != null)
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

}


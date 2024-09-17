using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    public List<TextMeshProUGUI> playerInfoTexts = new List<TextMeshProUGUI>();
    public List<GameObject> pressToStartImages = new List<GameObject>();
    public List<GameObject> playerSelection = new List<GameObject>();


    public GameObject startButton; 
    private MultiplayerInputManager inputManager;

    public int maxPlayers;

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
        }

        if (inputManager.players.Count >= maxPlayers && startButton != null)
        {
            startButton.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        inputManager.onPlayerJoined -= UpdatePlayerInfo;
    }
}


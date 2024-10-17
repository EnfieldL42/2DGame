using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MultiplayerInputManager inputManager;

    public static int playerOne;
    public static int playerTwo;
    public static List<int> losers = new List<int>(); // Track all losers
    public PlayerInput[] players;

    private void Awake()
    {
        inputManager = FindObjectOfType<MultiplayerInputManager>();
    }

    public static void SetPlayerOne(int id)
    {
        playerOne = id;
    }

    public static void SetPlayerTwo(int id)
    {
        playerTwo = id;
    }
    public void DisableInputs()
    {
        foreach(var players in players)
        {
            players.DisableControls();
        }
    }
    public void GameEnded(int id)
    {

        if (id == playerTwo && !losers.Contains(id)) // Ensure each loser is only added once
        {
            losers.Add(id);
            OCGameManager.nerfedPlayer = 100;
        } 
        else
        {
            OCGameManager.nerfedPlayer = playerOne;
        }
        StartCoroutine(ReturnToMain());
        Debug.Log("Losers: " + string.Join(", ", losers));
    }

    IEnumerator ReturnToMain()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Don't Stop Smithing");
    }
}

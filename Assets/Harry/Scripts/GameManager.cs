using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MultiplayerInputManager inputManager;

    public static int playerOne;
    public static int playerTwo;
    public static List<int> losers = new List<int>(); // Track all losers

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

    public void GameEnded(int id)
    {
        if (!losers.Contains(id)) // Ensure each loser is only added once
        {
            losers.Add(id);
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

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MultiplayerInputManager inputManager;
    public string currentScene;

    public static int playerOne;
    public static int playerTwo;

    private void Awake()
    {
        inputManager = FindObjectOfType<MultiplayerInputManager>();
        currentScene = SceneManager.GetActiveScene().name;    
    }

    public static void SetPlayerOne(int id)
    {
        playerOne = id;
    }
    public static void SetPlayerTwo(int id)
    {
        playerTwo = id;
    }

    public void GameEnded()
    {
        inputManager.DisableInputs();
        
    }

}

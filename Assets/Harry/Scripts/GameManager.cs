using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public MultiplayerInputManager inputManager;

    public static int playerOne;
    public static int playerTwo;
    public static int loserID = 5;

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
        loserID = id;
        StartCoroutine(ReturnToMain());
        Debug.Log(loserID);

    }

    IEnumerator ReturnToMain()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Don't Stop Smithing");
    }

}
